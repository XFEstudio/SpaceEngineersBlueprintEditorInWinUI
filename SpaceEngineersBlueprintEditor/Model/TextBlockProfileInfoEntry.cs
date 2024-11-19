using SpaceEngineersBlueprintEditor.Interface;

namespace SpaceEngineersBlueprintEditor.Model;

public record class TextBlockProfileInfoEntry(string ProfilePath, Action<string?> ProfileLoadFunc) : IProfileInfoEntry { }