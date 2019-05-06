using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyScript : MonoBehaviour
{

    public float aliveTime = 0.2f;

    private void Start()
    {
        Destroy(this.gameObject, aliveTime);
    }
}
