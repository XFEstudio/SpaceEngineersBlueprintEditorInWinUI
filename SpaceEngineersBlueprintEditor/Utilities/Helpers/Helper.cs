namespace SpaceEngineersBlueprintEditor.Utilities.Helpers;

/// <summary>
/// 帮助类
/// </summary>
public static class Helper
{
    /// <summary>
    /// 执行异步等待
    /// </summary>
    /// <param name="func">等待表达式</param>
    /// <param name="delay">检测延迟</param>
    /// <returns></returns>
    public static async Task Wait(Func<bool> func, int delay = 100)
    {
        while (!func())
            await Task.Delay(delay);
    }
}
