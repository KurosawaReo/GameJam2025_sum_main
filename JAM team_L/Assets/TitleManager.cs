using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] string _SceneName;

    float time = 0;

    public bool pushButton = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pushButton = false;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (pushButton)
        {
            time += Time.deltaTime;
            if(time >= 1.0f)
            {
                SceneManager.LoadScene(_SceneName);
            }
        }

    }

    public void LoadSceneEvent()
    {
        pushButton = true;
        GetComponent<Animator>().SetBool("pushButton", pushButton);
    }
    public void LoadScene(string sceneNeme)
    {
        GetComponent<Animator>().enabled = true;
    }
}
