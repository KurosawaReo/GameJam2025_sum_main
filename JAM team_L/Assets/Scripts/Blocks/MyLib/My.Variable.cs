/*
   - MyLib.Variable -
   ver.2025/06/28
*/
using UnityEngine;
using System;

/// <summary>
/// �ϐ��p�̒ǉ��@�\.
/// </summary>
namespace MyLib.Variable
{
    /// <summary>
    /// �͈͂���int�^�ϐ�.
    /// </summary>
    [Serializable] public struct IntR //int range.
    {
        [SerializeField] private int max;  //�ő�l.
        [SerializeField] private int min;  //�ŏ��l.
        [SerializeField] private int init; //�������l.
        private int now; //���ݒl.

        //set, get.
        public int Now //���ݒl.
        {
            get => now;
            set {
                now = value;
                if (now > max) { now = max; } //�ő�ɐݒ�.
                if (now < min) { now = min; } //�ŏ��ɐݒ�.
            }
        }

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public IntR(int _max, int _min, int _init)
        {
            max  = _max;
            min  = _min;
            init = _init;
            now  = _init; //�����l�ɐݒ�.
        }
        /// <summary>
        /// ���Z�b�g.
        /// </summary>
        public void Reset()
        {
            now = init;
        }
    }

    /// <summary>
    /// �͈͂���float�^�ϐ�.
    /// </summary>
    [Serializable] public struct FloatR //float range.
    {
        [SerializeField] private float max;  //�ő�l.
        [SerializeField] private float min;  //�ŏ��l.
        [SerializeField] private float init; //�������l.
        private float now; //���ݒl.

        //set, get.
        public float Now //���ݒl.
        {
            get => now;
            set {
                now = value;
                if (now > max) { now = max; } //�ő�ɐݒ�.
                if (now < min) { now = min; } //�ŏ��ɐݒ�.
            }
        }

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public FloatR(float _max, float _min, float _init)
        {
            max  = _max;
            min  = _min;
            init = _init;
            now  = _init; //�����l�ɐݒ�.
        }
        /// <summary>
        /// ���Z�b�g.
        /// </summary>
        public void Reset()
        {
            now = init;
        }
    }
}