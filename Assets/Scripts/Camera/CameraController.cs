using System;
using Inputs;
using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        #region Properties
        
        [field: SerializeField] public Transform targetObject { get; private set; }
        [field: SerializeField] public float rotationSpeed { get; private set; }
        [field: SerializeField] public float zoomSpeed { get; private set; }
        [field: SerializeField] public float minZoom { get; private set; }
        [field: SerializeField] public float maxZoom { get; private set; }

        private UnityEngine.Camera cam;
        private float currentAngle = 0f;
        private float targetAngle = 0f;
        private float currentZoom = 10f;
        private bool isRotating = false;

        #endregion
        
        #region Methods
        
        private void Start()
        {
            if (InputManager.Instance == null)
            {
                throw new Exception("no input manager");
            }

            InputManager.Instance.CameraInput.OnLeftArrowClickDown += StartRotateLeft;
            InputManager.Instance.CameraInput.OnRightArrowClickDown += StartRotateRight;
            InputManager.Instance.CameraInput.OnMouseScroolForward += ZoomInCamera;
            InputManager.Instance.CameraInput.OnMouseScroolBack += ZoomOutCamera;

            cam = UnityEngine.Camera.main;
            currentZoom = Vector3.Distance(transform.position, targetObject.position);
        }

        private void Update()
        {
            if (isRotating)
            {
                float step = rotationSpeed * Time.deltaTime;
                float angle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, step);

                RotateCamera(angle);

                if (Mathf.Approximately(angle, targetAngle))
                {
                    isRotating = false;
                }
            }
        }

        private void StartRotateLeft()
        {
            if (isRotating) return;

            targetAngle = currentAngle - 90f;
            isRotating = true;
        }

        private void StartRotateRight()
        {
            if (isRotating) return;

            targetAngle = currentAngle + 90f;
            isRotating = true;
        }

        private void RotateCamera(float angle)
        {
            transform.RotateAround(targetObject.position, Vector3.up, angle - currentAngle);
            currentAngle = angle;
        }

        private void ZoomInCamera()
        {
            currentZoom = Mathf.Clamp(currentZoom - zoomSpeed * Time.deltaTime, minZoom, maxZoom);
            UpdateCameraPosition();
        }

        private void ZoomOutCamera()
        {
            currentZoom = Mathf.Clamp(currentZoom + zoomSpeed * Time.deltaTime, minZoom, maxZoom);
            UpdateCameraPosition();
        }

        private void UpdateCameraPosition()
        {
            Vector3 direction = (transform.position - targetObject.position).normalized;
            transform.position = targetObject.position + direction * currentZoom;

            transform.LookAt(targetObject);
        }
        
        #endregion
    }
}