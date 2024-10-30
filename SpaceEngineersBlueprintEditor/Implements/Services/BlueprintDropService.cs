using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using SpaceEngineersBlueprintEditor.Interface.Services;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI;

namespace SpaceEngineersBlueprintEditor.Implements.Services;

internal class BlueprintDropService : IFileDropService
{
    private string currentPath = "";
    private UIElement? dropContainerElement;
    private Compositor? _compositor;
    private SpringVector3NaturalMotionAnimation? _springAnimation;
    public event EventHandler<(string, DragEventArgs)>? Drop;

    [MemberNotNull(nameof(dropContainerElement), nameof(_compositor))]
    public void Initialize(UIElement element, Compositor compositor)
    {
        _compositor = compositor;
        dropContainerElement = element;
        dropContainerElement.DragEnter += DropContainerElement_DragEnter;
        dropContainerElement.DragLeave += DropContainerElement_DragLeave;
        dropContainerElement.Drop += DropContainerElement_Drop;
    }

    private void DropContainerElement_DragEnter(object sender, DragEventArgs e)
    {
        if (e.DataView.Contains(StandardDataFormats.StorageItems))
        {
            var waiter = e.DataView.GetStorageItemsAsync();
            waiter.Wait();
            var items = waiter.GetResults();
            if (items.Count == 1 && items[0] is StorageFile storageFile && storageFile.Name.ToLower().EndsWith(".sbc"))
            {
                currentPath = storageFile.Path;
                if (dropContainerElement is not null && _compositor is not null)
                {
                    CreateOrUpdateSpringAnimation(1.3f);
                    dropContainerElement.StartAnimation(_springAnimation);
                    if (dropContainerElement is Panel panel)
                        panel.Background = new SolidColorBrush(Color.FromArgb(128, 128, 128, 128));
                }
                e.AcceptedOperation = DataPackageOperation.Copy;
                e.DragUIOverride.Caption = "DragFileToEditorText".GetLocalized();
                e.DragUIOverride.SetContentFromBitmapImage(new BitmapImage(new Uri("ms-appx:///Assets/Images/BlueprintDrag.png", UriKind.RelativeOrAbsolute)));
                e.DragUIOverride.IsGlyphVisible = false;
            }
        }
    }

    private void DropContainerElement_DragLeave(object sender, DragEventArgs e)
    {
        if (dropContainerElement is not null && _compositor is not null)
        {
            CreateOrUpdateSpringAnimation(1.0f);
            dropContainerElement.StartAnimation(_springAnimation);
            if (dropContainerElement is Panel panel)
                panel.Background = new SolidColorBrush();
        }
    }

    private void DropContainerElement_Drop(object sender, DragEventArgs e)
    {
        if (dropContainerElement is not null && _compositor is not null)
        {
            CreateOrUpdateSpringAnimation(1.0f);
            dropContainerElement.StartAnimation(_springAnimation);
            if (dropContainerElement is Panel panel)
                panel.Background = new SolidColorBrush();
        }
        Drop?.Invoke(this, (currentPath, e));
    }

    private void CreateOrUpdateSpringAnimation(float finalValue)
    {
        if (_springAnimation is null)
        {
            _springAnimation = (_compositor ??= App.MainWindow.Compositor).CreateSpringVector3Animation();
            _springAnimation.Target = "Scale";
        }
        _springAnimation.FinalValue = new Vector3(finalValue);
    }
}