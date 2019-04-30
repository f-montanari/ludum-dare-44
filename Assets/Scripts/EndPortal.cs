using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPortal : MonoBehaviour
{
    Transform player;

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector3.Distance(player.position, this.transform.position) < 1f)
        {
            // We've entered the portal.
            Debug.Log("Player entered the portal");
            GameManager.instance.NewLevel();
        }
    }
}
