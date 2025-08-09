/*
   - MyLib.Animation -
   ver.2025/08/10
*/
using UnityEngine;

/// <summary>
/// アニメーション用の追加機能.
/// </summary>
namespace MyLib.Animation
{
    /// <summary>
    /// アニメーション自動消滅.
    /// MyLib.Animationを直接アタッチすればこのクラスがつく.
    /// </summary>
    public class AutoEraseAnim : MonoBehaviour
    {
        Animator animr; //保存する用.

        void Awake ()
        {
            //コンポーネント取得.
            animr = GetComponent<Animator>();
        }
        void Update()
        {
            //アニメーションの状態を取得.
            AnimatorStateInfo info = animr.GetCurrentAnimatorStateInfo(0);
            //アニメーション終了したら.
            if(info.normalizedTime > 1.0f)
            {
                Destroy(gameObject); //消去.
            }
        }
    }
}