using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationHelper : MonoBehaviour, IAnimations
{
    NavMeshAgent agent;
    public Animator anim;

    private bool IsDying = false;
    public bool isInCombat = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speed", agent.velocity.magnitude);
        anim.SetBool("IsInCombat", isInCombat);
        anim.SetBool("IsDying", IsDying);
    }

    public void GetAttacked()
    {
        anim.SetTrigger("GetHit");
    }
    public void Die()
    {
        IsDying = true;
        anim.SetTrigger("Die");                
    }
    public void EndDie()
    {
        IsDying = false;
    }    

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }

    public bool isDying()
    {
        return IsDying;
    }
}
