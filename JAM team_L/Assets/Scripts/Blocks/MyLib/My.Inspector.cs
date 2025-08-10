/*
   - MyLib.Inspector -
   ver.2025/06/28

   ���Q�l�T�C�g.
   https://mu-777.hatenablog.com/entry/2022/09/04/113850

   �R�����g�͐����̌�����������.
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// �C���X�y�N�^�[�p�̒ǉ��@�\.
/// </summary>
namespace MyLib.Inspector
{
    //�s��.
    using GetCondFunc = Func<SerializedProperty, InspectorDisableAttribute, bool>;

    /// <summary>
    /// Attribute�̕��ŕۑ������l��ǂݎ��class.
    /// ������Inspector�\���̐ݒ������.
    /// </summary>
    [CustomPropertyDrawer(typeof(InspectorDisableAttribute))]
    internal sealed class ConditionalDisableDrawer : PropertyDrawer
    {
    //�������o.
        //???
        private Dictionary<Type, GetCondFunc> DisableCondFuncMap = new Dictionary<Type, GetCondFunc>() {
            { typeof(bool),   (prop, attr) => { return attr.isDisable ? !prop.boolValue : prop.boolValue;} },
            { typeof(string), (prop, attr) => { return attr.isDisable ? prop.stringValue == attr.compStr   : prop.stringValue != attr.compStr;  } },
            { typeof(int),    (prop, attr) => { return attr.isDisable ? prop.intValue    == attr.compInt   : prop.intValue    != attr.compInt;  } },
            { typeof(float),  (prop, attr) => { return attr.isDisable ? prop.floatValue  <= attr.compFloat : prop.floatValue  >  attr.compFloat;} }
        };

    //��public�֐�.

        /// <summary>
        /// ???
        /// </summary>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //???
            var attr = attribute as InspectorDisableAttribute;
            //???
            SerializedProperty prop;

            //�p�X��H��, ���ꂪ�Ȃ���class�̃����o�ɃA�N�Z�X�ł��Ȃ�.
            {
                string parentPath = property.propertyPath; //�p�X.
                int lastDot = parentPath.LastIndexOf('.'); //�Ō�̃h�b�g��T��.
            
                //�Ō�̃h�b�g�������.
                if (lastDot >= 0)
                {
                    string parent = parentPath.Substring(0, lastDot); //��:player
                    prop = property.serializedObject.FindProperty($"{parent}.{attr.varName}"); //�e���܂ރp�X.
                }
                else
                {
                    prop = property.serializedObject.FindProperty(attr.varName); //���̂܂܂̃p�X.
                }
                //�l�Ȃ��G���[.
                if (prop == null)
                {
                    Debug.LogError($"Not found '{attr.varName}' property");
                    EditorGUI.PropertyField(position, property, label, true); //???
                }
            }

            //???
            var isDisable = IsDisable(attr, prop);
            if (attr.isInvisible && isDisable)
            {
                return; //���̐�̏��������Ȃ�.
            }

            EditorGUI.BeginDisabledGroup(isDisable);                  //begin
            EditorGUI.PropertyField(position, property, label, true); //???
            EditorGUI.EndDisabledGroup();                             //end
        }

        /// <summary>
        /// Inspector�œ��͗���\�����鍂�����擾.
        /// </summary>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //???
            var attr = attribute as InspectorDisableAttribute;
            //???
            SerializedProperty prop;

            //�p�X��H��, ���ꂪ�Ȃ���class�̃����o�ɃA�N�Z�X�ł��Ȃ�.
            {
                string parentPath = property.propertyPath; //�p�X.
                int lastDot = parentPath.LastIndexOf('.'); //�Ō�̃h�b�g��T��.
                //�Ō�̃h�b�g�������.
                if (lastDot >= 0)
                {
                    string parent = parentPath.Substring(0, lastDot); //��:player
                    prop = property.serializedObject.FindProperty($"{parent}.{attr.varName}"); //�e���܂ރp�X.
                }
                else
                {
                    prop = property.serializedObject.FindProperty(attr.varName); //���̂܂܂̃p�X.
                }
            }
            //�K�؂ȍ�����Ԃ�.
            if (attr.isInvisible && IsDisable(attr, prop))
            {
                return -EditorGUIUtility.standardVerticalSpacing;   //��\�����̕\���ʒu.
            }
            else
            {
                return EditorGUI.GetPropertyHeight(property, true); //�ʏ�̕\���ʒu.
            }
        }

        /// <summary>
        /// �������ǂ����擾.
        /// </summary>
        private bool IsDisable(InspectorDisableAttribute attr, SerializedProperty prop)
        {
            //����������p�̕ϐ�.
            GetCondFunc condFunc;
            
            //???�̎擾�����݂�.
            bool ret = DisableCondFuncMap.TryGetValue(attr.varType, out condFunc);
            if (!ret) //���s�G���[.
            {
                Debug.LogError($"{attr.varType} type is not supported");
                return false;
            }
            //���ʂ�Ԃ�.
            return condFunc(prop, attr);
        }
    }

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