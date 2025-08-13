/*
   - MyLib.Inspector -
   ver.2025/08/13

   セットで使用: MyLib.InspectorEditor

   このファイルはどこのフォルダに入れてもok.

   ↓参考サイト
   https://mu-777.hatenablog.com/entry/2022/09/04/113850
*/
using System;
using UnityEngine;

/// <summary>
/// インスペクター用の追加機能.
/// </summary>
namespace MyLib.Inspector
{
    /// <summary>
    /// [InspectorDisable()]で実行されるclass.
    /// このクラスに値を保存しておく.
    /// </summary>
    public partial class InspectorDisableAttribute : PropertyAttribute
    {
    //▼メンバ.
        public readonly string varName;      //変数名.
        public readonly Type   varType;      //変数型.
        public readonly bool   isDisable;    //無効かどうか.
        public readonly bool   isInvisible;  //OFFの時に非表示にするか.

        public readonly string compStr;      //string型の比較値.
        public readonly int    compInt;      //int   型の比較値.
        public readonly float  compFloat;    //float 型の比較値.

    //▼コンストラクタ.
        /// <summary>
        /// メインのコンストラクタ(全ての型対応)
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
        /// bool型.
        /// </summary>
        public InspectorDisableAttribute(
            string _varName, bool _isNot = false, bool _isInvisible = false
        )
        : this(_varName, typeof(bool), _isNot, _isInvisible) //自身のコンストラクタへ.
        {}
        /// <summary>
        /// string型.
        /// </summary>
        public InspectorDisableAttribute(
            string _varName, string _str, bool _isNot = false, bool _isInvisible = false
        )
        : this(_varName, _str.GetType(), _isNot, _isInvisible) //自身のコンストラクタへ.
        {
            compStr = _str;
        }
        /// <summary>
        /// int型.
        /// </summary>
        public InspectorDisableAttribute(
            string _varName, int _int, bool _isNot = false, bool _isInvisible = false
        )
        : this(_varName, _int.GetType(), _isNot, _isInvisible) //自身のコンストラクタへ.
        {
            compInt = _int;
        }
        /// <summary>
        /// float型.
        /// </summary>
        public InspectorDisableAttribute(
            string _varName, float _float, bool _isGreater = true, bool _isInvisible = false
        )
        : this(_varName, _float.GetType(), _isGreater, _isInvisible) //自身のコンストラクタへ.
        {
            compFloat = _float;
        }
    }
}