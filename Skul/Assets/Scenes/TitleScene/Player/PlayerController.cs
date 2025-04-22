using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static InputManager;
using static PlayerStateMachine;
using static UnityEngine.LightAnchor;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private int jumpCount;
    private int dashCount;
    private int ActionLimit = 2;
    private BoxCollider2D groundCollider;
    public bool isColliderEnable;
    public Vector2 currentDirection;
    public Vector2 jumpDirection;
    private PlayerMovement playerMovement;
    private LayerMask groundLayer = 1 << 9;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        isColliderEnable = true;
        currentDirection = Vector2.right;
        jumpDirection = Vector2.up;
        jumpCount = 0;
        dashCount = 0;
    }

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        groundCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (rb.linearVelocity.y <= -0.1f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -5f);
            PlayerManager.instance.GetStateMachine().PlayFall();
        }
    }
    public void MoveUp()
    {
        jumpDirection = Vector2.up;
    }

    public void MoveDown()
    {
        jumpDirection = Vector2.down;
    }

    public void MoveLeft()
    {
        if (PlayerManager.instance.GetStateMachine().isDashing)
        {
            return;
        }
        currentDirection = Vector2.left;
        playerMovement.Move(currentDirection * moveSpeed);
        transform.localScale = new Vector3(-1, 1, 1);
        PlayerManager.instance.GetStateMachine().SetBoolState("Walk", true);
    }

    public void MoveRight()
    {
        if (PlayerManager.instance.GetStateMachine().isDashing)
        {
            return;
        }
        currentDirection = Vector2.right;
        playerMovement.Move(currentDirection * moveSpeed);
        transform.localScale = new Vector3(1, 1, 1);
        PlayerManager.instance.GetStateMachine().SetBoolState("Walk", true);
    }

    public void StopMove()
    {
        currentDirection = Vector2.zero;
        KeyUp("Walk");
    }
    public void KeyUp(string stateName)
    {
        PlayerManager.instance.GetStateMachine().SetBoolState(stateName, false);
    }

    public void Jump()
    {
        DeActiveCollider();
        if (jumpCount < ActionLimit)
        {
            jumpCount++;
            playerMovement.Jump(jumpDirection * jumpForce);
            PlayerManager.instance.GetStateMachine().PlayAnimation("Jump", PlayerStateMachine.PlayerGroundState.IsFalling);
        }
        ActiveCollider();
    }

    public void Attack()
    {
        PlayerManager.instance.GetStateMachine().PlayAttackAnimation();
    }

    public void Dash()
    {
        if (dashCount < ActionLimit)
        {
            dashCount++;
            // 4프레임이니까
            PlayerManager.instance.GetStateMachine().PlayAnimation("Dash");
            StartCoroutine(StartDash(currentDirection, 4));
        }
    }

    IEnumerator StartDash(Vector2 currentDirection, int repeatCount)
    {
        DeActiveCollider();
        PlayerManager.instance.GetStateMachine().SetDash(true);
        for (int i = 0; i < repeatCount; ++i)
        {
            playerMovement.Move(currentDirection * moveSpeed * 10);
            yield return null;
        }
        ActiveCollider();
        PlayerManager.instance.GetStateMachine().SetDash(false);
    }
    public void UseSkill1()
    {
        PlayerManager.instance.GetStateMachine().PlayAnimation("Skill1");
    }

    public void UseSkill2()
    {
        PlayerManager.instance.GetStateMachine().PlayAnimation("Skill2");
    }

    public void UseSprit()
    {
        Debug.Log("아이템 Sprit사용");
    }

    public void Switching()
    {
        PlayerManager.instance.SwitchHead();
        PlayerManager.instance.GetStateMachine().PlayAnimation("Switching");
    }

    public void Interact()
    {
        // 외부와 상호작용하는 키
        Debug.Log("Interaction triggered.");
    }

    public void ArrowDash()
    {
        Debug.Log("에로우 대시는 비활성화 되었습니다.");
    }

    public void Scroll()
    {
        // 아이템창 오픈하기
        InputManager.instance.SetInputReceiver(InputReceiver.UIOnly);
        //UIManager.instance.SetAciveUI("", true);
        Debug.Log("Scroll Head triggered.");
    }

    public void ActiveCollider()
    {
        groundCollider.enabled = true;
    }

    public void DeActiveCollider()
    {
        groundCollider.enabled = false;
    }

    public void SetGround()
    {
        PlayerManager.instance.GetStateMachine().ChangeState(PlayerGroundState.IsGround);
        Debug.Log("집에 가지마 베붸");
        ReSetCount();
    }
    public void ReSetCount()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        jumpCount = 0;
        dashCount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Debug.Log("지면 착지");
            SetGround();
        }
    }
}
