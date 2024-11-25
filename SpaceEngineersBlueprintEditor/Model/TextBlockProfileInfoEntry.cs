using SpaceEngineersBlueprintEditor.Interface;

namespace SpaceEngineersBlueprintEditor.Model;

/// <summary>
/// TextBlock配置文件条目
/// </summary>
/// <param name="ProfilePath">配置文件访问路径</param>
/// <param name="ProfileLoadFunc">配置文件加载方法</param>
public record class TextBlockProfileInfoEntry(string ProfilePath, Action<string?> ProfileLoadFunc) : IProfileInfoEntry { }