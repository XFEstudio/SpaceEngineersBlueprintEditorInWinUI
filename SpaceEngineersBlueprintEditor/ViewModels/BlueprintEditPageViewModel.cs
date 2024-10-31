﻿using SpaceEngineersBlueprintEditor.Implements.Services;
using SpaceEngineersBlueprintEditor.Interface.Services;
using SpaceEngineersBlueprintEditor.Model;

namespace SpaceEngineersBlueprintEditor.ViewModels;

public partial class BlueprintEditPageViewModel : ViewModelBase
{
    public INavigationParameterService<BlueprintInfoViewData> NavigationParameterService { get; set; } = new NavigationParameterService<BlueprintInfoViewData>();

    public BlueprintEditPageViewModel()
    {
        NavigationParameterService.ParameterChange += NavigationParameterService_ParameterChange;
    }

    private void NavigationParameterService_ParameterChange(object? sender, BlueprintInfoViewData e)
    {

    }
}
