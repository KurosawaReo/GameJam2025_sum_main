/*
   - KR.Animation - (Unity)
   ver.2026/03/20
*/
using System;
using UnityEngine;

/// <summary>
/// アニメーション用の追加機能.
/// </summary>
namespace KR.Unity.Animation
{
    /// <summary>
    /// アニメーション機能が備わったクラス.
    /// [継承想定]
    /// </summary>
    public class AnimationKR : MonoBehaviour
    {
        float              timer;    //タイマー(0.0～1.0)
        float              maxSec;   //アニメーションが完了するまでの時間(秒)
        Func<float, float> easeFunc; //タイマー(0.0～1.0)の進み方を変化させる関数(イージング)

        //get.
        public float GetTimer() => easeFunc(timer);

        /// <summary>
        /// アニメーションが終了したか.
        /// </summary>
        public bool IsFinished() => timer >= 1.0f;

        /// <summary>
        /// 初期化処理.
        /// </summary>
        /// <param name="_maxSec">アニメーション完了時間(秒)</param>
        /// <param name="_ease">イージング関数</param>
        public void InitAnim(float _maxSec, Func<float, float> _easeFunc)
        {
            maxSec   = _maxSec;
            easeFunc = _easeFunc;
        }

        /// <summary>
        /// リセット処理.
        /// </summary>
        public void ResetAnim()
        {
            timer = 0.0f;
        }

        /// <summary>
        /// 更新処理.
        /// </summary>
        public void UpdateAnim()
        {
            //maxSec秒かけてタイマー進行.
            timer += Time.deltaTime / maxSec;
            //0.0～1.0に制限.
            timer = Mathf.Clamp01(timer);
        }

        /// <summary>
        /// 消去する.
        /// </summary>
        public void Delete()
        {
            Destroy(gameObject);
        }
    }
}