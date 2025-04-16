using System.Collections;
using UnityEngine;
using static InputManager;
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
    private BoxCollider2D boxCollider;
    public bool isColliderEnable;
    public Vector2 currentDirection;
    public Vector2 jumpDirection;
    private PlayerMovement playerMovement;

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
        boxCollider = GetComponent<BoxCollider2D>();
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
        currentDirection = Vector2.left;
        playerMovement.Move(currentDirection * moveSpeed);
        transform.localScale = new Vector3(-1, 1, 1);
        PlayerManager.instance.GetStateMachine().Walk();
    }

    public void MoveRight()
    {
        currentDirection = Vector2.right;
        playerMovement.Move(currentDirection * moveSpeed);
        transform.localScale = new Vector3(1, 1, 1);
        PlayerManager.instance.GetStateMachine().Walk();
    }

    public void Jump()
    {
        DeActiveCollider();
        if (jumpCount < ActionLimit)
        {
            jumpCount++;
            playerMovement.Move(jumpDirection * jumpForce);
            PlayerManager.instance.GetStateMachine().Jump();
        }
    }

    public void Attack()
    {
        if (jumpCount != 0)
        {
            PlayerManager.instance.GetStateMachine().Attack();
        }
    }

    public void Dash()
    {
        if (dashCount < ActionLimit)
        {
            dashCount++;
            // 4프레임이니까
            PlayerManager.instance.GetStateMachine().Dash();
            StartCoroutine(StartDash(currentDirection, 4));
        }
    }

    IEnumerator StartDash(Vector2 currentDirection, int repeatCount)
    {
        for (int i = 0; i < repeatCount; ++i)
        {
            playerMovement.Move(currentDirection * moveSpeed);
            yield return null;
        }
    }
    public void UseSkill1()
    {
        PlayerManager.instance.GetStateMachine().Skill1();

    }

    public void UseSkill2()
    {
        PlayerManager.instance.GetStateMachine().Skill2();
    }

    public void UseSprit()
    {
        Debug.Log("아이템 Sprit사용");
    }

    public void Switching()
    {
        PlayerManager.instance.SwitchHead();
        PlayerManager.instance.GetStateMachine().Switching();
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
        boxCollider.enabled = true;
    }

    public void DeActiveCollider()
    {
        boxCollider.enabled = false;
    }
    public void ReSetCount()
    {
        jumpCount = 0;
        dashCount = 0;
    }
}
