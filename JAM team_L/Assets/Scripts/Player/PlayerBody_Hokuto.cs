using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class PlayerBody_Hokuto : MonoBehaviour

{
    [SerializeField] public bool isOperation = true;

    [SerializeField] public bool isGround;
    [Tooltip("�v���C���[�̃X�s�[�h�{��"), SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] LayerMask groundlayer;     //�ڒn���肷�郌�C���[
    public bool startJump;                             //�W�����v���邩�ǂ���

    public bool isJump;
    Vector3 spownPosition;
    [SerializeField] bool isFront = true;
//  [SerializeField] float soundTimePlus = 5f;
//  float soundTime = 0;

    PlayerAction_Hokuto controls;
    Vector2 moveInput;

    Vector3 switchPosition;

    [SerializeField] float deathZone = -7.0f;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(isOperation)
        {
            PlayerKeyDown();
            IsGround();
            AnimationSet();
            GameOver();
            //PlaySoundEffects();
            switchPosition = transform.position;
        }
        else
        {
            //transform.position = switchPosition;
        }
    }

    private void Awake()
    {
        controls = new PlayerAction_Hokuto();
    }

    void OnMovePerformed(InputAction.CallbackContext context)
    {
        // ���͎��̓���
        moveInput = context.ReadValue<Vector2>();
        if (!isFront)
        {
            moveInput.x *= -1;
        }
    }

    void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // ���������̓���
        moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        // �uPlayer�v�A�N�V�����́uMove�v�A�N�V�����}�b�v��L��
        controls.Player.Move.Enable();
        controls.Player.Jump.Enable();
        controls.Player.Reset.Enable();
        controls.Player.GoBackTitle.Enable();

        // ���͎��ɌĂ΂��C�x���g��o�^
        controls.Player.Move.performed += OnMovePerformed; // ���͎�
        controls.Player.Move.canceled += OnMoveCanceled;   // ��������

        controls.Player.Jump.performed += OnPlayerJump;

        controls.Player.Reset.performed += OnSceneReset;

        controls.Player.GoBackTitle.performed += OnSceneTitle;
    }
    private void OnDisable()
    {
        // ���͎��ɌĂ΂��C�x���g���폜
        controls.Player.Move.performed -= OnMovePerformed; // ���͎�
        controls.Player.Move.canceled -= OnMoveCanceled;   // ��������

        controls.Player.Jump.performed -= OnPlayerJump;

        controls.Player.Reset.performed -= OnSceneReset;

        controls.Player.GoBackTitle.performed -= OnSceneTitle;

        // �uPlayer�v�A�N�V�����́uMove�v�A�N�V�����}�b�v�𖳌�
        controls.Player.Move.Disable();
        controls.Player.Jump.Disable();
        controls.Player.Reset.Disable();
        controls.Player.GoBackTitle.Disable();
    }

    /// <summary>
    /// ��莞�Ԃ��ƂɌĂяo�����֐�
    /// </summary>
    void FixedUpdate()
    {
        if (isOperation)
        {
            PlayerJump();
            PlayerMove();
        }
    }

    void Init()
    {
        //�����l��0���Ɠ����Ȃ��ăo�O�Ɗ��Ⴂ���邽�ߏ����l��0�Ȃ�1�ɕύX
        if (moveSpeed == 0)
        {
            moveSpeed = 1;
        }
        isGround = true;
        startJump = false;
        spownPosition = transform.position;
    }

    /// <summary>
    /// Key�������̏����ނ̂܂Ƃ�
    /// </summary>
    void PlayerKeyDown()
    {
        PushMoveKey();
        PushJumpKey();
    }

    /// <summary>
    /// ���E�ǂ���̃L�[��������Ă��邩�̎擾
    /// </summary>
    void PushMoveKey()
    {
        //���E�ړ�
        //move = Input.GetAxis("Horizontal"); //���������̓��̓`�F�b�N

        if (moveInput.x > 0.1f)
        {
            GetComponent<SpriteRenderer>().flipX = false;   //�摜�̌����̓f�t�H���g
        }

        else if (moveInput.x < -0.1f)
        {
            GetComponent<SpriteRenderer>().flipX = true;    //�摜�̌����͋t����
        }
    }

/*    void PlaySoundEffects()
    {
        if (isGround && Mathf.Abs(moveInput.x) > 0 && soundTime > 1.0f)
        {
            GetComponent<AudioSource>().Play();
            soundTime = 0;
        }
        soundTime += Time.deltaTime * soundTimePlus;
    }*/

    /// <summary>
    /// �W�����v���邩�̔��菈��
    /// </summary>
    void PushJumpKey()
    {
        if (isJump && isGround == true)
        {
            startJump = true;
            isJump = false;
        }
    }

    /// <summary>
    /// �n�ʂɂ��邩�̔���
    /// </summary>
    void IsGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
                            transform.position,     //�@ ���ˌ��i���S�ʒu�j
                            new Vector2(0.8f, 2.7f),	//�A Box�̃T�C�Y�i���ƍ����j
                            0,                      //�B ��]�p�i����͉�]�Ȃ��j
                            new Vector2(0, -1.0f),	//�C ���˕����i�������j
                            0.02f,                   //�D �����i0.2���j�b�g���Ɍ������āj
                            groundlayer);			//�E �ΏۂƂ��郌�C���[�i�n�ʃ��C���[�j
        if (hit.collider != null)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }

    /// <summary>
    /// �ǂ̃A�j���[�V�����𓮂������ݒ肷�邽�߂̊֐�
    /// </summary>
    void AnimationSet()
    {
        //�A�j���[�V������Ԃ̕ύX
        GetComponent<Animator>().SetBool("isJump", !isGround);      //�󒆂��ǂ���(!isGround�͒n�ʂ��ǂ����������Ă�̂Œn�ʂłȂ��̂Ȃ�󒆂Ƃ݂Ȃ�)
        GetComponent<Animator>().SetFloat("move", Mathf.Abs(moveInput.x));
    }

    /// <summary>
    /// �v���C���[�����񂾂烊�Z�b�g����@�\
    /// </summary>
    void GameOver()
    {
        if (transform.position.y <= deathZone)
        {
            transform.position = spownPosition;
        }
    }

    /// <summary>
    /// �v���C���[����ԏ���
    /// </summary>
    void PlayerJump()
    {
        if (startJump)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector3(0, jumpPower, 0));
            startJump = false;
        }
    }

    /// <summary>
    /// �v���C���[������
    /// </summary>
    void PlayerMove()
    {
        transform.position += new Vector3(moveInput.x * moveSpeed * Time.fixedDeltaTime, 0, 0);    //Time.fixedDeltaTime�Ƃ����͎̂��Ԃ̍����擾���Ă��Ă���������邱�Ƃłǂ�Ȍv�Z���x�ł������悤�ȃX�s�[�h�ɂȂ�悤��
        //PlaySoundEffects();
    }

    void OnSceneReset(InputAction.CallbackContext ctx)
    {
        Debug.Log("test");
        SceneManager.LoadScene("SerectScene");
    }
    void OnSceneTitle(InputAction.CallbackContext ctx)
    {
        Debug.Log("test");
        SceneManager.LoadScene("TitleScene");
    }

    void OnPlayerJump(InputAction.CallbackContext ctx)
    {
        if(isGround && !isJump)
        {
            isJump = true;
        }
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