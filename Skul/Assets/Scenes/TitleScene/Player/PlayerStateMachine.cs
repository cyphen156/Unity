using UnityEngine;

public class PlayerStateMachine
{
    private Animator animator;

    public void Initialize(Animator animator)
    {
        this.animator = animator;
    }

    public void PlayAnimation(string triggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }

    //public void SetAnimationState
    public void Tick()
    {
    }
    public void Walk()
    {
        PlayAnimation("Walk");
    }

    public void Jump()
    {
        PlayAnimation("Jump");
    }

    public void Attack()
    {
        PlayAnimation("Attack");
    }
    public void Dash()
    {
        PlayAnimation("Dash");
    }
    public void Skill1()
    {
        PlayAnimation("Skill1");
    }

    public void Skill2()
    {
        PlayAnimation("Skill2");
    }
    public void Switching()
    {
        PlayAnimation("Switching");
    }
}
