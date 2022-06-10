using WDE.Module;
using WDE.Module.Attributes;

[assembly:ModuleRequiresCore("WDE.CMMySqlDatabase")] //<--- put the name you gave the "cmangos" core here
[assembly:ModuleBlocksOtherAttribute("WDE.TrinityMySqlDatabase")]

namespace WDE.CMaNGOS
{
    public class CMaNGOSModule : ModuleBase
    {
    }
}