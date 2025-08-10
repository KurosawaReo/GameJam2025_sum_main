using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalGate : MonoBehaviour
{
    /// <summary>
    /// �Q�[���N���A.
    /// </summary>
    private void GameClear()
    {
        SceneManager.LoadScene("TitleScene"); //�^�C�g����.
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
