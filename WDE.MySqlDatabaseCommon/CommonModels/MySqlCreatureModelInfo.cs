using LinqToDB.Mapping;
using WDE.Common.Database;

namespace WDE.MySqlDatabaseCommon.CommonModels;

[Table(Name = "creature_model_info")]
public abstract class MySqlCreatureModelInfoBase : ICreatureModelInfo
{
    [PrimaryKey]
    [Column(Name = "DisplayID")]
    public uint DisplayId { get; set; }
    
    [Column(Name = "BoundingRadius")]
    public float BoundingRadius { get; set; }
    
    [Column(Name = "CombatReach")]
    public float CombatReach { get; set; }

    public abstract uint Gender { get; set; }
}

[Table(Name = "creature_model_info")]
public class MySqlCreatureModelInfoShadowlands : MySqlCreatureModelInfoBase
{
    public override uint Gender { get; set; }
}

[Table(Name = "creature_model_info")]
public class MySqlCreatureModelInfo : MySqlCreatureModelInfoBase
{
    [Column(Name = "Gender")]
    public override uint Gender { get; set; }
}

/// <summary>
/// Creature System (Model related info)
/// </summary>
[Table("creature_model_info")]
public class CMWrathMySqlCreatureModelInfo : ICreatureModelInfo
{
    [Column("modelid"             , IsPrimaryKey = true)] public uint  DisplayId          { get; set; } // mediumint(8) unsigned
    [Column("bounding_radius"                          )] public float BoundingRadius     { get; set; } // float
    [Column("combat_reach"                             )] public float CombatReach        { get; set; } // float
    /// <summary>
    /// Default walking speed for any creature with model
    /// </summary>
    [Column("SpeedWalk"                                )] public float SpeedWalk          { get; set; } // float
    /// <summary>
    /// Default running speed for any creature with model
    /// </summary>
    [Column("SpeedRun"                                 )] public float SpeedRun           { get; set; } // float
    [Column("gender"                                   )] public uint  Gender             { get; set; } // tinyint(3) unsigned
    [Column("modelid_other_gender"                     )] public uint  ModelidOtherGender { get; set; } // mediumint(8) unsigned
    [Column("modelid_alternative"                      )] public uint  ModelidAlternative { get; set; } // mediumint(8) unsigned
}
