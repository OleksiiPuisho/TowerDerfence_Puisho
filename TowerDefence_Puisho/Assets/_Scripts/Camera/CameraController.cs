using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using Helpers.Events;

public class CameraController : MonoBehaviour
{
    private Transform _camera;
    private Animator _animator;
    [SerializeField] private float _speedPosition;
    [SerializeField] private float _speedZoom;

    [Header("Calmp Position")]
    [SerializeField] private float _minX;
    [SerializeField] private float _minZ;
    [SerializeField] private float _maxX;
    [SerializeField] private float _maxZ;

    [Header("Calmp Zoom")]
    [SerializeField] private float _minZoom;
    [SerializeField] private float _maxZoom;

    [SerializeField] private float _zoomDistance;
    private const float _speedCorrectedHeight = 40f;

    private Touch _touch0;
    private Touch _touch1;

    private Vector2 _centerScreen;
    void Awake()
    {
        EventAggregator.Subscribe<GameWinEvent>(GameWinChange);
        EventAggregator.Subscribe<GameOverEvent>(GameOverChange);
        _camera = gameObject.transform;
        _animator = GetComponent<Animator>();
        _centerScreen = new Vector2(Screen.width / 2, Screen.height / 2);
#if UNITY_EDITOR
        _speedPosition *= 2f;
#endif
    }

    void LateUpdate()
    {
        CameraRaycast();
        CameraMovement(_camera.transform);
        Zoom();
        _zoomDistance = Mathf.Clamp(_zoomDistance, _minZoom, _maxZoom);
    }
    private void CameraMovement(Transform camera)
    {
#if PLATFORM_ANDROID
        if (Input.touchCount == 1)
        {
            var touchPoint = Input.GetTouch(0);
            float speedX = touchPoint.deltaPosition.x * _speedPosition * Time.deltaTime;
            float speedZ = touchPoint.deltaPosition.y * _speedPosition * Time.deltaTime;

            camera.position = new Vector3(Mathf.Clamp(camera.position.x + speedX, _minX, _maxX), camera.position.y, Mathf.Clamp(camera.position.z + speedZ, _minZ, _maxZ));
        }
#endif
#if UNITY_EDITOR
        if(Input.GetKey(KeyCode.W))
        {
            camera.position = new Vector3(camera.position.x, camera.position.y, Mathf.Clamp(camera.position.z - _speedPosition * Time.deltaTime, _minZ, _maxZ));
        }
        if (Input.GetKey(KeyCode.S))
        {
            camera.position = new Vector3(camera.position.x, camera.position.y, Mathf.Clamp(camera.position.z + _speedPosition * Time.deltaTime, _minZ, _maxZ));
        }
        if (Input.GetKey(KeyCode.A))
        {
            camera.position = new Vector3(Mathf.Clamp(camera.position.x + _speedPosition * Time.deltaTime, _minX, _maxX), camera.position.y, camera.position.z);
        }
        if (Input.GetKey(KeyCode.D))
        {
            camera.position = new Vector3(Mathf.Clamp(camera.position.x - _speedPosition * Time.deltaTime, _minX, _maxX), camera.position.y, camera.position.z);
        }
#endif

    }
    private void Zoom()
    {
        if (Input.touchCount == 2)
        {
            _touch0 = Input.GetTouch(0);
            _touch1 = Input.GetTouch(1);
            float delta = (Mathf.Abs(_touch0.deltaPosition.x) + Mathf.Abs(_touch1.deltaPosition.x)) * _speedZoom * Time.deltaTime;
            if (_touch0.phase == TouchPhase.Moved && _touch1.phase == TouchPhase.Moved)
            {
                if (_touch0.position.x < _centerScreen.x)
                {
                    if (_touch0.deltaPosition.x < 0)
                        _zoomDistance -= delta;
                    else
                        _zoomDistance += delta;
                }
                else
                {
                    if (_touch0.deltaPosition.x < 0)
                        _zoomDistance += delta;
                    else
                        _zoomDistance -= delta;
                }
            }
        }
#if UNITY_EDITOR
        if(Input.mouseScrollDelta.y != 0)
        {
            _zoomDistance -= Input.mouseScrollDelta.y * _speedZoom;
        }
#endif
    }
    private void CameraRaycast()
    {
        if (Physics.Raycast(_camera.position, Vector3.down, out RaycastHit hit))
        {
            if (hit.distance < _zoomDistance - 1f)
            {
                _camera.position = new(_camera.position.x, _camera.position.y + _speedCorrectedHeight * Time.deltaTime, _camera.position.z);
            }
            else if (hit.distance > _zoomDistance + 1f)
            {
                _camera.position = new(_camera.position.x, _camera.position.y - _speedCorrectedHeight * Time.deltaTime, _camera.position.z);
            }
        }
    }
    private void GameWinChange(object sender, GameWinEvent eventData)
    {
        _animator.enabled = true;
        GetComponent<CameraController>().enabled = false;
    }
    private void GameOverChange(object sender, GameOverEvent eventData)
    {
        _animator.enabled = true;
        GetComponent<CameraController>().enabled = false;
    }
}
