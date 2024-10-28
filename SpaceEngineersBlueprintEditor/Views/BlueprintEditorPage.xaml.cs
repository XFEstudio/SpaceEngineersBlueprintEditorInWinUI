using Microsoft.UI.Xaml.Media.Imaging;
using SpaceEngineersBlueprintEditor.Utilities;
using System.Diagnostics;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

namespace SpaceEngineersBlueprintEditor.Views;

/// <summary>
/// À¶Í¼±à¼­Ò³Ãæ
/// </summary>
public sealed partial class BlueprintEditorPage : Page
{
    public static BlueprintEditorPage? Current { get; set; }
    public BlueprintEditorPage()
    {
        PageManager.AddOrUpdateCurrentPage(Current = this);
        this.InitializeComponent();
    }

    private async void Grid_Drop(object sender, DragEventArgs e)
    {
        if (e.DataView.Contains(StandardDataFormats.StorageItems))
        {
            var items = await e.DataView.GetStorageItemsAsync();
            if (items.Count > 0)
            {
                var storageFile = items[0] as StorageFile;
                Debug.WriteLine(storageFile.Path);
            }
        }
    }

    private void Grid_DragOver(object sender, DragEventArgs e)
    {
        e.AcceptedOperation = DataPackageOperation.Copy;
        e.DragUIOverride.Caption = "DragFileToEditorText".GetLocalized();
        e.DragUIOverride.SetContentFromBitmapImage(new BitmapImage(new Uri("ms-appx:///Assets/Images/BlueprintDrag.png", UriKind.RelativeOrAbsolute)));
        e.DragUIOverride.IsGlyphVisible = false;
    }
}