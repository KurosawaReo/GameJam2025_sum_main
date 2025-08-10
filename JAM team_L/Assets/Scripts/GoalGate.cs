using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalGate : MonoBehaviour
{
    /// <summary>
    /// ゲームクリア.
    /// </summary>
    private void GameClear()
    {
        SceneManager.LoadScene("TitleScene"); //タイトルへ.
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
