using SpaceEngineersBlueprintEditor.Interface;

namespace SpaceEngineersBlueprintEditor.Model;

public record class ComboBoxProfileInfoEntry(string ProfilePath, Func<string, object?> ProfileSaveFunc, Func<List<object>, object?, object?> ProfileLoadFunc) : IProfileInfoEntry { }