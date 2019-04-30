using UnityEngine;
using UnityEngine.AI;
public class PlayerAnimations : MonoBehaviour, IAnimations
{
    NavMeshAgent agent;
    Animator animator;

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void Die()
    {
        animator.SetTrigger("Die");
    }

    public void GetAttacked()
    {
        animator.SetTrigger("GetHit");
    }

    public bool isDying()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die_SwordShield"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        animator.SetFloat("speed", agent.velocity.sqrMagnitude);
    }
}

