﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEngine.Data;
using TheEngine.ECS;
using TheEngine.Entities;
using TheEngine.Handles;
using TheEngine.Interfaces;
using TheMaths;
using WDE.MapRenderer.StaticData;
using WDE.MpqReader.Structures;
using WDE.Common.Database;
using WDE.MapRenderer;

namespace WDE.MapRenderer.Managers
{
    public class CreatureManager : System.IDisposable
    {
        public class CreatureChunkData
        {
            public List<StaticRenderHandle> registeredEntities = new();
        }
        
        private Random rng = new Random();
        private readonly IGameContext gameContext;
        private readonly IMeshManager meshManager;
        private readonly IRenderManager renderManager;
        private readonly DbcManager dbcManager;
        private readonly MdxManager mdxManager;
        private readonly IDatabaseProvider database;
        private PerChunkHolder<IList<ICreature>> CreatureDataPerChunk = new();
        private IList<ICreatureTemplate> CreatureTemplateData;
        private Transform transform = new Transform();
        private IMesh Mesh;
        private Material Material;
        // private TextureHandle Texture;
        // private readonly GameManager GameManager;
        // private readonly IDatabaseProvider database;

        public CreatureManager(IGameContext gameContext, 
            IMeshManager meshManager, 
            IMaterialManager materialManager,
            IRenderManager renderManager,
            DbcManager dbcManager,
            MdxManager mdxManager,
            IDatabaseProvider database)
        {
            this.gameContext = gameContext;
            this.meshManager = meshManager;
            this.renderManager = renderManager;
            this.dbcManager = dbcManager;
            this.mdxManager = mdxManager;
            this.database = database;
            //Mesh = gameContext.Engine.MeshManager.CreateMesh(ObjParser.LoadObj("meshes/sphere.obj").MeshData);
            Mesh = meshManager.CreateMesh(ObjParser.LoadObj("meshes/box.obj").MeshData);
            Material = materialManager.CreateMaterial("data/gizmo.json");
            Material.BlendingEnabled = true;
            Material.SourceBlending = Blending.SrcAlpha;
            Material.DestinationBlending = Blending.OneMinusSrcAlpha;
            Material.DepthTesting = DepthCompare.Lequal;
            Material.ZWrite = false;
            Material.SetUniform("objectColor", new Vector4(0.415f, 0.4f, 0.75f, 0.4f));            // this.database = database;
            CreatureTemplateData = database.GetCreatureTemplates().ToList();
        }

        public IEnumerator LoadEssentialData(CancellationToken cancel)
        {
            CreatureDataPerChunk.Clear();

            var mapCreaturesTask = database.GetCreaturesByMapAsync((uint)gameContext.CurrentMap.Id);

            yield return mapCreaturesTask;
            
            foreach (var creature in mapCreaturesTask.Result)
            {
                if (cancel.IsCancellationRequested)
                    yield break;
                
                var chunk = new Vector3(creature.X, creature.Y, creature.Z).WoWPositionToChunk();
                if (chunk.Item1 <= 0 || chunk.Item2 <= 0 || chunk.Item1 >= 64 || chunk.Item2 >= 64)
                    continue;
                
                if (CreatureDataPerChunk[chunk.Item1, chunk.Item2] == null)
                    CreatureDataPerChunk[chunk.Item1, chunk.Item2] = new List<ICreature>();
                
                CreatureDataPerChunk[chunk.Item1, chunk.Item2]!.Add(creature);
            }
        }

        // public bool OverrideLighting { get; set; }

        public void Dispose()
        {
            meshManager.DisposeMesh(Mesh);
        }

        public IEnumerator LoadCreatures(CreatureChunkData chunk, int chunkX, int chunkY, CancellationToken cancel)
        {
            if (CreatureDataPerChunk[chunkX, chunkY] == null)
                yield break;
            
            foreach (var creature in CreatureDataPerChunk[chunkX, chunkY]!)
            {
                if (cancel.IsCancellationRequested)
                    yield break;
                
                // check phasemask
                if ((creature.PhaseMask & 1) != 1)
                    continue;

                var creaturePosition = new Vector3(creature.X, creature.Y, creature.Z);

                transform.Position = creaturePosition.ToOpenGlPosition();
                transform.Rotation = Quaternion.FromEuler(0, MathUtil.RadiansToDegrees(-creature.O), 0.0f);
                float height = 0;

                ICreatureTemplate creaturetemplate = CreatureTemplateData.First(x => x.Entry == creature.Entry);

                if (dbcManager.CreatureDisplayInfoStore.Contains(creaturetemplate.GetModel(0)))
                {
                    // System.Diagnostics.Debug.WriteLine($"Cr Y :  {creature.Y}");

                    // randomly select one of the display ids of the creature
                    int numberOfModels = 0;
                    for (int i = 0; i < creaturetemplate.ModelsCount; ++i)
                    {
                        if (creaturetemplate.GetModel(i) > 0)
                            numberOfModels++;
                        else
                            break;
                    }

                    var randomModel = creaturetemplate.GetModel(rng.Next(numberOfModels));

                    CreatureDisplayInfo crdisplayinfo = dbcManager.CreatureDisplayInfoStore[randomModel];

                    string M2Path = dbcManager.CreatureModelDataStore[crdisplayinfo.ModelId].ModelName;

                    transform.Scale = new Vector3(crdisplayinfo.CreatureModelScale * creaturetemplate.Scale);

                    TaskCompletionSource<MdxManager.MdxInstance?> mdx = new();
                    yield return mdxManager.LoadM2Mesh(M2Path, mdx, crdisplayinfo.Id);
                    if (mdx.Task.Result == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Can't load {M2Path}"); //could not load mdx
                    }
                    else
                    {
                        int i = 0;
                        var instance = mdx.Task.Result;
                        height = instance.mesh.Bounds.Height / 2;
                        // position, rotation
                        foreach (var material in instance.materials)
                            chunk.registeredEntities.Add(renderManager.RegisterStaticRenderer(instance.mesh.Handle, material, i++, transform));

                        transform.Scale = instance.mesh.Bounds.Size / 2 ;
                        transform.Position  += instance.mesh.Bounds.Center;
                        chunk.registeredEntities.Add(renderManager.RegisterStaticRenderer(Mesh.Handle, Material, 0, transform));

                        // gameContext.Engine.Ui.DrawWorldText("calibri", new Vector2(0.5f, 1f), creaturetemplate.Name, 2.5f, Matrix.TRS(t.Position + Vector3.Up * height, in Quaternion.Identity, in Vector3.One));
                    }
                }
            }
        }

        public IEnumerator UnloadChunk(CreatureChunkData chunk)
        {
            foreach (var creature in chunk.registeredEntities)
            {
                renderManager.UnregisterStaticRenderer(creature);
            }

            yield break;
        }
    }
}