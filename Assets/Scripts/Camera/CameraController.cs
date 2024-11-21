using UnityEngine;
using DG.Tweening;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        #region Properties
        
        [field: Header("Camera Settings")]
        [field: SerializeField] public Transform target { get; private set; }
        [field: SerializeField] public float rotationSpeed { get; private set; }
        [field: SerializeField] public float moveSpeed { get; private set; }
        [field: SerializeField] public float zoomSpeed { get; private set; }
        [field: SerializeField] public float minZoom { get; private set; }
        [field: SerializeField] public float maxZoom { get; private set; }

        private Vector3 _currentForward;
        private Vector3 _currentRight;
        private float _currentDistance = 20f;
        private bool _isRotating;

        #endregion
        
        #region Methods
        
        private void Start()
        {
            if (target == null)
            {
                Debug.LogError("Target not assigned. Please assign a target in the inspector.");
                return;
            }

            UpdateMovementAxes();
        }

        private void Update()
        {
            if (target == null) return;

            HandleRotationInput();
            HandleMovementInput();
            HandleZoomInput();
            UpdateCameraPosition();
        }

        private void HandleRotationInput()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                RotateAroundTarget(-90f);
            else if (Input.GetKeyDown(KeyCode.E))
                RotateAroundTarget(90f);
        }

        private void RotateAroundTarget(float angle)
        {
            if (_isRotating) return;

            _isRotating = true;
            target.DORotate(target.eulerAngles + new Vector3(0, angle, 0), 0.5f)
                .OnComplete(UpdateMovementAxes);
        }

        private void HandleMovementInput()
        {
            Vector3 moveDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) moveDirection += _currentForward;
            if (Input.GetKey(KeyCode.S)) moveDirection -= _currentForward;
            if (Input.GetKey(KeyCode.A)) moveDirection -= _currentRight;
            if (Input.GetKey(KeyCode.D)) moveDirection += _currentRight;

            ApplyMovement(moveDirection);
        }

        private void ApplyMovement(Vector3 direction)
        {
            direction.Normalize();
            target.position += direction * moveSpeed * Time.deltaTime;
        }

        private void HandleZoomInput()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                AdjustZoom(scroll);
            }
        }

        private void AdjustZoom(float scroll)
        {
            _currentDistance -= scroll * zoomSpeed;
            _currentDistance = Mathf.Clamp(_currentDistance, minZoom, maxZoom);
        }

        private void UpdateMovementAxes()
        {
            _currentForward = target.forward;
            _currentRight = target.right;
            _isRotating = false;
        }

        private void UpdateCameraPosition()
        {
            Vector3 targetPosition = target.position - target.forward * _currentDistance + Vector3.up * (_currentDistance * 0.5f);
            transform.position = targetPosition;

            Vector3 directionToTarget = target.position - transform.position;
            transform.rotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
        }
        
        #endregion
    }
}
