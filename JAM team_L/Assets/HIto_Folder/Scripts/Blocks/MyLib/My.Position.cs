/*
   - MyLib.Position -
   ver.2025/06/27
*/
using UnityEngine;

/// <summary>
/// ���W�Ǘ�������p�̒ǉ��@�\.
/// </summary>
namespace MyLib.Position
{
    /// <summary>
    /// �㉺���E.
    /// </summary>
    public struct LBRT
    {
        public float left;
        public float bottom;
        public float right;
        public float top;

        //�R���X�g���N�^.
        public LBRT(float _l, float _b, float _r, float _t)
        {
            left   = _l;
            bottom = _b;
            right  = _r;
            top    = _t;
        }
    }

    /// <summary>
    /// Position�֐�.
    /// </summary>
    public static class PS_Func
    {
        /// <summary>
        /// ��ʂ̏㉺���E�̍��W���擾.
        /// �J������Orthographic���[�h�̎�����.
        /// </summary>
        public static LBRT GetWindowLBRT()
        {
            Camera cam = Camera.main;

            float height = cam.orthographicSize * 2f;
            float width = height * cam.aspect;

            Vector3 center = cam.transform.position;

            float left   = center.x - width  / 2f;
            float right  = center.x + width  / 2f;
            float bottom = center.y - height / 2f;
            float top    = center.y + height / 2f;

            return new LBRT(left, bottom, right, top);
        }

        /// <summary>
        /// �ړ��\�͈͓��ɕ␳����.
        /// </summary>
        /// <param name="pos">�I�u�W�F�N�g���W</param>
        /// <param name="size">�I�u�W�F�N�g�T�C�Y</param>
        /// <param name="lim">���E���W(�㉺���E)</param>
        /// <returns>�␳�ύ��W</returns>
        public static Vector3 FixPosInArea(Vector3 pos, Vector2 size, LBRT lim)
        {
            if (pos.x < lim.left   + size.x/2) { pos.x = lim.left   + size.x/2; }
            if (pos.y < lim.bottom + size.y/2) { pos.y = lim.bottom + size.y/2; }
            if (pos.x > lim.right  - size.x/2) { pos.x = lim.right  - size.x/2; }
            if (pos.y > lim.top    - size.y/2) { pos.y = lim.top    - size.y/2; }

            return pos;
        }

        /// <summary>
        /// ���[�J�����W�����[���h���W�ɕϊ�.
        /// </summary>
        /// <param name="obj">�e�I�u�W�F�N�g</param>
        /// <param name="lPos">���[�J�����W</param>
        /// <returns>���[���h���W</returns>
        public static Vector2 LPosToWPos(GameObject obj, Vector2 lPos)
        {
            var wPos = obj.transform.TransformPoint(lPos);
            return wPos;
        }

        /// <summary>
        /// ���[���h���W�����[�J�����W�ɕϊ�.
        /// </summary>
        /// <param name="obj">�e�I�u�W�F�N�g</param>
        /// <param name="wPos">���[���h���W</param>
        /// <returns>���[�J�����W</returns>
        public static Vector2 WPosToLPos(GameObject obj, Vector2 wPos)
        {
            var lPos = obj.transform.InverseTransformPoint(wPos);
            return lPos;
        }
    }
}