using Microsoft.UI.Xaml.Media;

namespace SpaceEngineersBlueprintEditor.Interface.Services;

/// <summary>
/// 全局背景图片设置服务
/// </summary>
public interface IBackgroundImageService : IGlobalService
{
    /// <summary>
    /// 初始化背景图片设置服务
    /// </summary>
    /// <param name="page">需要设置背景图片的页面</param>
    /// <param name="grid">亚克力层网格</param>
    void Initialize(Page page, Grid grid);
    /// <summary>
    /// 设置背景图片
    /// </summary>
    /// <param name="imageSource">背景图片</param>
    void SetBackgroundImage(ImageSource imageSource);
    /// <summary>
    /// 重置背景图片
    /// </summary>
    void ResetBackground();
}
