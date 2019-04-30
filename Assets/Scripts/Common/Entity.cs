using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    // Vars
    [SerializeField]
    private float baseHealth;
    [SerializeField]
    public float baseDamage;
    [SerializeField]
    protected float attackSpeed;
    [SerializeField]
    protected float attackRange;

    protected float lastAttackTime = 0;

    public Entity currentTarget;
    public Entity currentEnemy;

    // Properties
    public float Health {
        get
        {
            return baseHealth;
        }
        set
        {
            baseHealth = value;
            if( baseHealth <= 0)
            {
                Die();
            }            
        }
    }
    public float AttackRange
    {
        get
        {
            return attackRange;
        }
    }

    public float AttackSpeed
    {
        get
        {
            return attackSpeed;
        }
    }

    // Methods
    public abstract void MoveTowards(Vector3 position);
    public abstract void Attack(Entity target);
    public abstract void Heal(float amount);
    public abstract void TakeDamage(float amount, Entity source);
    public abstract void Die();

}
