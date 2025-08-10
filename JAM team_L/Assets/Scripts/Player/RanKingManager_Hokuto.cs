using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RanKingManager_Hokuto : MonoBehaviour
{
    [SerializeField] CountUpTimer timer;

    public float nowTime;
    public float[] time = new float[5];

    [SerializeField] Text nowTimeText;
    [SerializeField] Text[] text = new Text[5];

    [SerializeField] public bool isPanel = false;
    [SerializeField] GameObject panel;

    private string sceneName;

    void Start()
    {
        panel.SetActive(false);
    }

    private void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;

        // 保存されているランキングを読み込み（シーン名付きキー）
        for (int i = 0; i < 5; i++)
        {
            time[i] = PlayerPrefs.GetFloat($"{sceneName}_Ranking_{i}", 9999);
        }
    }

    void Update()
    {
        if (isPanel)
        {
            panel.SetActive(true);
        }

        nowTimeText.text = "YouTime:" + nowTime.ToString("f1");
        for (int i = 0; i < 5; i++)
        {
            text[i].text = $"{i + 1}位: {time[i]:f1}";
        }
    }

    public void SaveRanking()
    {
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetFloat($"{sceneName}_Ranking_{i}", time[i]);
        }
        PlayerPrefs.Save();
    }

    public void Goal()
    {
        nowTime = timer.countup;
        isPanel = true;

        for (int i = 0; i < 5; i++)
        {
            if (time[i] > nowTime)
            {
                for (int j = 4; j > i; j--)
                {
                    time[j] = time[j - 1];
                }
                time[i] = nowTime;
                break;
            }
        }

        SaveRanking();
    }
    public void GoBackTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void GoBackStageSelect()
    {
        SceneManager.LoadScene("SerectScene");
    }
}