using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    // TODO: Move player instance to another class
    public Entity player;
    public Camera camera;
    public LayerMask floorMask;

    // Should move this aswell
    [Header("Projectile launching")]
    public GameObject fireballPrefab;
    public float projectileSpeed = 25f;

    private float attackTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        camera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        attackTime -= Time.deltaTime;
        if (attackTime <= 0) attackTime = 0;

        if(camera == null)
        {
            camera = FindObjectOfType<Camera>();
            if(camera == null)
            {
                gameObject.SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            // Shoot out a ray
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If we hit
            if (Physics.Raycast(ray, out hit, floorMask))
            {
                player.MoveTowards(hit.point);
            }
        }
        if(Input.GetMouseButtonDown(0))
        {
            if(attackTime > 0)
            {
                return;
            }
            else
            {
                attackTime = player.AttackSpeed;

                // Shoot out a ray
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // If we hit
                if (Physics.Raycast(ray, out hit, floorMask))
                {
                    SpawnProjectile(hit.point);
                }
            }            
        }
    }

    private void SpawnProjectile(Vector3 point)
    {
        if(player == null)
        {
            // We're probably dead.
            return;
        }

        Vector3 spawnpoint = player.transform.position + (point - player.transform.position).normalized * 1f;
        Vector3 correctedSpawnpoint = new Vector3(spawnpoint.x, 1f, spawnpoint.z);
        Projectile fireball = Instantiate(fireballPrefab, correctedSpawnpoint, Quaternion.identity).GetComponent<Projectile>();
        fireball.baseDamage = player.baseDamage;
        fireball.speed = projectileSpeed;
        fireball.owner = player;
        Vector3 direction = (point - player.transform.position).normalized;
        fireball.direction = new Vector3(direction.x,0,direction.z);
    }
}
