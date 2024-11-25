namespace SpaceEngineersBlueprintEditor.Interface.Services;

/// <summary>
/// 设置服务
/// </summary>
public interface ISettingService
{
    /// <summary>
    /// 设置控件字典
    /// </summary>
    Dictionary<object, IProfileInfoEntry> SettingControls { get; }
    /// <summary>
    /// 添加一个ComboBox控件的设置选项
    /// </summary>
    /// <param name="comboBox">目标控件</param>
    /// <param name="saveFunc">保存方法</param>
    /// <param name="loadFunc">加载方法</param>
    void AddComboBox(ComboBox comboBox, Func<string, object?> saveFunc, Func<List<object>, object?, object?> loadFunc);
    /// <summary>
    /// 添加一个TextBlock控件的设置选项
    /// </summary>
    /// <param name="textBlock">目标控件</param>
    /// <param name="loadFunc">加载方法</param>
    void AddTextBlock(TextBlock textBlock, Action<string?> loadFunc);
    /// <summary>
    /// 初始化设置服务
    /// </summary>
    void Initialize();
    /// <summary>
    /// 订阅事件
    /// </summary>
    void RegisterEvents();
}
