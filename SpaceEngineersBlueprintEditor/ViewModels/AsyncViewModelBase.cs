using Microsoft.UI.Dispatching;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public abstract class AsyncViewModelBase(DispatcherQueue dispatcherQueue) : ViewModelBase
{
    protected DispatcherQueue _dispatcherQueue = dispatcherQueue;
    protected void OnPropertyChangedAsync(string? propertyName = null) => _dispatcherQueue.TryEnqueue(() => OnPropertyChanged(propertyName));
}
