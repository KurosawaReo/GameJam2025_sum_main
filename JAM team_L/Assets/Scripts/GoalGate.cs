using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalGate : MonoBehaviour
{
    [Header("- script -")]
    [SerializeField] RanKingManager_Hokuto        scptRanking;
    [SerializeField] PlayerOperationSwitch_Hokuto scptPlyOpe;
    [SerializeField] CountUpTimer                 scptTimer;

    /// <summary>
    /// �Q�[���N���A.
    /// </summary>
    private void GameClear()
    {
        scptRanking.Goal();       //�����L���O����.
        scptPlyOpe.PlayerNoOpe(); //�v���C���[�����~.
        scptTimer.IsMove = false; //�^�C�}�[������~.
    }

    //�����ɐG�ꂽ��.
    private void OnTriggerEnter2D(Collider2D hit)
    {
        //�v���C���[�Ȃ�.
        if (hit.gameObject.CompareTag("player"))
        {
            GameClear();
        }
    }
}
