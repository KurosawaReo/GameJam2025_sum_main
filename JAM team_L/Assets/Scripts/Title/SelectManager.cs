using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{
    [SerializeField] GameObject pnlWhite; //white panel

    string sceneName; //�ǂ��̃V�[���Ɉړ����邩.

    float  time = 0;           //�^�C�}�[.
    bool   pushButton = false; //�{�^���������ꂽ��.

    void Update()
    {
        if (pushButton)
        {
            time += Time.deltaTime;
            if(time >= 0.5f)
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    /// <summary>
    /// easy����������.
    /// </summary>
    public void PushEasy()
    {
        sceneName = "StageEasy";
        LoadSceneEvent();
    }
    /// <summary>
    /// normal����������.
    /// </summary>
    public void PushNormal()
    {
        sceneName = "StageNormal";
        LoadSceneEvent();
    }
    /// <summary>
    /// hard����������.
    /// </summary>
    public void PushHard()
    {
        sceneName = "StageHard";
        LoadSceneEvent();
    }

    private void LoadSceneEvent()
    {
        pushButton = true;

        pnlWhite.SetActive(true); //�L����.
        pnlWhite.GetComponent<Animator>().SetTrigger("pushButton");
    }
}
