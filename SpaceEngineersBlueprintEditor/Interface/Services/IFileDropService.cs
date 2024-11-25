using Microsoft.UI.Composition;

namespace SpaceEngineersBlueprintEditor.Interface.Services;

/// <summary>
/// 文件拖拽服务
/// </summary>
public interface IFileDropService
{
    /// <summary>
    /// 文件拖拽完成事件
    /// </summary>
    event EventHandler<(string, DragEventArgs)> Drop;
    /// <summary>
    /// 初始化文件拖拽服务
    /// </summary>
    /// <param name="element">拖拽的目标接受控件</param>
    /// <param name="compositor">目标Compositor</param>
    void Initialize(UIElement element, Compositor compositor);
}
