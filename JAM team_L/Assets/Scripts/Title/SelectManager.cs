using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{
    [SerializeField] GameObject pnlWhite; //white panel

    string sceneName; //どこのシーンに移動するか.

    float  time = 0;           //タイマー.
    bool   pushButton = false; //ボタンが押されたか.

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
    /// easyを押した時.
    /// </summary>
    public void PushEasy()
    {
        sceneName = "StageEasy";
        LoadSceneEvent();
    }
    /// <summary>
    /// normalを押した時.
    /// </summary>
    public void PushNormal()
    {
        sceneName = "StageNormal";
        LoadSceneEvent();
    }
    /// <summary>
    /// hardを押した時.
    /// </summary>
    public void PushHard()
    {
        sceneName = "StageHard";
        LoadSceneEvent();
    }

    private void LoadSceneEvent()
    {
        pushButton = true;

        pnlWhite.SetActive(true); //有効に.
        pnlWhite.GetComponent<Animator>().SetTrigger("pushButton");
    }
}
