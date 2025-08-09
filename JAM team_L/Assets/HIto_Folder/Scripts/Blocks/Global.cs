
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
    /// Global定数.
    /// </summary>
    public static class GL_Const
    {
#if false
        //例:
        public const int COUNT = 3;
        
        public static string[] TEXT =
        {
            "AAA",
            "BBB",
            "CCC",
        };
#endif
    }

    /// <summary>
    /// Global関数.
    /// </summary>
    public static class GL_Func
    {
#if false
        public static void Test()
        {

        }
#endif
    }
}