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
}
