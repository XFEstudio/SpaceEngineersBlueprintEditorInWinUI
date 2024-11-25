using SpaceEngineersBlueprintEditor.Interface;

namespace SpaceEngineersBlueprintEditor.Model;

/// <summary>
/// ComboBox配置文件条目
/// </summary>
/// <param name="ProfilePath">配置文件访问路径</param>
/// <param name="ProfileSaveFunc">配置文件保存方法</param>
/// <param name="ProfileLoadFunc">配置文件加载方法</param>
public record class ComboBoxProfileInfoEntry(string ProfilePath, Func<string, object?> ProfileSaveFunc, Func<List<object>, object?, object?> ProfileLoadFunc) : IProfileInfoEntry { }