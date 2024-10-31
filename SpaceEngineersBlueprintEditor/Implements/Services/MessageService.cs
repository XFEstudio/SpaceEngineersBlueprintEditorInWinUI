using Microsoft.UI.Dispatching;
using SpaceEngineersBlueprintEditor.Interface.Services;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using XFEExtension.NetCore.StringExtension;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

class MessageService : GlobalServiceBase, IMessageService
{
    private StackPanel? messageStackPanel;
    private DispatcherQueue? _dispatcherQueue;
    private readonly Dictionary<string, InfoBar> messageStack = [];

    public ReadOnlyDictionary<string, InfoBar> MessageStack => new(messageStack);

    public InfoBar? GetMessage(string messageId) => messageStack.TryGetValue(messageId, out InfoBar? value) ? value : null;

    [MemberNotNull(nameof(messageStackPanel), nameof(_dispatcherQueue))]
    public void Initialize(StackPanel stackPanel, DispatcherQueue dispatcherQueue)
    {
        messageStackPanel = stackPanel;
        _dispatcherQueue = dispatcherQueue;
    }

    public bool RemoveMessage(string messageId)
    {
        if (messageStackPanel is not null && messageStack.TryGetValue(messageId, out InfoBar? value))
        {
            messageStackPanel.Children.Remove(value);
            return true;
        }
        return false;
    }

    public bool ShowMessage(string message, string title = "", object? content = null, InfoBarSeverity severity = InfoBarSeverity.Informational, double time = 5, bool canClose = true, string buttonText = "", Action<object, RoutedEventArgs>? callBackAction = null, string messageId = "")
    {
        return _dispatcherQueue is not null && _dispatcherQueue.TryEnqueue(() =>
        {
            ShowMessage(ConstructInfoBar(message, title, content, severity, canClose, buttonText, callBackAction), time, messageId);
        });
    }

    public bool ShowMessage(InfoBar infoBar, double time = -1, string messageId = "")
    {
        if (messageId.IsNullOrEmpty())
            messageId = Guid.NewGuid().ToString();
        if (messageStack.TryAdd(messageId, infoBar) && messageStackPanel is not null)
        {
            infoBar.Closed += InfoBar_Closed;
            StartTimeCounter(infoBar, time);
            messageStackPanel.Children.Add(infoBar);
            return true;
        }
        return false;
    }

    private void InfoBar_Closed(InfoBar sender, InfoBarClosedEventArgs args)
    {
        string messageId = "";
        foreach (var entry in messageStack)
            if (entry.Value == sender)
                messageId = entry.Key;
        if (messageStack.ContainsKey(messageId))
        {
            messageStackPanel?.Children.Remove(sender);
            messageStack.Remove(messageId);
        }
    }

    public bool ShowMessageWithId(string messageId, string message, string title = "", InfoBarSeverity severity = InfoBarSeverity.Informational, double time = 5) => ShowMessage(message, title, null, severity, time, true, "", null, messageId);

    public void Clear()
    {
        foreach (var entry in messageStack)
            RemoveMessage(entry.Key);
    }

    private void StartTimeCounter(InfoBar infoBar, double time) => Task.Run(async () =>
    {
        await Task.Delay(TimeSpan.FromSeconds(time));
        string messageId = "";
        foreach (var entry in messageStack)
            if (entry.Value == infoBar)
                messageId = entry.Key;
        _dispatcherQueue?.TryEnqueue(() =>
        {
            if (messageStack.ContainsKey(messageId))
            {
                messageStackPanel?.Children.Remove(infoBar);
                messageStack.Remove(messageId);
            }
        });
    });

    private static InfoBar ConstructInfoBar(string message, string title, object? content, InfoBarSeverity severity, bool canClose, string buttonText, Action<object, RoutedEventArgs>? callBackAction)
    {
        var infoBar = new InfoBar()
        {
            Message = message,
            Title = title,
            IsOpen = true,
            Severity = severity,
            IsClosable = canClose
        };
        if (content is not null)
            infoBar.Content = content;
        if (buttonText is not null && callBackAction is not null)
        {
            var actionButton = new Button
            {
                Content = buttonText
            };
            actionButton.Click += (sender, e) => callBackAction.Invoke(sender, e);
            infoBar.ActionButton = actionButton;
        }
        return infoBar;
    }

    public bool ShowMessage(string message, string title = "") => ShowMessage(message, title, InfoBarSeverity.Informational);

    public bool ShowMessage(string message, string title, InfoBarSeverity severity) => ShowMessageWithId("", message, title, severity);

    public bool ShowButtonMessage(string message, string title, string buttonText, Action<object, RoutedEventArgs> callBackAction, InfoBarSeverity severity = InfoBarSeverity.Informational, bool canClose = true) => ShowMessage(message, title, null, severity, -1, true, buttonText, callBackAction, "");
}
