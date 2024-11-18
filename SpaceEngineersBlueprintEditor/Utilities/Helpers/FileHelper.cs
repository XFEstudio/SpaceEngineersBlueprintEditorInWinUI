using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.InteropServices;
using VRage.FileSystem;
using Windows.Storage.Streams;
using Microsoft.UI.Xaml.Media.Imaging;
using SpaceEngineersBlueprintEditor.SpaceEngineersCore;

namespace SpaceEngineersBlueprintEditor.Utilities.Helpers;

public static class FileHelper
{
    public static string GetCopyFileName(string originalName, int currentNum = 1)
    {
        var currentFileName = @$"{Path.GetDirectoryName(originalName)}\{Path.GetFileNameWithoutExtension(originalName)}_{currentNum}{Path.GetExtension(originalName)}";
        if (File.Exists(currentFileName) || Directory.Exists(currentFileName))
            return GetCopyFileName(originalName, ++currentNum);
        return currentFileName;
    }

    public static void CopyDirectory(string sourceDirectory, string targetDirectory, bool overwrite = false)
    {
        if (!Directory.Exists(sourceDirectory)) throw new DirectoryNotFoundException($"源目录未找到: {sourceDirectory}");
        if (!Directory.Exists(targetDirectory))
            Directory.CreateDirectory(targetDirectory);
        foreach (string file in Directory.GetFiles(sourceDirectory))
        {
            string targetFile = Path.Combine(targetDirectory, Path.GetFileName(file));
            File.Copy(file, targetFile, overwrite);
        }
        foreach (string subDir in Directory.GetDirectories(sourceDirectory))
        {
            string subDirectoryName = Path.GetFileName(subDir);
            string newTargetDirectory = Path.Combine(targetDirectory, subDirectoryName);
            CopyDirectory(subDir, newTargetDirectory, overwrite);
        }
    }

    public static async Task<BitmapImage?> ToBitmap(string ddsFilePath)
    {
        try
        {
            using var readStream = MyFileSystem.OpenRead(ddsFilePath);
            int width = -1;
            int height = -1;
            if (ImageConverter.TextureToRgbaBuffer(readStream, 0, ref width, ref height) is byte[] buffer)
            {
                var bmp = new System.Drawing.Bitmap(width, height, PixelFormat.Format32bppArgb);
                var bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                Marshal.Copy(buffer, 0, bmpData.Scan0, buffer.Length);
                bmp.UnlockBits(bmpData);
                using var saveStream = new MemoryStream();
                bmp.Save(saveStream, ImageFormat.Png);
                saveStream.Seek(0, SeekOrigin.Begin);
                var randomAccessStream = new InMemoryRandomAccessStream();
                await randomAccessStream.WriteAsync(saveStream.ToArray().AsBuffer());
                randomAccessStream.Seek(0);
                var bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(randomAccessStream);
                return bitmapImage;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }
}
