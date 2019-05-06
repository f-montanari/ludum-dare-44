using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Vector3 direction;
    public float speed;
    public Entity owner;
    public float baseDamage;
    public GameObject OnHitParticle;
    public float OnHitParticleLifeTime = 0.2f;

    private float maxLife = 5f;
    private float currentLife = 0f;
    // Start is called before the first frame update
    void Start()
    {        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += direction * speed * Time.deltaTime;
        currentLife += Time.deltaTime;
        if(currentLife >= maxLife)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Collided with: " + collision.gameObject.name);
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Entity enemy = collision.transform.root.gameObject.GetComponent<NavEntityBehaviour>();
            GameManager.instance.UpdateSkeletonTargets(enemy);            
            enemy.TakeDamage(baseDamage, owner);
        }

        if(OnHitParticle != null)
        {
            GameObject particle = Instantiate(OnHitParticle, transform.position, transform.rotation);
            Destroy(particle, OnHitParticleLifeTime);
        }
        Destroy(gameObject);
    }
}
