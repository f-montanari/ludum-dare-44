using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavEntityBehaviour : Entity
{
    NavMeshAgent agent;
    public AnimationHelper animationHandler;
    public float stoppingDistance;


    #region Entity Implementation

    public override void Attack(Entity target)
    {
        if(lastAttackTime <= 0)
        {
            // We can attack
            
            if(animationHandler == null)
            {
                Debug.LogWarning("This agent doesn't have an animationHandler");
            }
            if (animationHandler != null)
            {                
                animationHandler.Attack();
            }

            // Face the enemy
            transform.LookAt(target.transform);
            target.TakeDamage(baseDamage, this);
            lastAttackTime = attackSpeed;
        }        
    }

    public override void Die()
    {        
        StartCoroutine("processDeath");
        // Stand still
        agent.velocity = Vector3.zero;
        agent.acceleration = 0;
        if (gameObject.CompareTag("Player"))
        {
            GameManager.instance.PlayerDied();
        }
    }

    IEnumerator processDeath()
    {
        
        if(Health <= -35f)
        {
            Destroy(gameObject);
        }

        animationHandler.Die();
        while(animationHandler.isDying())
        {
            yield return null;
        }        
        Destroy(gameObject);
    }

    public override void Heal(float amount)
    {
        // TODO: Play animations
        Health += amount;
    }

    public override void MoveTowards(Vector3 position)
    {
        // Instantly face the point we want to go to.
        transform.forward = (position - this.transform.position).normalized;        
        
        agent.SetDestination(position);
    }

    public override void TakeDamage(float amount, Entity source)
    {
        // TODO: Play animations
        animationHandler.GetAttacked();
        Health -= amount;

        // Fight back
        //if(currentEnemy == null)
            currentEnemy = source;
        

    }
    #endregion

    #region Unity callbacks
    // Start is called before the first frame update
    void Start()
    {        
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;

        animationHandler = GetComponent<AnimationHelper>();
        if (animationHandler == null)
        {
            Debug.LogError("This animation handler is null for some reason.");
        }        
    }

    // Update is called once per frame
    void Update()
    {

        if(currentEnemy != null)
        {
            animationHandler.isInCombat = true;
        }
        else
        {
            animationHandler.isInCombat = false;
        }        

        if(lastAttackTime > 0)
        {
            // We attacked recently, decriment timer.
            lastAttackTime -= attackSpeed * Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.AddSkeleton();
        }        
    }

    #endregion
}
