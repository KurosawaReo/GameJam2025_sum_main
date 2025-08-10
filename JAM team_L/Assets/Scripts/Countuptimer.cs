using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Countuptimer : MonoBehaviour
{
    public float countup = 0.0f;

    [SerializeField]public Text timeText;

    [SerializeField] RanKingManager_Hokuto RanKingManager_Hokuto;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!RanKingManager_Hokuto.isPanel)
        {
            countup += Time.deltaTime;
        }

        timeText.text = "Time : " + countup.ToString("f1");
    }
}
