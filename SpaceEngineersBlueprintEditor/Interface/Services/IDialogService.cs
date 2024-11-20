namespace SpaceEngineersBlueprintEditor.Interface.Services;

public interface IDialogService
{
    Dictionary<string, ContentDialog> DialogDictionary { get; }
    void RegisterDialog(ContentDialog contentDialog) => DialogDictionary.Add(contentDialog.Name, contentDialog);
    async Task<ContentDialogResult> ShowDialog(string dialogName) => await DialogDictionary[dialogName].ShowAsync();
}
