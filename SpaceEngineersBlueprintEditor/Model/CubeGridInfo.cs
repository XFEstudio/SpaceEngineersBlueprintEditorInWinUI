namespace SpaceEngineersBlueprintEditor.Model;

/// <summary>
/// 方块网格信息
/// </summary>
/// <param name="GridName">网格名称</param>
/// <param name="CubeInGridCount">网格中的方块数量</param>
public record class CubeGridInfo(string GridName, string CubeInGridCount) { }
