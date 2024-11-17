using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public abstract class AsyncViewModelBase(DispatcherQueue dispatcherQueue) : ObservableObject
{
    protected DispatcherQueue _dispatcherQueue = dispatcherQueue;
    protected void OnPropertyChangedAsync(string? propertyName = null) => _dispatcherQueue.TryEnqueue(() => OnPropertyChanged(propertyName));
}
