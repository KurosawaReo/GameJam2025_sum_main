/*
   - MyLib.Animation -
   ver.2025/08/10
*/
using UnityEngine;

/// <summary>
/// �A�j���[�V�����p�̒ǉ��@�\.
/// </summary>
namespace MyLib.Animation
{
    /// <summary>
    /// �A�j���[�V������������.
    /// MyLib.Animation�𒼐ڃA�^�b�`����΂��̃N���X����.
    /// </summary>
    public class AutoEraseAnim : MonoBehaviour
    {
        Animator animr; //�ۑ�����p.

        void Awake ()
        {
            //�R���|�[�l���g�擾.
            animr = GetComponent<Animator>();
        }
        void Update()
        {
            //�A�j���[�V�����̏�Ԃ��擾.
            AnimatorStateInfo info = animr.GetCurrentAnimatorStateInfo(0);
            //�A�j���[�V�����I��������.
            if(info.normalizedTime > 1.0f)
            {
                Destroy(gameObject); //����.
            }
        }
    }
}