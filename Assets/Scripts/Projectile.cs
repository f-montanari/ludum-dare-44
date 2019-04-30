using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public Vector3 direction;
    public float speed;
    public Entity owner;
    public float baseDamage;

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
            enemy.TakeDamage(baseDamage, owner);
        }
        Destroy(gameObject);
    }
}
