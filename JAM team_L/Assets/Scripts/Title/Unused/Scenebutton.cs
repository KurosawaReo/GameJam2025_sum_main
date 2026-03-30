using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scenebutton : MonoBehaviour
{
    public string SceneName;

    public FadeScene FadeScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            FadeScene.LoadScene(SceneName);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
