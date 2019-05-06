using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;
    public float smoothSpeed = 2f;

    public float currentZoom = 1f;
    public float maxZoom = 3f;
    public float minZoom = .3f;
    public float yawSpeed = 70;
    public float zoomSensitivity = .7f;
    public float height = 5f;
    float dst;

    float zoomSmoothV;
    float targetZoom;

    public void StartChasing(Transform target)
    {
        this.target = target;
        StartChasing();
    }

    public void StartChasing()
    {
        dst = offset.magnitude;
        transform.LookAt(target);
        targetZoom = currentZoom;
    }

    void Update()
    {
        if(target == null)
        {
            return;
        }

        float scroll = Input.GetAxisRaw("Mouse ScrollWheel") * zoomSensitivity;

        if (scroll != 0f)
        {
            targetZoom = Mathf.Clamp(targetZoom - scroll, minZoom, maxZoom);
        }
        currentZoom = Mathf.SmoothDamp(currentZoom, targetZoom, ref zoomSmoothV, .15f);
    }

    void LateUpdate()
    {
        if(target == null)
        {
            return;
        }

        transform.position = target.position - transform.forward * dst * currentZoom;
        transform.LookAt(target.position);

        float yawInput = Input.GetAxisRaw("Horizontal");
        transform.RotateAround(target.position, Vector3.up, -yawInput * yawSpeed * Time.deltaTime);
        transform.position += new Vector3(0, height, 0);        
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CameraManager))]
public class CameraEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Center Camera"))
        {
            CameraManager manager = (CameraManager)target;
            manager.StartChasing();
        }
    }
}
#endif