using SpaceEngineersBlueprintEditor.SpaceEngineersCore;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using VRage.FileSystem;

namespace SpaceEngineersBlueprintEditor.Utilities.Helpers;

/// <summary>
/// 文件帮助类
/// </summary>
public static class FileHelper
{
    /// <summary>
    /// 获取文件夹大小
    /// </summary>
    /// <param name="directoryInfo">文件夹信息</param>
    /// <returns>文件夹大小</returns>
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

    /// <summary>
    /// 获取目标文件的复制名称
    /// </summary>
    /// <param name="originalName">目标文件原始名称</param>
    /// <param name="currentNum">当前递归层数</param>
    /// <returns></returns>
    public static string GetCopyFileName(string originalName, int currentNum = 1)
    {
        var currentFileName = @$"{Path.GetDirectoryName(originalName)}\{Path.GetFileNameWithoutExtension(originalName)}_{currentNum}{Path.GetExtension(originalName)}";
        if (File.Exists(currentFileName) || Directory.Exists(currentFileName))
            return GetCopyFileName(originalName, ++currentNum);
        return currentFileName;
    }

    /// <summary>
    /// 获取目标文件夹的复制名称
    /// </summary>
    /// <param name="originalName">目标文件夹原始名称</param>
    /// <param name="currentNum">当前递归层数</param>
    /// <returns></returns>
    public static string GetCopyDirectoryName(string originalName, int currentNum = 1)
    {
        var currentDirectoryName = @$"{Path.GetDirectoryName(originalName)}\{Path.GetFileName(originalName)}_{currentNum}";
        if (File.Exists(currentDirectoryName) || Directory.Exists(currentDirectoryName))
            return GetCopyDirectoryName(originalName, ++currentNum);
        return currentDirectoryName;
    }

    /// <summary>
    /// 复制整个文件及其子文件夹
    /// </summary>
    /// <param name="sourceDirectory">源文件夹</param>
    /// <param name="targetDirectory">目标文件夹</param>
    /// <param name="overwrite">覆盖</param>
    /// <exception cref="DirectoryNotFoundException"></exception>
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

    /// <summary>
    /// 转为Bitmap
    /// </summary>
    /// <param name="ddsFilePath">DDS文件路径</param>
    /// <param name="savePath">保存位置</param>
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

    /// <summary>
    /// 更改文件的拓展名
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="extension">目标扩展名</param>
    /// <returns></returns>
    public static string ChangeExtension(string filePath, string extension) => @$"{Path.GetDirectoryName(filePath)}\{Path.GetFileNameWithoutExtension(filePath)}.{extension}";

    /// <summary>
    /// 如果目标目录不存在则自动创建目录
    /// </summary>
    /// <param name="filePath">目标目录</param>
    /// <returns>是否创建了新的目录</returns>
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
