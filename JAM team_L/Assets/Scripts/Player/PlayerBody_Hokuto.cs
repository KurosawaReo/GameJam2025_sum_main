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
    [Tooltip("プレイヤーのスピード倍率"), SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] LayerMask groundlayer;     //接地判定するレイヤー
    public bool startJump;                             //ジャンプするかどうか

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
        // 入力時の動作
        moveInput = context.ReadValue<Vector2>();
        if (!isFront)
        {
            moveInput.x *= -1;
        }
    }

    void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // 離した時の動作
        moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        // 「Player」アクションの「Move」アクションマップを有効
        controls.Player.Move.Enable();
        controls.Player.Jump.Enable();
        controls.Player.Reset.Enable();
        controls.Player.GoBackTitle.Enable();

        // 入力時に呼ばれるイベントを登録
        controls.Player.Move.performed += OnMovePerformed; // 入力時
        controls.Player.Move.canceled += OnMoveCanceled;   // 離した時

        controls.Player.Jump.performed += OnPlayerJump;

        controls.Player.Reset.performed += OnSceneReset;

        controls.Player.GoBackTitle.performed += OnSceneTitle;
    }
    private void OnDisable()
    {
        // 入力時に呼ばれるイベントを削除
        controls.Player.Move.performed -= OnMovePerformed; // 入力時
        controls.Player.Move.canceled -= OnMoveCanceled;   // 離した時

        controls.Player.Jump.performed -= OnPlayerJump;

        controls.Player.Reset.performed -= OnSceneReset;

        controls.Player.GoBackTitle.performed -= OnSceneTitle;

        // 「Player」アクションの「Move」アクションマップを無効
        controls.Player.Move.Disable();
        controls.Player.Jump.Disable();
        controls.Player.Reset.Disable();
        controls.Player.GoBackTitle.Disable();
    }

    /// <summary>
    /// 一定時間ごとに呼び出される関数
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
        //初期値が0だと動かなくてバグと勘違いするため初期値が0なら1に変更
        if (moveSpeed == 0)
        {
            moveSpeed = 1;
        }
        isGround = true;
        startJump = false;
        spownPosition = transform.position;
    }

    /// <summary>
    /// Key押下字の処理類のまとめ
    /// </summary>
    void PlayerKeyDown()
    {
        PushMoveKey();
        PushJumpKey();
    }

    /// <summary>
    /// 左右どちらのキーが押されているかの取得
    /// </summary>
    void PushMoveKey()
    {
        //左右移動
        //move = Input.GetAxis("Horizontal"); //水平方向の入力チェック

        if (moveInput.x > 0.1f)
        {
            GetComponent<SpriteRenderer>().flipX = false;   //画像の向きはデフォルト
        }

        else if (moveInput.x < -0.1f)
        {
            GetComponent<SpriteRenderer>().flipX = true;    //画像の向きは逆向き
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
    /// ジャンプするかの判定処理
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
    /// 地面にいるかの判定
    /// </summary>
    void IsGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
                            transform.position,     //① 発射元（中心位置）
                            new Vector2(0.8f, 2.7f),	//② Boxのサイズ（幅と高さ）
                            0,                      //③ 回転角（今回は回転なし）
                            new Vector2(0, -1.0f),	//④ 発射方向（下方向）
                            0.02f,                   //⑤ 距離（0.2ユニット下に向かって）
                            groundlayer);			//⑥ 対象とするレイヤー（地面レイヤー）
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
    /// どのアニメーションを動かすか設定するための関数
    /// </summary>
    void AnimationSet()
    {
        //アニメーション状態の変更
        GetComponent<Animator>().SetBool("isJump", !isGround);      //空中かどうか(!isGroundは地面かどうかが入ってるので地面でないのなら空中とみなす)
        GetComponent<Animator>().SetFloat("move", Mathf.Abs(moveInput.x));
    }

    /// <summary>
    /// プレイヤーが死んだらリセットする機能
    /// </summary>
    void GameOver()
    {
        if (transform.position.y <= deathZone)
        {
            transform.position = spownPosition;
        }
    }

    /// <summary>
    /// プレイヤーが飛ぶ処理
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
    /// プレイヤーが動く
    /// </summary>
    void PlayerMove()
    {
        transform.position += new Vector3(moveInput.x * moveSpeed * Time.fixedDeltaTime, 0, 0);    //Time.fixedDeltaTimeというのは時間の差を取得していてこれをかけることでどんな計算速度でも同じようなスピードになるように
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