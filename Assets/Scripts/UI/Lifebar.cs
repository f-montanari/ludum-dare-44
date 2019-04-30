using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifebar : MonoBehaviour
{

    public RectTransform bar;
    public Entity target;
    private float initialHealth;
    private float initialWidth;

    // Start is called before the first frame update
    void Start()
    {
        initialWidth = bar.sizeDelta.x;        
    }

    public void SetTarget(Entity target)
    {
        this.target = target;
        initialHealth = target.Health;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            return;
        }
        float relativeHealth = target.Health / initialHealth;
        bar.sizeDelta = new Vector2(initialWidth * relativeHealth, bar.sizeDelta.y);
    }
}
