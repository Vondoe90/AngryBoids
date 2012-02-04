using System.Runtime.CompilerServices;
namespace CryEngine{
    public partial class ActorSystem    {
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static float _GetPlayerHealth(uint uint0);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _SetPlayerHealth(uint uint0, float float1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static float _GetPlayerMaxHealth(uint uint0);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _SetPlayerMaxHealth(uint uint0, float float1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _RegisterActorClass(string string0, bool bool1);
    }
}
namespace CryEngine{
    public partial class ItemSystem    {
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _CacheItemGeometry(string string0);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _CacheItemSound(string string0);
    }
}
namespace CryEngine{
    public partial class Inventory    {
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _GiveItem(uint uint0, string string1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _GiveEquipmentPack(uint uint0, string string1);
    }
}
namespace CryEngine{
    public partial class CryConsole    {
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _RegisterCommand(string string0, string string1, EVarFlags EVarFlags2);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _RegisterCVarFloat(string string0,ref float float1, float float2, EVarFlags EVarFlags3, string string4);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _RegisterCVarInt(string string0,ref int int1, int int2, EVarFlags EVarFlags3, string string4);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _RegisterCVarString(string string0,ref string string1, string string2, EVarFlags EVarFlags3, string string4);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static bool _HasCVar(string string0);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static float _GetCVarFloat(string string0);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static int _GetCVarInt(string string0);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static string _GetCVarString(string string0);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _SetCVarFloat(string string0, float float1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _SetCVarInt(string string0, int int1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _SetCVarString(string string0, string string1);
    }
}
namespace CryEngine{
    public partial class GameRulesSystem    {
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _RegisterGameMode(string string0);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _AddGameModeAlias(string string0, string string1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _AddGameModeLevelLocation(string string0, string string1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _SetDefaultGameMode(string string0);
    }
}
namespace CryEngine{
    public partial class EntitySystem    {
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static uint _SpawnEntity(ref EntitySpawnParams EntitySpawnParams0, bool bool1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static bool _RegisterEntityClass(EntityRegisterParams EntityRegisterParams0, object[] object1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static string _GetPropertyValue(uint uint0, string string1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _SetPropertyValue(uint uint0, string string1, string string2);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static uint _FindEntity(string string0);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static object[] _GetEntitiesByClass(string string0);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _SetWorldPos(uint uint0, Vec3 Vec31);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static Vec3 _GetWorldPos(uint uint0);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _SetWorldAngles(uint uint0, Vec3 Vec31);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static Vec3 _GetWorldAngles(uint uint0);
    }
}
namespace CryEngine{
    public partial class FlowSystem    {
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _RegisterNode(string string0, string string1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static bool _IsPortActive(int int0, int int1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _SetRegularlyUpdated(int int0, bool bool1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _ActivateOutput(int int0, int int1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _ActivateOutputInt(int int0, int int1, int int2);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _ActivateOutputFloat(int int0, int int1, float float2);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _ActivateOutputEntityId(int int0, int int1, uint uint2);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _ActivateOutputString(int int0, int int1, string string2);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _ActivateOutputBool(int int0, int int1, bool bool2);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _ActivateOutputVec3(int int0, int int1, Vec3 Vec32);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static int _GetPortValueInt(int int0, int int1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static float _GetPortValueFloat(int int0, int int1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static uint _GetPortValueEntityId(int int0, int int1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static string _GetPortValueString(int int0, int int1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static bool _GetPortValueBool(int int0, int int1);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static object _GetPortValueVec3(int int0, int int1);
    }
}
namespace CryEngine.Utils{
    public partial class Tester    {
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static void _TestScriptBind(string string0, int int1, object[] object2);
    }
}
