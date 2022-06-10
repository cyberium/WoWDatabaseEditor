﻿using LinqToDB.Mapping;
using WDE.Common.Database;

namespace WDE.CMMySqlDatabase.Models
{
    [Table(Name = "quest_template_addon")]
    public class MySqlQuestTemplateAddon
    {
        [PrimaryKey]
        [Column(Name = "ID")]
        public uint Entry { get; set; }

        [Column(Name = "PrevQuestId")]
        public int PrevQuestId { get; set; }

        [Column(Name = "NextQuestId")]
        public int NextQuestId { get; set; }

        [Column(Name = "ExclusiveGroup")]
        public int ExclusiveGroup { get; set; }
        
        [Column(Name = "BreadcrumbForQuestId")]
        public int BreadcrumbForQuest { get; set; }

        [Column(Name = "AllowableClasses")]
        public uint AllowableClasses { get; set; }
    }
    
    [Table(Name = "quest_template_addon")]
    public class MySqlCataQuestTemplateAddon : MySqlQuestTemplateAddon
    {
        [Column(Name="AllowableRaces")]
        public CharacterRaces AllowableRaces { get; set; }
    }

    [Table(Name = "quest_template_addon")]
    public class MySqlQuestTemplateAddonWithScriptName : MySqlQuestTemplateAddon
    {
        [Column(Name = "ScriptName")] 
        public string ScriptName { get; set; } = "";
    }
}