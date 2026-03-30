using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void SceneToReset()
    {
        //Œ»İ‚ÌƒV[ƒ“‚ğ‚â‚è’¼‚·.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SceneToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
