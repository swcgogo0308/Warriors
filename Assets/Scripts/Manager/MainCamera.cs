using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour
{

    public Transform target;

    new private Camera camera;
    private LimitArea limitArea;
    private float fixedZAxis;

    public float followSpeed;

    void Start()
    {
        SetupLimitArea();
        fixedZAxis = transform.position.z;
    }

    void LateUpdate()
    {
        Vector3 position = limitArea.Clamp(Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime));
        //Vector3 position = limitArea.Clamp(target.position);
        position.z = fixedZAxis;

        transform.position = position;
    }

    private void SetupLimitArea()
    {
        camera = GetComponent<Camera>();

        if (!camera.orthographic)
        {
            Debug.LogError("Error: main camera is not orthographic.");
            return;
        }

        float cameraHeight = camera.orthographicSize;
        float cameraWidth = cameraHeight * camera.aspect;

        limitArea = MapManager.LimitArea.AddMargin(cameraHeight, cameraWidth);
    }
}
