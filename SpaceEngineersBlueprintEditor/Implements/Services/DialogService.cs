using SpaceEngineersBlueprintEditor.Interface.Services;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

public class DialogService : IDialogService
{
    private readonly Dictionary<string, ContentDialog> dialogDictionary = [];
    public Dictionary<string, ContentDialog> DialogDictionary => dialogDictionary;
}
