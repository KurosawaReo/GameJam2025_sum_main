
/// <summary>
/// 汎用定数や汎用関数などをまとめる用.
/// </summary>
namespace Global
{
    /// <summary>
    /// ブロックの種類.
    /// </summary>
    public enum BlockType
    {
        Break,   //壊せる.
        Carry,   //運べる.
        Terrain, //地形.
    }
    /// <summary>
    /// ステージの種類.
    /// </summary>
    public enum Stage
    {
        Easy,
        Normal,
        Hard
    }
}