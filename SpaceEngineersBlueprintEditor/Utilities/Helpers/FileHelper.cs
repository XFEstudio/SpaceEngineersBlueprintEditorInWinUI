using SpaceEngineersBlueprintEditor.SpaceEngineersCore;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using VRage.FileSystem;

namespace SpaceEngineersBlueprintEditor.Utilities.Helpers;

public static class FileHelper
{
    public static long GetDirectorySize(DirectoryInfo directoryInfo)
    {
        long size = 0;
        FileInfo[] files = directoryInfo.GetFiles();
        foreach (FileInfo file in files)
        {
            size += file.Length;
        }
        DirectoryInfo[] directories = directoryInfo.GetDirectories();
        foreach (DirectoryInfo directory in directories)
        {
            size += GetDirectorySize(directory);
        }
        return size;
    }

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

    public static void ToBitmap(string ddsFilePath, string savePath)
    {
        try
        {
            using var readStream = MyFileSystem.OpenRead(ddsFilePath);
            int width = -1;
            int height = -1;
            if (ImageConverter.TextureToRgbaBuffer(readStream, 0, ref width, ref height) is byte[] buffer)
            {
                var bitmap = new System.Drawing.Bitmap(width, height, PixelFormat.Format32bppArgb);
                var data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);
                bitmap.UnlockBits(data);
                bitmap.Save(savePath, ImageFormat.Png);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    public static string ChangeExtension(string filePath, string extension) => @$"{Path.GetDirectoryName(filePath)}\{Path.GetFileNameWithoutExtension(filePath)}.{extension}";

    public static bool AutoCreateDirectory(string filePath)
    {
        if (Path.GetDirectoryName(filePath) is string directory && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            return true;
        }
        return false;
    }
}
