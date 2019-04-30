using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{  
    enum IState
    {
        FOLLOWING,
        IDLE,
        ATTACKING
    }

    Entity myEntity;    
    IState currentState;

    public float followingDistance = 15f;
    public float minFollowingDistance = 3f;
    public float viewDistance = 10f;

    // Start is called before the first frame update
    void Start()
    {                        
        myEntity = GetComponent<NavEntityBehaviour>();
        myEntity.currentTarget = null;
        currentState = IState.IDLE;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(currentState)
        {
            case IState.IDLE:
                IdleUpdate();
                break;
            case IState.FOLLOWING:
                FollowingUpdate();                
                break;
            case IState.ATTACKING:
                AttackingUpdate();
                break;
        }
    }

    private void AttackingUpdate()
    {
        if(myEntity.currentEnemy == null)
        {
            // We defeated the enemy, we can go back to Idle state.
            currentState = IState.IDLE;
            myEntity.currentTarget = null;
            return;
        }
        if (Vector3.Distance(myEntity.transform.position, myEntity.currentEnemy.transform.position) > myEntity.AttackRange)
        {
            // Is he too far away?
            myEntity.currentTarget = myEntity.currentEnemy;
            currentState = IState.FOLLOWING;
        }
        else
        {
            myEntity.Attack(myEntity.currentEnemy);
        }
    }

    private void FollowingUpdate()
    {
        if(myEntity.currentTarget == null)
        {
            // The target has died or something... Go back to idling, or following the player.
            currentState = IState.IDLE;            
            return;
        }

        // Are we getting attacked?
        if (myEntity.currentEnemy != null && myEntity.currentTarget != myEntity.currentEnemy)
        {
            // Focus first the one who's attacking
            myEntity.currentTarget = myEntity.currentEnemy;
        }

        // Are we there yet?
        if (Vector3.Distance(myEntity.transform.position, myEntity.currentTarget.transform.position) > myEntity.AttackRange)
        {
            // We're not, keep chasing target if we don't find any enemies while walking.
            if(myEntity.currentEnemy == null)
            {
                myEntity.currentEnemy = CheckForEntity("Player");
                if (myEntity.currentEnemy != null)
                {
                    Debug.Log("Found an enemy!");
                    myEntity.currentTarget = myEntity.currentEnemy;
                    currentState = IState.ATTACKING;
                    return;
                }
            }            
            myEntity.MoveTowards(myEntity.currentTarget.transform.position);
        }
        else
        {
            // We're! Were we chasing an enemy, or a friend?
            if(myEntity.currentEnemy != null)
            {
                currentState = IState.ATTACKING;
            }
            else
            {
                currentState = IState.IDLE;
            }            
        }
    }

    /// <summary>
    /// Checks for an entity in it's view distance.
    /// </summary>
    /// <param name="Tag">Tag of the entity. The GameObject tagged must have an Entity component (such as NavEntityBehaviour)</param>
    /// <returns>The first entity that's found.</returns>
    Entity CheckForEntity(string Tag)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, viewDistance);
        NavEntityBehaviour FoundEntity = null;
        foreach (Collider col in cols)
        {
            if(col.transform.root.gameObject.CompareTag(Tag))
            {
                Debug.Log("Found an enemy withing viewing radius: " + col.transform.root.name);
                FoundEntity = col.transform.root.gameObject.GetComponentInParent<NavEntityBehaviour>();
                break;
            }
        }

        if(FoundEntity == null)
        {
            return FoundEntity;
        }
        

        // It's in the viewing zone, but... Can we actually see it?
        Vector3 direction = FoundEntity.transform.position - this.transform.position;

        RaycastHit hit;
        // Does the ray intersect any objects? (Added offset to exclude the player)
        if (Physics.Raycast(transform.position + direction * 0.25f + Vector3.up * 0.25f , direction, out hit, Mathf.Infinity))
        {            
            if(hit.collider.transform.root.gameObject.CompareTag(Tag))
            {
                // We hit the same entity (or one that has the same Tag, so it's the same)                
                return FoundEntity;
            }
            else
            {
                return null;
            }
        }
        else
        {            
            return null;
        }

    }

    void IdleUpdate()
    {

        // OPTIONAL: Ocational moving?

        // First attack, then follow
        myEntity.currentEnemy = CheckForEntity("Player");
        if(myEntity.currentEnemy != null)
        {
            Debug.Log("Found the player!");
            myEntity.currentTarget = myEntity.currentEnemy;
            currentState = IState.ATTACKING;
            return;
        }              
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, viewDistance);
    }
}
