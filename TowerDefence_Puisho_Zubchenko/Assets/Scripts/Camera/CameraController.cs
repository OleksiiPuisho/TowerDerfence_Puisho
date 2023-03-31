using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _camera;
    private float _speedCamera;
    void Start()
    {
        _camera = gameObject.transform;
    }

    void Update()
    {
        CameraMovement(_camera);
    }
    private void CameraMovement(Transform camera)
    {
        if(Input.touchCount > 0)
        {
            if(Input.touchCount == 1)
            {
                var touchPoint = Input.GetTouch(0);
                camera.position = new(camera.position.x + touchPoint.deltaPosition.x * Time.deltaTime, 
                    camera.position.y, camera.position.z + touchPoint.deltaPosition.y * Time.deltaTime);
            }
        }
    }
}
