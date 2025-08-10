using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalGate : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] RanKingManager_Hokuto        scptRanking;
    [SerializeField] PlayerOperationSwitch_Hokuto scptPlyOpe;
    [SerializeField] CountUpTimer                 scptTimer;

    /// <summary>
    /// ゲームクリア.
    /// </summary>
    private void GameClear()
    {
        scptRanking.Goal();       //ランキング処理.
        scptPlyOpe.PlayerNoOpe(); //プレイヤー動作停止.
        scptTimer.IsMove = false; //タイマー動作を停止.
    }

    //何かに触れた時.
    private void OnTriggerEnter2D(Collider2D hit)
    {
        //プレイヤーなら.
        if (hit.gameObject.CompareTag("player"))
        {
            GameClear();
        }
    }
}
