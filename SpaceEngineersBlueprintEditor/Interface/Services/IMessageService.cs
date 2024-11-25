using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;

namespace SpaceEngineersBlueprintEditor.Interface.Services;

/// <summary>
/// 全局消息显示服务
/// </summary>
public interface IMessageService : IGlobalService
{
    /// <summary>
    /// 消息堆栈
    /// </summary>
    ReadOnlyDictionary<string, InfoBar> MessageStack { get; }
    /// <summary>
    /// 显示一条消息
    /// </summary>
    /// <param name="message">消息文本</param>
    /// <param name="title">消息标题（可选）</param>
    /// <returns>是否显示成功</returns>
    bool ShowMessage(string message, string title = "");
    /// <summary>
    /// 显示一条消息
    /// </summary>
    /// <param name="message">消息文本</param>
    /// <param name="title">消息标题</param>
    /// <param name="severity">消息的提示等级</param>
    /// <returns>是否显示成功</returns>
    bool ShowMessage(string message, string title, InfoBarSeverity severity);
    /// <summary>
    /// 显示一条带按钮的消息
    /// </summary>
    /// <param name="message">消息文本</param>
    /// <param name="title">消息标题</param>
    /// <param name="buttonText">按钮文本</param>
    /// <param name="callBackAction">按钮回调方法</param>
    /// <param name="severity">提示等级</param>
    /// <param name="canClose">是否可以被关闭</param>
    /// <returns>是否显示成功</returns>
    bool ShowButtonMessage(string message, string title, string buttonText, Action<object, RoutedEventArgs> callBackAction, InfoBarSeverity severity = InfoBarSeverity.Informational, bool canClose = true);
    /// <summary>
    /// 显示一条消息
    /// </summary>
    /// <param name="message">消息文本</param>
    /// <param name="title">消息标题</param>
    /// <param name="content">消息自定义内容</param>
    /// <param name="severity">消息的提示等级</param>
    /// <param name="time">消息持续时间</param>
    /// <param name="canClose">是否可被关闭</param>
    /// <param name="buttonText">按钮文本</param>
    /// <param name="callBackAction">按钮回调方法</param>
    /// <param name="messageId">消息的ID</param>
    /// <returns>是否显示成功</returns>
    bool ShowMessage(string message, string title = "", object? content = null, InfoBarSeverity severity = InfoBarSeverity.Informational, double time = 5, bool canClose = true, string buttonText = "", Action<object, RoutedEventArgs>? callBackAction = null, string messageId = "");
    /// <summary>
    /// 显示一条消息，使用指定ID
    /// </summary>
    /// <param name="messageId">消息ID</param>
    /// <param name="message">消息文本</param>
    /// <param name="title">消息标题</param>
    /// <param name="severity">消息的提示等级</param>
    /// <param name="time">消息的持续时间</param>
    /// <returns>是否显示成功</returns>
    bool ShowMessageWithId(string messageId, string message, string title = "", InfoBarSeverity severity = InfoBarSeverity.Informational, double time = 5);
    /// <summary>
    /// 显示一条消息，使用自定义InfoBar控件
    /// </summary>
    /// <param name="infoBar">自定义InfoBar控件</param>
    /// <param name="time">消息的持续时间</param>
    /// <param name="messageId">消息ID</param>
    /// <returns>是否显示成功</returns>
    bool ShowMessage(InfoBar infoBar, double time = -1, string messageId = "");
    /// <summary>
    /// 根据消息ID获取一条消息
    /// </summary>
    /// <param name="messageId">消息ID</param>
    /// <returns>InfoBar控件</returns>
    InfoBar? GetMessage(string messageId);
    /// <summary>
    /// 移除一条消息
    /// </summary>
    /// <param name="messageId">消息ID</param>
    /// <returns>是否移除成功</returns>
    bool RemoveMessage(string messageId);
    /// <summary>
    /// 清除所有消息列表
    /// </summary>
    void Clear();
    /// <summary>
    /// 初始化消息显示服务
    /// </summary>
    /// <param name="stackPanel">需要显示的消息的StackPanel控件</param>
    /// <param name="dispatcherQueue">目标DispatcherQueue</param>
    void Initialize(StackPanel stackPanel, DispatcherQueue dispatcherQueue);
}
