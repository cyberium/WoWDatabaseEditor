using LinqToDB;
using LinqToDB.Data;
//using WDE.MySqlDatabaseCommon.CommonModels;

namespace WDE.CMMySqlDatabase.Models
{
    public class CMaNGOSAuthDatabase : DataConnection
    {
        public CMaNGOSAuthDatabase() : base("CMaNGOSAuth")
        {
        }
// 
//         public ITable<RbacPermission> RbacPermissions => GetTable<RbacPermission>();
//         public ITable<RbacLinkedPermission> RbacLinkedPermissions => GetTable<RbacLinkedPermission>();
    }
}