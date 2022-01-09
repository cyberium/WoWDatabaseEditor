﻿using System.Collections;
using WDE.Common.DBC;

namespace WDE.MpqReader.Structures
{
    public enum AreaTriggerShape
    {
        Box,
        Sphere
    }
    
    public class AreaTrigger
    {
        public readonly int Id;
        public readonly int ContinentId;
        public readonly float X;
        public readonly float Y;
        public readonly float Z;
        public readonly float Radius;
        public readonly float BoxLength;
        public readonly float BoxWidth;
        public readonly float BoxHeight;
        public readonly float BoxYaw;

        public AreaTriggerShape Shape => Radius > 0 ? AreaTriggerShape.Sphere : AreaTriggerShape.Box;
        
        public AreaTrigger(IDbcIterator dbcIterator)
        {
            Id = dbcIterator.GetInt(0);
            ContinentId = dbcIterator.GetInt(1);
            X = dbcIterator.GetFloat(2);
            Y = dbcIterator.GetFloat(3);
            Z = dbcIterator.GetFloat(4);
            Radius = dbcIterator.GetFloat(5);
            BoxLength = dbcIterator.GetFloat(6);
            BoxWidth = dbcIterator.GetFloat(7);
            BoxHeight = dbcIterator.GetFloat(8);
            BoxYaw = dbcIterator.GetFloat(9);
        }

        private AreaTrigger()
        {
            Id = -1;
            ContinentId = -1;
            X = 0;
            Y = 0;
            Z = 0;
            Radius = 0;
            BoxLength = 0;
            BoxWidth = 0;
            BoxHeight = 0;
            BoxYaw = 0;
        }

        public static AreaTrigger Empty => new AreaTrigger();
    }

    public class AreaTriggerStore : IEnumerable<AreaTrigger>
    {
        private Dictionary<int, AreaTrigger> store = new();
        public AreaTriggerStore(IEnumerable<IDbcIterator> rows)
        {
            foreach (var row in rows)
            {
                var o = new AreaTrigger(row);
                store[o.Id] = o;
            }
        }

        public bool Contains(int id) => store.ContainsKey(id);
        public AreaTrigger this[int id] => store[id];
        public IEnumerator<AreaTrigger> GetEnumerator() => store.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => store.Values.GetEnumerator();
    }
}