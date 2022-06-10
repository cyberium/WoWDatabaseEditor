using LinqToDB.Mapping;
using WDE.Common.Database;

namespace WDE.CMMySqlDatabase.Models
{
    [Table(Name = "spell_dbc")]
    public class AzerothMySqlSpellDbc : IDatabaseSpellDbc
    {
        [PrimaryKey]
        [Column(Name = "ID")]
        public uint Id { get; set; }

        [Column(Name = "Name_Lang_enUS")]
        public string? Name { get; set; }
    }
}