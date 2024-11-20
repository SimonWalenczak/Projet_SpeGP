using System;
using Common;

namespace Inputs
{
    /// <summary>
    /// This class handle the input scheme and create classes to handle each action scheme
    /// </summary>
    public class InputManager : Singleton<InputManager>
    {
        public InputActionScheme Scheme { get; private set; }
        public InputCameraController CameraInput { get; private set; }

        protected override void InternalAwake()
        {
            Scheme = new InputActionScheme();
            Scheme.Enable();
            CameraInput = new InputCameraController(this);
        }
    }

    public class InputCameraController
    {
        public Action OnLeftArrowClickDown { get; set; }
        public Action OnRightArrowClickDown { get; set; }
        public Action OnMouseScroolForward { get; set; }
        public Action OnMouseScroolBack { get; set; }

        
        public InputCameraController(InputManager manager)
        {
            manager.Scheme.Camera.RotateLeft.started += context => OnLeftArrowClickDown?.Invoke();
            manager.Scheme.Camera.RotateRight.started += context => OnRightArrowClickDown?.Invoke();

            manager.Scheme.Camera.ZoomIn.started += context => OnMouseScroolForward?.Invoke();
            manager.Scheme.Camera.ZoomOut.started += context => OnMouseScroolBack?.Invoke();
        }
    }
}