/*
   - MyLib.Inspector -
   ver.2025/08/13

   �Z�b�g�Ŏg�p: MyLib.InspectorEditor

   ���̃t�@�C���͂ǂ��̃t�H���_�ɓ���Ă�ok.

   ���Q�l�T�C�g
   https://mu-777.hatenablog.com/entry/2022/09/04/113850
*/
using System;
using UnityEngine;

/// <summary>
/// �C���X�y�N�^�[�p�̒ǉ��@�\.
/// </summary>
namespace MyLib.Inspector
{
    /// <summary>
    /// [InspectorDisable()]�Ŏ��s�����class.
    /// ���̃N���X�ɒl��ۑ����Ă���.
    /// </summary>
    public partial class InspectorDisableAttribute : PropertyAttribute
    {
    //�������o.
        public readonly string varName;      //�ϐ���.
        public readonly Type   varType;      //�ϐ��^.
        public readonly bool   isDisable;    //�������ǂ���.
        public readonly bool   isInvisible;  //OFF�̎��ɔ�\���ɂ��邩.

        public readonly string compStr;      //string�^�̔�r�l.
        public readonly int    compInt;      //int   �^�̔�r�l.
        public readonly float  compFloat;    //float �^�̔�r�l.

    //���R���X�g���N�^.
        /// <summary>
        /// ���C���̃R���X�g���N�^(�S�Ă̌^�Ή�)
        /// </summary>
        private InspectorDisableAttribute(
            string _varName, Type _varType, bool _isDisable = false, bool _isInvisible = false
        ){
            varName     = _varName;
            varType     = _varType;
            isDisable   = _isDisable;
            isInvisible = _isInvisible;
        }
        /// <summary>
        /// bool�^.
        /// </summary>
        public InspectorDisableAttribute(
            string _varName, bool _isNot = false, bool _isInvisible = false
        )
        : this(_varName, typeof(bool), _isNot, _isInvisible) //���g�̃R���X�g���N�^��.
        {}
        /// <summary>
        /// string�^.
        /// </summary>
        public InspectorDisableAttribute(
            string _varName, string _str, bool _isNot = false, bool _isInvisible = false
        )
        : this(_varName, _str.GetType(), _isNot, _isInvisible) //���g�̃R���X�g���N�^��.
        {
            compStr = _str;
        }
        /// <summary>
        /// int�^.
        /// </summary>
        public InspectorDisableAttribute(
            string _varName, int _int, bool _isNot = false, bool _isInvisible = false
        )
        : this(_varName, _int.GetType(), _isNot, _isInvisible) //���g�̃R���X�g���N�^��.
        {
            compInt = _int;
        }
        /// <summary>
        /// float�^.
        /// </summary>
        public InspectorDisableAttribute(
            string _varName, float _float, bool _isGreater = true, bool _isInvisible = false
        )
        : this(_varName, _float.GetType(), _isGreater, _isInvisible) //���g�̃R���X�g���N�^��.
        {
            compFloat = _float;
        }
    }
}