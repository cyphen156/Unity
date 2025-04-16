using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField]
    public GameObject defaultHead;
    public Transform headMountPosition;

    private GameObject firstHead;
    private GameObject secondHead;
    private GameObject currentHead;
    public PlayerStateMachine stateMachine;
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

        stateMachine = new PlayerStateMachine();
    }

    private void OnEnable()
    {
        ResetPlayer();
    }

    private void Start()
    {
        startPosition = transform.position;
        firstHead = Instantiate(defaultHead, headMountPosition);
        currentHead = firstHead;
    }
    public void ResetPlayer()
    {
        currentHead = null;

        if (firstHead != null )
        {
            Destroy(firstHead);
        }
        if (secondHead != null)
        {
            Destroy(secondHead);
        }

        firstHead = Instantiate(defaultHead, headMountPosition);
        currentHead = firstHead;
        SetHeadState(currentHead, true);
        secondHead = null;
        transform.position = startPosition;
        ApplyHeadAnimator();
    }

    public void SwitchHead()
    {
        // �ٲ� �Ӹ����� ����
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
        // ���ο� Head �ν��Ͻ� ���� �� ��Ȱ��ȭ
        GameObject newHead = Instantiate(newHeadPrefab, headMountPosition);
        newHead.transform.localPosition = Vector3.zero;
        SetHeadState(newHead, false);

        // �ι�° �Ӹ��� ������ �����
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

            stateMachine.Initialize(animator);
        }

    }
}
