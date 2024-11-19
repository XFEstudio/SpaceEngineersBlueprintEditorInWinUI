namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface ISettingService
{
    Dictionary<object, IProfileInfoEntry> SettingControls { get; }
    void AddComboBox(ComboBox comboBox, Func<string, object?> saveFunc, Func<List<object>, object?, object?> loadFunc);
    void AddTextBlock(TextBlock textBlock, Action<string?> loadFunc);
    void Initialize();
    void RegisterEvents();
}
