using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CountUpTimer : MonoBehaviour
{
    public float countup = 0.0f;

    public Text timeText;

    bool isMove = true; //�^�C�}�[�𓮂������ǂ���.
    //set.
    public bool IsMove { set => isMove = value; }

    void Update()
    {
        //���쒆�̂�.
        if (isMove)
        {
            countup += Time.deltaTime;

            timeText.text = "Timer: " + countup.ToString("f1") + "�b";
        }
    }
}
