using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void PushStart()
    {
        // �J�ڂ��������V�[�������w��
        SceneManager.LoadScene("SelectScene");
    }
}
