using XFEExtension.NetCore.AutoPath;

namespace SpaceEngineersBlueprintEditor.Utilities;

/// <summary>
/// 太空工程师路径
/// </summary>
public partial class SpaceEngineersPath
{
    /// <summary>
    /// 用户游戏数据根目录
    /// </summary>
    [AutoPath]
    private static readonly string userGameDataRoot = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\SpaceEngineers";
    /// <summary>
    /// 蓝图文件路径
    /// </summary>
    [AutoPath]
    private static readonly string blueprintsRoot = $@"{UserGameDataRoot}\Blueprints";
    /// <summary>
    /// 本地蓝图文件路径
    /// </summary>
    [AutoPath]
    private static readonly string localBlueprints = $@"{BlueprintsRoot}\local";
    /// <summary>
    /// 云端蓝图文件路径
    /// </summary>
    [AutoPath]
    private static readonly string cloudBlueprints = $@"{BlueprintsRoot}\cloud";
    /// <summary>
    /// 工坊蓝图文件路径
    /// </summary>
    [AutoPath]
    private static readonly string workshopBlueprints = $@"{BlueprintsRoot}\workshop\temp\Steam";
    /// <summary>
    /// 太空工程师游戏内容路径
    /// </summary>
    public static string SpaceEngineerContentPath => $@"{SystemProfile.GameRootPath}\Content";
    /// <summary>
    /// 太空工程师游戏Bin目录路径
    /// </summary>
    public static string SpaceEngineerBinPath => $@"{SystemProfile.GameRootPath}\Bin64";
    /// <summary>
    /// 太空工程师动态链接库路径集
    /// </summary>
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
