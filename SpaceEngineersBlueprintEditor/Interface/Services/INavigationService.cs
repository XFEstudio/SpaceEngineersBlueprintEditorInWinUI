﻿namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface INavigationService : INavigable
{
    event EventHandler<Type> Navigated;
    bool CanGoBack { get; }
    bool CanGoForward { get; }
    Frame? Frame { get; set; }
    List<Page> NavigationStack { get; }
    public void GoBack();
}