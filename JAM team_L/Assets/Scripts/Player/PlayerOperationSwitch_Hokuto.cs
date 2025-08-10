using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOperationSwitch_Hokuto : MonoBehaviour
{
    [SerializeField]PlayerBody_Hokuto body;
    [SerializeField]PlayerHead_Hokuto head;

    bool isBody = true;

    PlayerOperationSwitchAction_Hokuto action;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        action = new PlayerOperationSwitchAction_Hokuto();
    }

    private void OnEnable()
    {
        action.Player.Switch.Enable();

        action.Player.Switch.performed += OnPlayerRelease;
    }

    private void OnDisable()
    {
        action.Player.Switch.performed -= OnPlayerRelease;

        action.Player.Switch.Disable();
    }

    public void OnPlayerRelease(InputAction.CallbackContext ctx)
    {
        if (isBody && body.isGround)
        {
            head.isMovingHead = false;
            body.isOperation = false;
            head.isOperation = true;
            isBody = false;
        }
        else if(!isBody && !head.isMovingHead)
        {
            body.startJump = false;
            body.isJump = false;
            head.lineRenderer.enabled = false;
            body.isOperation = true;
            head.isOperation = false;
            isBody = true;
            body.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }
}
