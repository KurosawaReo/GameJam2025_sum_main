/*
   - MyLib.InspectorEditor -
   ver.2025/08/13

   �Z�b�g�Ŏg�p: MyLib.Inspector
   �t�H���_: Editor�ɓ����
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using MyLib.Inspector;

/// <summary>
/// �C���X�y�N�^�[�p�̒ǉ��@�\.
/// </summary>
namespace MyLib.InspectorEditor
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
}