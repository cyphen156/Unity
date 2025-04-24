using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerStateMachine : MonoBehaviour 
{
    // Bool 베이스 상태 전이
    public enum PlayerGroundState
    {
        IsGround,
        IsFalling
    }

    private Animator animator;

    private float currentTime;
    private float setTime;
    public PlayerGroundState currentGroundState;
    private bool isAttacking; 
    public bool isDashing;
    private void Awake()
    {
        currentTime = 0.0f;
        setTime = 10.0f;    // 10초 대기했으면 많이 기다려 줫다
        currentGroundState = PlayerGroundState.IsGround;
        isAttacking = false;
        isDashing = false;
    }

    private void Update()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("Idle"))
        {
            currentTime += Time.deltaTime;

            if (currentTime > setTime)
            {
                PlayAnimation("Wait");
            }
        }
        else
        {
            currentTime = 0;
        }
    }
    public void Initialize(Animator animator)
    {
        this.animator = animator;
    }

    public void PlayAttackAnimation()
    {
        
        if (isAttacking)
        {
            return;
        }
        isAttacking = true;
        animator.SetTrigger("Attack");
        PlayerManager.instance.GetStateMachine().SetBoolState("IsAttacking", true);
    }
    public void PlayFall()
    {
        if (currentGroundState == PlayerGroundState.IsFalling)
        {
            return;
        }

        PlayAnimation("Fall", PlayerGroundState.IsFalling);
        //animator.SetTrigger("Fall");
        //ChangeState(PlayerGroundState.IsFalling);
    }
    public void PlayAnimation(string triggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }

    public void PlayAnimation(string triggerName, PlayerGroundState newState)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
            ChangeState(newState);
        }
    }

    public void SetBoolState(string boolStateName, bool state)
    {
        if (boolStateName == "IsAttacking")
        {
            isAttacking = state;
        }
        animator.SetBool(boolStateName, state);

    }

    public void ChangeState(PlayerGroundState newState)
    {
        if (currentGroundState == newState)
        {
            return;
        }
        ExitState(currentGroundState);
        currentGroundState = newState;
        EnterState(newState);
    }
    private void EnterState(PlayerGroundState newState)
    {
        switch (newState)
        {
            case PlayerGroundState.IsFalling:
                SetBoolState("IsFalling", true);
                break;
            case PlayerGroundState.IsGround:
                SetBoolState("IsGround", true);
                break;
        }
    }
    private void ExitState(PlayerGroundState oldState)
    {
        switch (oldState)
        {
            case PlayerGroundState.IsFalling:
                SetBoolState("IsFalling", false);
                break;
            case PlayerGroundState.IsGround:
                SetBoolState("IsGround", false);
                break;
        }
    }

    public void SetDash(bool state)
    {
        isDashing = state;
    }

    public void PlayDeathSequence()
    {
        animator.SetTrigger("Dead");

        PlayerManager.instance.StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        while (!state.IsName("Dead"))
        {
            yield return null;
            state = animator.GetCurrentAnimatorStateInfo(0);
        }

        yield return new WaitForSeconds(state.length);

        GameManager.instance.ChangeScene("Stage1");
    }
}
