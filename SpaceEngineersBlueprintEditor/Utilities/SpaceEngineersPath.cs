using XFEExtension.NetCore.AutoPath;

namespace SpaceEngineersBlueprintEditor.Utilities;

public partial class SpaceEngineersPath
{
    [AutoPath]
    private static readonly string userGameDataRoot = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\SpaceEngineers";
    [AutoPath]
    private static readonly string blueprintsRoot = $@"{UserGameDataRoot}\Blueprints";
    [AutoPath]
    private static readonly string localBlueprints = $@"{BlueprintsRoot}\local";
    [AutoPath]
    private static readonly string cloudBlueprints = $@"{BlueprintsRoot}\cloud";
    [AutoPath]
    private static readonly string workshopBlueprints = $@"{BlueprintsRoot}\workshop\temp\Steam";
    public static string SpaceEngineerContentPath => $@"{SystemProfile.GameRootPath}\Content";
    public static string SpaceEngineerBinPath => $@"{SystemProfile.GameRootPath}\Bin64";
    public static string[] SpaceEngineerDynamicLibrary =>
    [
        $@"{SpaceEngineerBinPath}\EmptyKeys.UserInterface.Core.dll",
        $@"{SpaceEngineerBinPath}\EmptyKeys.UserInterface.dll",
        $@"{SpaceEngineerBinPath}\HavokWrapper.dll",
        $@"{SpaceEngineerBinPath}\ProtoBuf.Net.Core.dll",
        $@"{SpaceEngineerBinPath}\ProtoBuf.Net.dll",
        $@"{SpaceEngineerBinPath}\Sandbox.Common.dll",
        $@"{SpaceEngineerBinPath}\Sandbox.Game.dll",
        $@"{SpaceEngineerBinPath}\Sandbox.Game.XmlSerializers.dll",
        $@"{SpaceEngineerBinPath}\Sandbox.Graphics.dll",
        $@"{SpaceEngineerBinPath}\Sandbox.RenderDirect.dll",
        $@"{SpaceEngineerBinPath}\SpaceEngineers.Game.dll",
        $@"{SpaceEngineerBinPath}\SpaceEngineers.ObjectBuilders.dll",
        $@"{SpaceEngineerBinPath}\steam_api64.dll",
        $@"{SpaceEngineerBinPath}\VRage.dll",
        $@"{SpaceEngineerBinPath}\VRage.Ansel.dll",
        $@"{SpaceEngineerBinPath}\VRage.Audio.dll",
        $@"{SpaceEngineerBinPath}\VRage.Game.dll",
        $@"{SpaceEngineerBinPath}\VRage.Game.XmlSerializers.dll",
        $@"{SpaceEngineerBinPath}\VRage.Input.dll",
        $@"{SpaceEngineerBinPath}\VRage.Library.dll",
        $@"{SpaceEngineerBinPath}\VRage.Math.dll",
        $@"{SpaceEngineerBinPath}\VRage.Math.XmlSerializers.dll",
        $@"{SpaceEngineerBinPath}\VRage.Native.dll",
        $@"{SpaceEngineerBinPath}\VRage.NativeWrapper.dll",
        $@"{SpaceEngineerBinPath}\VRage.Network.dll",
        $@"{SpaceEngineerBinPath}\VRage.Render.dll",
        $@"{SpaceEngineerBinPath}\VRage.Render11.dll",
        $@"{SpaceEngineerBinPath}\VRage.Scripting.dll",
        $@"{SpaceEngineerBinPath}\VRage.Steam.dll",
        $@"{SpaceEngineerBinPath}\VRage.UserInterface.dll",
        $@"{SpaceEngineerBinPath}\VRage.XmlSerializers.dll"
    ];
}
