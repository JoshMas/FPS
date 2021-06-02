using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FramedWok.PlayerController
{
    /// <summary>
    /// Class that handles input for the first person platforming controller
    /// Uses the horizontal and vertical input axes for ground movement
    /// Default jump and dash buttons are Space and LeftShift respectively
    /// </summary>
    public class PlayerInput : MonoBehaviour
    {
      
        [Header("Rotation Variables")]
       
       
        public float minY = -60, maxY = 60;
        private float _rotY;
        private float _rotX;
        public KeyCode jumpKey = KeyCode.Space;
        public KeyCode dashKey = KeyCode.LeftShift;
     
        /// How much the mouse movement affects the rotation of the camera
        /// </summary>
        [SerializeField] private Vector2 mouseSensitivity = new Vector2(5.0f, 2.0f);

        /// <summary>
        /// Returns a movement vector on the XZ plane
        /// </summary>
        public Vector3 GetGroundMovementVector(bool _isGrounded)
        {
            Vector3 vector = (transform.TransformPoint(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))) - transform.position);
            vector = Vector3.ClampMagnitude(vector, 1.0f);
            return vector;
        }

        /// <summary>
        /// Returns the new rotation of the camera
        /// </summary>
        public Vector3 GetCameraRotation()
        {
            //Maximum Y rotation
            _rotY += Input.GetAxis("Mouse Y") * mouseSensitivity.y;
            _rotY = Mathf.Clamp(_rotY, minY, maxY);
            _rotX += Input.GetAxis("Mouse X") * mouseSensitivity.x;

            Vector3 rotation = new Vector3(-_rotY, _rotX, 0);
            Camera.main.transform.eulerAngles = rotation;
            //rotation.Set(rotation.x, rotation.y, 0);
            return rotation;
        }
    }
}