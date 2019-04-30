using UnityEngine;
using UnityEngine.UI;

public class SkeletonCounter : MonoBehaviour
{
    public Text text;
    
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void LateUpdate()
    {
        text.text = "Available Skeletons: " + GameManager.instance.AvailableSkeletons;
    }
}
