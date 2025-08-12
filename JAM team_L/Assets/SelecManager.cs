using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SelecManager : MonoBehaviour
{
    PlayerAction_Hokuto controls;
    Vector2 moveInput;

    [SerializeField] TitleManager easy;
    [SerializeField] TitleManager normal;
    [SerializeField] TitleManager hard;

    [SerializeField] GameObject easyB;
    [SerializeField] GameObject normalB;
    [SerializeField] GameObject hardB;

    int nowButton = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(nowButton == 0)
        {
            easyB.GetComponent<Image>().color = new Color32(0, 214, 255, 255);
            normalB.GetComponent<Image>().color = Color.white;
            hardB.GetComponent<Image>().color = Color.white;
        }
        else if (nowButton == 1)
        {
            normalB.GetComponent<Image>().color = new Color32(0, 214, 255, 255);
            easyB.GetComponent<Image>().color = Color.white;
            hardB.GetComponent<Image>().color = Color.white;
        }
        else
        {
            hardB.GetComponent<Image>().color = new Color32(0, 214, 255, 255);
            easyB.GetComponent<Image>().color = Color.white;
            normalB.GetComponent<Image>().color = Color.white;
        }
    }

    private void OnEnable()
    {
        // �uPlayer�v�A�N�V�����́uMove�v�A�N�V�����}�b�v��L��
        controls.Player.Move.Enable();
        controls.Player.Jump.Enable();

        // ���͎��ɌĂ΂��C�x���g��o�^
        controls.Player.Move.performed += OnMovePerformed; // ���͎�

        controls.Player.Jump.performed += OnPlayerJump;


    }
    private void OnDisable()
    {

        // ���͎��ɌĂ΂��C�x���g���폜
        controls.Player.Move.performed -= OnMovePerformed; // ���͎�

        controls.Player.Jump.performed -= OnPlayerJump;

        // �uPlayer�v�A�N�V�����́uMove�v�A�N�V�����}�b�v�𖳌�
        controls.Player.Move.Disable();
        controls.Player.Jump.Disable();

    }
    void OnMovePerformed(InputAction.CallbackContext context)
    {

        moveInput = context.ReadValue<Vector2>();
        if(moveInput.x > 0.1f && nowButton < 2)
        {
            nowButton += 1;
        }
        if(moveInput.x < -0.1f && nowButton > 0)
        {
            nowButton -= 1;
        }

    }
    void OnPlayerJump(InputAction.CallbackContext ctx)
    {
        if(nowButton == 0)
        {
            easy.LoadSceneEvent();
        }
        else if (nowButton == 1)
        {
            normal.LoadSceneEvent();
        }
        else
        {
            hard.LoadSceneEvent();
        }

    }
}
