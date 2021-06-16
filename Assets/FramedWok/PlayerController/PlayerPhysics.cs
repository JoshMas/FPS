using UnityEngine;

using Mirror;

namespace FramedWok.PlayerController
{
    /// <summary>
    /// Class that handles physics simulation for the first person platforming controller
    /// Does not use the mass of the attached RigidBody
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerPhysics : MonoBehaviour
    {
        private Rigidbody playerRigidbody;
        private Collider playerCollider;
        private Camera playerCamera;

        /// <summary>
        /// The length of the Raycast used to check if the playerCollider is on top of another collider
        /// </summary>
        //[SerializeField, Tooltip("This value should be slightly more than half your player model's height")] private float groundCheckLength = 1.05f;

        private void Awake()
        {
            playerRigidbody = GetComponent<Rigidbody>();
            playerRigidbody.freezeRotation = true;
            playerRigidbody.isKinematic = false;
            playerRigidbody.useGravity = false;
            playerCollider = GetComponent<Collider>();
            if (playerCollider == null)
                playerCollider = gameObject.AddComponent<CapsuleCollider>();
            playerCamera = Camera.main;
        }

        /// <summary>
        /// Adds a force as a velocity change, for moving on the XZ plane
        /// </summary>
        /// <param name="_acceleration">The player's movement vector</param>
        public void SetGroundMovement(Vector3 _acceleration)
        {
            playerRigidbody.MovePosition(playerRigidbody.position + _acceleration);
            playerRigidbody.AddForce(Vector3.down * 19.6f, ForceMode.Acceleration);
        }

        /// <summary>
        /// Lerps the rigidbody's velocity between itself and a restricted version of itself
        /// </summary>
        /// <param name="_maximum">The restricted magnitude for the velocity</param>
        /// <param name="_rate">The lerp value</param>
        public void RestrictVelocity(float _maximum, float _rate)
        {
            if(playerRigidbody.velocity.magnitude > _maximum)
                playerRigidbody.velocity = Vector3.Lerp(playerRigidbody.velocity, Vector3.ClampMagnitude(playerRigidbody.velocity, _maximum), _rate);        }

        /// <summary>
        /// Rotates the player's transform and the camera to point in the new direction
        /// </summary>
        public void Rotate(Vector3 _rotation)
        {
            transform.localEulerAngles = new Vector3(0, _rotation.y, 0);
            playerCamera.transform.localEulerAngles = new Vector3(_rotation.x, 0, 0);
        }

        /// <summary>
        /// Sets the upwards velocity to 0 before adding an upwards force
        /// </summary>
        /// <param name="_jumpStrength">The amount of upwards force applied</param>
        public void Jump(float _jumpStrength)
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
            playerRigidbody.AddForce(Vector3.up * _jumpStrength, ForceMode.VelocityChange);
        }

        /// <summary>
        /// Apply an amount of force in s certain direction as a dash
        /// </summary>
        /// <param name="_direction">Which way the dash goes</param>
        /// <param name="_dashStrength">Strength of the dash</param>
        public void Dash(Vector3 _direction, float _dashStrength)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.AddForce(_direction * _dashStrength, ForceMode.VelocityChange);
        }

        /// <summary>
        /// Get the direction the camera is looking as a normalised vector3
        /// </summary>
        /// <param name="_restrictYAxis">If true, the dash can only travel on the XZ plane</param>
        public Vector3 GetDashDirection(bool _restrictYAxis, Vector3 _moveDir)
        {
            Vector3 initialDir = playerCamera.transform.TransformDirection(Vector3.forward);
            Vector3 direction = _moveDir == Vector3.zero ? initialDir : new Vector3(_moveDir.x, initialDir.y, _moveDir.z);
            if (_restrictYAxis)
            {
                direction = new Vector3(direction.x, 0, direction.z);
            }
            return direction.normalized;
        }

        /// <summary>
        /// Returns true if the Raycast pointing downwards hits anything just below the collider
        /// </summary>
        //public bool IsGrounded()
        //{
        //    return Physics.Raycast(transform.position, Vector3.down, groundCheckLength);
        //}
    }
}