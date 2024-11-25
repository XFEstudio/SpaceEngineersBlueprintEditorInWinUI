namespace SpaceEngineersBlueprintEditor.Interface.Services;

/// <summary>
/// 对话框弹出服务
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// 对话框字典
    /// </summary>
    Dictionary<string, ContentDialog> DialogDictionary { get; }
    /// <summary>
    /// 注册对话框控件
    /// </summary>
    /// <param name="contentDialog">对话框控件</param>
    void RegisterDialog(ContentDialog contentDialog) => DialogDictionary.Add(contentDialog.Name, contentDialog);
    /// <summary>
    /// 显示一个对话框
    /// </summary>
    /// <param name="dialogName">需要显示的对话框名称</param>
    /// <returns>对话框返回结果（异步）</returns>
    async Task<ContentDialogResult> ShowDialog(string dialogName) => await DialogDictionary[dialogName].ShowAsync();
}
