using UnityEngine;
using Global;
using MyLib.Object;

/// <summary>
/// ブロッククラス.
/// </summary>
public class Block : MyObject
{
    [Header("- prefab -")]
    [SerializeField] MyPrefab prfbBreak; //破壊アニメーション.

    [Header("- sprite -")]
    [SerializeField] Sprite[] imgBlock; //画像.

    [Header("- setting -")]
    [SerializeField] BlockType type; //ブロックの種類.

    bool isMoveAble;  //動くブロックかどうか.
    bool isBreakAble; //壊れるかどうか.

    //get(他所から変数の値を入手できる)
    public bool IsMoveAble  { get => isMoveAble; }
    public bool IsBreakAble { get => isBreakAble; }

    void Start()
    {
        InitMyObj(); //MyObjの初期化.
        SetImage();  //画像設定.

        //種類別の設定.
        switch (type)
        {
            case BlockType.Break: //壊せる.
                isMoveAble = false;
                isBreakAble = true;
                break;

            case BlockType.Carry: //運べる.
                isMoveAble = true;
                isBreakAble = false;
                break;

            case BlockType.Terrain: //地形.
                isMoveAble = false;
                isBreakAble = false;
                break;

            default: Debug.LogError("[Error] 不正な値です。"); break;
        }
    }

    /// <summary>
    /// 画像適用.
    /// </summary>
    private void SetImage()
    {
        //種類別の設定.
        switch (type)
        {
            case BlockType.Break: //壊せる.
                MyObjImage = imgBlock[0];
                break;

            case BlockType.Carry: //運べる.
                MyObjImage = imgBlock[1];
                break;

            case BlockType.Terrain: //地形.
                MyObjImage = imgBlock[2];
                break;

            default: Debug.LogError("[Error] 不正な値です。"); break;
        }
    }

    /// <summary>
    /// ブロックを破壊する.
    /// </summary>
    public void BreakBlock()
    {
        //親オブジェクトを探す.
        GameObject parent = GameObject.Find("EffectObjects");
        //prefab生成.
        var obj = Instantiate(prfbBreak.obj, parent.transform);
        obj.transform.position = transform.position;

        Destroy(gameObject); //ブロックは消滅.
    }
}