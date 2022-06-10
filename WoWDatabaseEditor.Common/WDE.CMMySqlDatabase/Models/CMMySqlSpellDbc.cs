using LinqToDB.Mapping;
using WDE.Common.Database;

namespace WDE.CMMySqlDatabase.Models
{
    [Table(Name = "spell_dbc")]
    public class TrinityMySqlSpellDbc : IDatabaseSpellDbc
    {
        [PrimaryKey]
        [Column(Name = "Id")]
        public uint Id { get; set; }

        [Column(Name = "SpellName")]
        public string? Name { get; set; }
    }
    
    [Table(Name = "serverside_spell")]
    public class TrinityMasterMySqlServersideSpell : IDatabaseSpellDbc
    {
        [PrimaryKey]
        [Column(Name = "Id")]
        public uint Id { get; set; }

        [Column(Name = "SpellName")]
        public string? Name { get; set; }
    }
}