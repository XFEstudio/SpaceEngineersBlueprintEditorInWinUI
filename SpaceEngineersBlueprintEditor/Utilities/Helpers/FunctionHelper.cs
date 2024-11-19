namespace SpaceEngineersBlueprintEditor.Utilities.Helpers;

public static class Helper
{
    public static async Task Wait(Func<bool> func, int delay = 100)
    {
        while (!func())
            await Task.Delay(delay);
    }
}
