﻿using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Utilities;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

internal class NavigationService : INavigationService
{
    private Frame? frame;
    private readonly List<Page> navigationStack = [];
    public bool CanGoBack => navigationStack.Count > 1;
    public bool CanGoForward => Frame is not null && Frame.CanGoForward;
    public Frame? Frame { get => frame; set => frame = value; }

    public List<Page> NavigationStack => navigationStack;

    public event EventHandler<Type>? Navigated;

    public void GoBack()
    {
        if (navigationStack.Count > 1 && Frame is not null)
        {
            navigationStack.RemoveAt(navigationStack.Count - 1);
            var lastPage = navigationStack.Last();
            Frame.Content = lastPage;
            Navigated?.Invoke(Frame, lastPage.GetType());
        }
    }

    public void GoForward() => Frame?.GoForward();

    public void NavigateTo(Type type, object? parameter = null)
    {
        if (Frame is not null && (navigationStack.Count == 0 || navigationStack.Last().GetType() != type || parameter is not null && navigationStack.Last().GetParameter() != parameter))
        {
            if (PageManager.CurrentPages.TryGetValue(type.FullName!, out var page))
            {
                page.SetParameter(parameter);
                navigationStack.Add(page);
                Frame.Content = page;
            }
            else
            {
                var instance = Activator.CreateInstance(type);
                if (instance is Page pageInstance)
                {
                    pageInstance.SetParameter(parameter);
                    navigationStack.Add(pageInstance);
                    Frame.Content = pageInstance;
                }
            }
            Navigated?.Invoke(Frame, type);
        }
    }

    public void NavigateTo<T>(object? parameter = null) where T : Page => NavigateTo(typeof(T), parameter);

    public void NavigateTo(string pageName, object? parameter = null)
    {
        if (PageManager.PageDefinitions.TryGetValue(pageName, out var pageType))
            NavigateTo(pageType, parameter);
    }
}
