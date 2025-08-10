using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class PlayerHead_Hokuto : MonoBehaviour
{
    [SerializeField] public bool isOperation = false;
    [SerializeField] float rotateSpeed = 1.0f;

    [SerializeField] GameObject playerBody;

    [SerializeField] float headPosition = 1.0f;

    [SerializeField] float lineLength    = 2.0f; // 線の長さ
    [SerializeField] float lineThickness = 0.5f; // 線の太さ
    [SerializeField] public LineRenderer lineRenderer;
    [SerializeField] public LineRenderer neckLineRenderer;
    Vector3 headMoveStartPosition;
    Vector3 headMoveEnd;
    Vector3 headMoveEndPosition;

    float time = 0;
    int moveCount = 0;

    [SerializeField] float moveTime = 1.0f;

    bool isTouchObject = false;

    bool isBreak = false;

    // 伸び or 戻りの移動中なら true
    public bool isMovingHead = false;

    PlayerHeadAction_Hokuto controls;
    Vector2 moveInput;

    Collider2D blockSave;

    bool isPushPull = false;
    [SerializeField] public float pullSpeed = 1.0f;
    void Start()
    {
        Init();
    }

    void Update()
    {
        if (isOperation)
        {
            HeadOperation();
        }
        else
        {
            transform.position = new Vector3(playerBody.transform.position.x, playerBody.transform.position.y + headPosition, 0);
        }
        GetComponent<Animator>().SetBool("CloseStop",isTouchObject);
        GetComponent<Animator>().SetBool("OpenStop", isOperation);
    }

    void Init()
    {
        //lineRenderer = GetComponent<LineRenderer>();
        //neckLineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogWarning("LineRenderer が見つかりません。自動追加します。");
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        if (lineRenderer == null)
        {
            Debug.LogWarning("LineRenderer が見つかりません。自動追加します。");
            neckLineRenderer = gameObject.AddComponent<LineRenderer>();
        }


        neckLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        neckLineRenderer.positionCount = 2;
        neckLineRenderer.startWidth = lineThickness;
        neckLineRenderer.endWidth = lineThickness;
        neckLineRenderer.startColor = new Color(1.0f, 0.75f, 0.72f);
        neckLineRenderer.endColor = new Color(1.0f, 0.75f, 0.72f);

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    void PlayerHeadRotate()
    {
        float rotate = rotateSpeed;
        if (0.1 <= Math.Abs(moveInput.x))
        {
            if (moveInput.x < -0.1f)
            {
                rotate *= -1.0f;
            }
            transform.Rotate(0, 0, rotate);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "block" && isOperation)
        {
            //Debug.Log("test");
            isTouchObject = true;
            blockSave = collision;
        }
    }

    private void Awake()
    {
        controls = new PlayerHeadAction_Hokuto();
    }

    private void OnEnable()
    {
        controls.PlayerHead.Rotate.Enable();
        controls.PlayerHead.Shot_Break.Enable();
        controls.PlayerHead.Release.Enable();
        controls.PlayerHead.Pull.Enable();

        controls.PlayerHead.Rotate.performed += OnRotatePerformed;
        controls.PlayerHead.Rotate.canceled += OnRotateCanceled;

        controls.PlayerHead.Shot_Break.performed += OnPlayerShot;

        controls.PlayerHead.Release.performed += OnPlayerRelease;

        controls.PlayerHead.Pull.started += OnPullStarted;
        controls.PlayerHead.Pull.canceled += OnPullCanceled;
    }

    private void OnDisable()
    {
        controls.PlayerHead.Rotate.performed -= OnRotatePerformed;
        controls.PlayerHead.Rotate.canceled -= OnRotateCanceled;

        controls.PlayerHead.Shot_Break.performed -= OnPlayerShot;

        controls.PlayerHead.Release.performed -= OnPlayerRelease;

        controls.PlayerHead.Pull.started -= OnPullStarted;
        controls.PlayerHead.Pull.canceled -= OnPullCanceled;

        controls.PlayerHead.Rotate.Disable();
        controls.PlayerHead.Shot_Break.Disable();
        controls.PlayerHead.Release.Disable();
        controls.PlayerHead.Pull.Disable();
    }

    void OnRotatePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void OnRotateCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    void OnPlayerRelease(InputAction.CallbackContext ctx)
    {
        if (isTouchObject && isMovingHead)
        {
            isTouchObject = false;

            // 戻り開始
            Vector3 temp = headMoveStartPosition;
            headMoveStartPosition = transform.position;
            headMoveEndPosition = temp;

            time = 0;
            moveCount = 1;
        }
    }

    void OnPlayerShot(InputAction.CallbackContext ctx)
    {
        if (!isMovingHead)
        {
            lineRenderer.enabled = false;
            isMovingHead = true;
            headMoveStartPosition = transform.position;

            Vector3 direction = transform.right;
            headMoveEndPosition = headMoveStartPosition + direction * lineLength;

            headMoveEnd = headMoveEndPosition - headMoveStartPosition;
            time = 0;
            moveCount = 0; // 0 = 行き, 1 = 戻り
        }
        else
        {
            isBreak = true;
        }
    }

    private void OnPullStarted(InputAction.CallbackContext ctx)
    {
        Debug.Log("Pull started: isPushPull = " + isPushPull);
        isPushPull = true;
    }

    private void OnPullCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("Pull canceled: isPushPull = " + isPushPull);
        isPushPull = false;
    }

    void UpdateLine()
    {
        if (isOperation)
        {
            lineRenderer.enabled = true;
        }
        Vector3 start = transform.position;
        Vector3 direction = transform.right;
        Vector3 end = start + direction * lineLength;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);


    }

    void HeadOperation()
    {
        if (isMovingHead)
        {
            Vector3 start = playerBody.transform.position + new Vector3(0,headPosition - 0.2f,0);
            Vector3 direction = transform.right;
            Vector3 end = transform.position;

            neckLineRenderer.SetPosition(0, start);
            neckLineRenderer.SetPosition(1, end);
            neckLineRenderer.enabled = true;
            if (!isTouchObject)
            {
                if(blockSave != null)
                {
                    Rigidbody2D rb = blockSave.GetComponent<Rigidbody2D>();
                    Rigidbody2D rb_body = playerBody.GetComponent<Rigidbody2D>();
                    if(blockSave.GetComponent<Block>().IsMoveAble == true)
                    {
                        rb.gravityScale = 1.0f;
                    }
                    else
                    {
                        rb_body.gravityScale = 1.0f;
                    }
                }
                time += Time.deltaTime;
                transform.position = Vector3.Lerp(headMoveStartPosition, headMoveEndPosition, time / moveTime);
            }
            else
            {
                Rigidbody2D rb = blockSave.GetComponent<Rigidbody2D>();
                Rigidbody2D rb_body = playerBody.GetComponent<Rigidbody2D>();
                if (blockSave.GetComponent<Block>().IsMoveAble == true)
                {
                    rb.gravityScale = 0f;
                }
                else
                {
                    rb_body.gravityScale = 0f;
                }
                if (isPushPull)  // ここを追加
                {
                    if (blockSave != null)
                    {
                        Block blockScript = blockSave.GetComponent<Block>();
                        if (blockScript != null && blockScript.IsMoveAble)
                        {
                            Vector3 playerBodyPosition = new Vector3(playerBody.transform.position.x, playerBody.transform.position.y + headPosition, playerBody.transform.position.z);
                            Vector3 directionToPlayer = (playerBodyPosition - transform.position).normalized;
                            Vector3 moveVector = directionToPlayer * pullSpeed * Time.deltaTime;

                            blockSave.transform.position += moveVector;
                            transform.position += moveVector;
                        }
                        else if (blockScript != null && !blockScript.IsMoveAble)
                        {
                            rb_body.gravityScale = 0;
                            Vector3 playerHeadPosition = new Vector3(transform.position.x, transform.position.y - headPosition, transform.position.z);
                            Vector3 directionToPlayer = (playerHeadPosition - playerBody.transform.position).normalized;
                            Vector3 moveVector = directionToPlayer * pullSpeed * Time.deltaTime;

                            playerBody.transform.position += moveVector;
                        }
                    }

                }
                if (isBreak)
                {
                    if(blockSave.GetComponent<Block>().IsBreakAble)
                    {
                        rb_body.gravityScale = 1.0f;
                        Vector3 temp = headMoveStartPosition;
                        headMoveStartPosition = transform.position;
                        headMoveEndPosition = temp;
                        time = 0;
                        moveCount = 1;
                        blockSave.GetComponent<Block>().BreakBlock();
                        isTouchObject = false;
                    }
                    isBreak = false;
                }

            }
            if (time >= moveTime)
            {
                if (!isTouchObject)
                {
                    if (moveCount == 0)
                    {
                        
                        // 行き終わり → 戻り開始
                        Vector3 temp = headMoveStartPosition;
                        headMoveStartPosition = headMoveEndPosition;
                        headMoveEndPosition = temp;
                        time = 0;
                        moveCount = 1;
                    }
                    else
                    {
                        // 戻り完了
                        isMovingHead = false;
                        moveCount = 0;
                    }
                }
            }
            if (isMovingHead && moveCount == 1)
            {
                headMoveEndPosition = playerBody.transform.position + new Vector3(0, headPosition, 0);
            }
        }
        else
        {
            if (!isTouchObject)
            {
                Vector3 savePosition = playerBody.transform.position + new Vector3(0, headPosition, 0);
                if (savePosition != transform.position)
                {
                    
                    transform.position = savePosition;
                }
                PlayerHeadRotate();
                UpdateLine();
                neckLineRenderer.enabled = false; ;
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }
    }
}
