using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField]
    public GameObject defaultHead;
    public Transform headMountPosition;

    public GameObject firstHead;
    public GameObject secondHead;
    public GameObject currentHead;
    public PlayerStateMachine stateMachine;
    public PlayerController playerController;
    private Vector2 startPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        stateMachine = GetComponent<PlayerStateMachine>();
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        ResetPlayer();
    }

    private void Start()
    {
        startPosition = transform.position;
        ResetPlayer();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
    public void ResetPlayer()
    {

        if (firstHead != null )
        {
            Destroy(firstHead);
        }
        if (secondHead != null)
        {
            Destroy(secondHead);
        }

        currentHead = null;

        firstHead = Instantiate(defaultHead, headMountPosition);
        currentHead = firstHead;
        SetHeadState(currentHead, true);
        secondHead = null;
        transform.position = startPosition;
        ApplyHeadAnimator();
    }

    public void SwitchHead()
    {
        // 바꿀 머리통이 없음
        if (secondHead == null)
        {
            return;
        }

        if (currentHead == firstHead)
        {
            // AvatorForm Active
            firstHead.SetActive(false);
            secondHead.SetActive(true);
            currentHead = secondHead;
        }
        else
        {
            secondHead.SetActive(false);
            firstHead.SetActive(true);
            currentHead = firstHead;
        }
        ApplyHeadAnimator();
    }

    public void GetHead(GameObject newHeadPrefab)
    {
        // 새로운 Head 인스턴스 생성 및 비활성화
        GameObject newHead = Instantiate(newHeadPrefab, headMountPosition);
        newHead.transform.localPosition = Vector3.zero;
        SetHeadState(newHead, false);

        // 두번째 머리통 슬롯이 비었음
        if (firstHead != null && secondHead == null)
        {
            secondHead = newHead;
        }
        else if (firstHead != null && secondHead != null)
        {
            SetHeadState(firstHead, false);
            firstHead = newHead;
            SetHeadState(firstHead, true);
            SetHeadState(secondHead, false);
            currentHead = firstHead;
        }
        else
        {
            firstHead = newHead;
            currentHead = firstHead;
            SetHeadState(firstHead, true);
        }
    }
    private void SetHeadState(GameObject head, bool isEquipped)
    {
        Transform avatar = head.transform.Find("AvatarForm");
        Transform item = head.transform.Find("ItemForm");

        if (avatar != null)
        {
            avatar.gameObject.SetActive(isEquipped);
        }

        if (item != null)
        {
            item.gameObject.SetActive(!isEquipped);
        }
    }
    public GameObject GetCurrentHead()
    {
        return currentHead;
    }

    public PlayerStateMachine GetStateMachine()
    {
        return stateMachine;
    }

    public void ApplyHeadAnimator()
    {
        if (currentHead == null) return;

        HeadBase headBase = currentHead.GetComponent<HeadBase>();
        if (headBase != null && headBase.overrideController != null)
        {
            Animator animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = headBase.overrideController;
            playerController.SetHitBoxCollider();
            stateMachine.Initialize(animator);
        }
        Debug.Log($"[ApplyHeadAnimator] 설정된 애니메이터: {headBase.overrideController.name}");
    }

    public void SetStartPosition()
    {
        transform.position = startPosition;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;

    }

    public BoxCollider2D GetHitBoxCollider()
    {
        if (currentHead == null)
        {
            return null;
        }
        Debug.Log(currentHead.ToString() + "콜라이더 설정");
        return currentHead.GetComponent<BoxCollider2D>();
    }
}
