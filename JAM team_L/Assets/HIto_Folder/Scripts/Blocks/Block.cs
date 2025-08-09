using UnityEngine;
using Global;
using MyLib.Object;

/// <summary>
/// �u���b�N�N���X.
/// </summary>
public class Block : MyObject
{
    [Header("- sprite -")]
    [SerializeField] Sprite[] imgBlock; //�摜.

    [Header("- setting -")]
    [SerializeField] BlockType type; //�u���b�N�̎��.

    bool isMoveAble;  //�����u���b�N���ǂ���.
    bool isBreakAble; //���邩�ǂ���.

    //get(��������ϐ��̒l�����ł���)
    public bool IsMoveAble  { get => isMoveAble; }
    public bool IsBreakAble { get => isBreakAble; }

    void Start()
    {
        InitMyObj(); //MyObj�̏�����.
        SetImage();  //�摜�ݒ�.

        //��ޕʂ̐ݒ�.
        switch (type)
        {
            case BlockType.Break: //�󂹂�.
                isMoveAble = false;
                isBreakAble = true;
                break;

            case BlockType.Carry: //�^�ׂ�.
                isMoveAble = true;
                isBreakAble = false;
                break;

            case BlockType.Terrain: //�n�`.
                isMoveAble = false;
                isBreakAble = false;
                break;

            default: Debug.LogError("[Error] �s���Ȓl�ł��B"); break;
        }
    }

    void Update()
    {

    }

    /// <summary>
    /// �摜�K�p.
    /// </summary>
    private void SetImage()
    {
        //��ޕʂ̐ݒ�.
        switch (type)
        {
            case BlockType.Break: //�󂹂�.
                MyObjImage = imgBlock[0];
                break;

            case BlockType.Carry: //�^�ׂ�.
                MyObjImage = imgBlock[1];
                break;

            case BlockType.Terrain: //�n�`.
                MyObjImage = imgBlock[2];
                break;

            default: Debug.LogError("[Error] �s���Ȓl�ł��B"); break;
        }
    }

    /// <summary>
    /// �u���b�N��j�󂷂�.
    /// </summary>
    public void BreakBlock()
    {
        Destroy(gameObject); //��.
    }
}