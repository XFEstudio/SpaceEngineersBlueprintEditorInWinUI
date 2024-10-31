using Microsoft.UI.Dispatching;
using System.Collections.ObjectModel;

namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface IMessageService : IGlobalService
{
    ReadOnlyDictionary<string, InfoBar> MessageStack { get; }
    bool ShowMessage(string message, string title = "");
    bool ShowMessage(string message, string title, InfoBarSeverity severity);
    bool ShowButtonMessage(string message, string title, string buttonText, Action<object, RoutedEventArgs> callBackAction, InfoBarSeverity severity = InfoBarSeverity.Informational, bool canClose = true);
    bool ShowMessage(string message, string title = "", object? content = null, InfoBarSeverity severity = InfoBarSeverity.Informational, double time = 5, bool canClose = true, string buttonText = "", Action<object, RoutedEventArgs>? callBackAction = null, string messageId = "");
    bool ShowMessageWithId(string messageId, string message, string title = "", InfoBarSeverity severity = InfoBarSeverity.Informational, double time = 5);
    bool ShowMessage(InfoBar infoBar, double time = -1, string messageId = "");
    InfoBar? GetMessage(string messageId);
    bool RemoveMessage(string messageId);
    void Clear();
    void Initialize(StackPanel stackPanel, DispatcherQueue dispatcherQueue);
}
