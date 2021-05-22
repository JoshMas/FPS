using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;


namespace FramedWok.PlayerController
{
    /// <summary>
    /// Attach this script to a player model to give it controls designed for a first person platformer.
    /// You are able to adjust the values for walking, jumping, and air dashing.
    /// It's recommended to make the main camera a child of the object this script is attached to.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerPhysics))]
    public class PlayerController : NetworkBehaviour
    {
        private PlayerInput input;
        private PlayerPhysics physics;
        private Transform cameraPoint;

        #region movement
        /// <summary>
        /// How quickly the player accelerates
        /// </summary>
        [SerializeField, Min(0)] private float walkSpeed = 100.0f;
        /// <summary>
        /// The rate at which the player's current velocity is brought down 
        /// </summary>
        [SerializeField, Range(0, 1), Tooltip("How quickly the player's velocity is restricted to maximum. 0 means it's never used, 1 means it's used immediately")] private float rateOfRestriction = 0.5f;
        #endregion

        #region jump
        /// <summary>
        /// Enables/disable the jump
        /// </summary>
        [SerializeField] private bool canJump = true;
        /// <summary>
        /// The amount of control that the player's movement keys have on the player while airborne
        /// </summary>
        [SerializeField, Range(0, 1)] private float airControl = 1.0f;
        /// <summary>
        /// How much force the jump has
        /// </summary>
        [SerializeField, Min(0)] private float jumpStrength = 10.0f;
        /// <summary>
        /// How many times the player can jump before landing
        /// </summary>
        [SerializeField, Tooltip("The number of times the player can jump before landing")] private int numberOfJumps = 1;
        private int jumpCounter = 0;
        private bool isGrounded = true;
        //private float groundCheckCounter = 0.0f;
        #endregion

        #region dash
        /// <summary>
        /// Enables/disables the dash
        /// </summary>
        [SerializeField] private bool canDash = true;
        /// <summary>
        /// Restricts the dash to the XZ plane
        /// </summary>
        [SerializeField, Tooltip("Restrict the dash to the XZ plane")] private bool horizontalDashOnly = false;
        /// <summary>
        /// How much force the dash has
        /// </summary>
        [SerializeField, Min(0)] private float dashStrength = 20.0f;
        /// <summary>
        /// How long the dash goes for before ending
        /// </summary>
        [SerializeField, Min(0)] private float dashDuration = 0.1f;
        /// <summary>
        /// How long it takes for the dash to become available again
        /// </summary>
        [SerializeField, Min(0)] private float dashCooldown = 1.0f;
        public float dashTimer = 0.0f;
        private bool isDashing = false;
        #endregion

        private Vector3 rotation = Vector3.zero;
        private bool jump = false;
        private bool dash = false;
        private Vector3 movement = Vector3.zero;


        // Start is called before the first frame update
        void Start()
        {
            input = GetComponent<PlayerInput>();
            physics = GetComponent<PlayerPhysics>();
            Cursor.lockState = CursorLockMode.Locked;
            cameraPoint = GetComponentsInChildren<Transform>()[1];
            Transform cameraMain = Camera.main.transform;
            cameraMain.parent = cameraPoint;
            cameraMain.position = cameraPoint.position;
            cameraMain.rotation = cameraPoint.rotation;
        }

        // Update is called once per frame
        [Client]
        void Update()
        {
            if (!hasAuthority)
                return;


            rotation = input.GetCameraRotation();
            jump = Input.GetKeyDown(input.jumpKey) && canJump && jumpCounter < numberOfJumps;
            dash = Input.GetKeyDown(input.dashKey) && canDash && !isDashing && dashTimer <= 0;
            //Pause
            if (Input.GetKeyDown(KeyCode.Escape))
                Cursor.lockState = CursorLockMode.None;
            if (dashTimer > 0)
            {
                dashTimer -= Time.deltaTime;
            }

            //if (isServer)
            //{
            //    RpcActionStuff(rotation, jump, dash);

            //}
            //else
            //{
            //    CmdActionStuff(rotation, jump, dash);

            //}
            CmdActionStuff(rotation, jump, dash);
        }

        [Command]
        private void CmdActionStuff(Vector3 _rotation, bool _jump, bool _dash)
        {
            //Validation (probably not though)

            RpcActionStuff(_rotation, _jump, _dash);

            ////Set the camera angle
            //physics.Rotate(_rotation);

            ////Jumping
            //if (_jump)
            //{
            //    physics.Jump(jumpStrength);
            //    jumpCounter++;
            //    isGrounded = false;
            //}
            ////groundCheckCounter += Time.deltaTime;
            ////if(groundCheckCounter > 0.1f)
            ////{
            ////    isGrounded = physics.IsGrounded();
            ////    if (isGrounded)
            ////        jumpCounter = 0;
            ////    groundCheckCounter = 0;
            ////}

            ////Dashing
            //if (_dash)
            //{
            //    dashTimer = dashCooldown;
            //    StartCoroutine(nameof(Dash));
            //}
        }

        [ClientRpc]
        private void RpcActionStuff(Vector3 _rotation, bool _jump, bool _dash)
        {
            //Set the camera angle
            physics.Rotate(_rotation);

            //Jumping
            if (_jump)
            {
                physics.Jump(jumpStrength);
                jumpCounter++;
                isGrounded = false;
            }
            //groundCheckCounter += Time.deltaTime;
            //if(groundCheckCounter > 0.1f)
            //{
            //    isGrounded = physics.IsGrounded();
            //    if (isGrounded)
            //        jumpCounter = 0;
            //    groundCheckCounter = 0;
            //}

            //Dashing
            if (_dash)
            {
                dashTimer = dashCooldown;
                StartCoroutine(nameof(Dash));
            }
        }

        private void FixedUpdate()
        {
            if (!hasAuthority)
                return;

            movement = input.GetGroundMovementVector(isGrounded) * walkSpeed * Time.deltaTime * (isGrounded ? 1 : airControl);


            //if (isServer)
            //{
            //    RpcMoveStuff(movement);
            //}
            //else
            //{
            //    CmdMoveStuff(movement);
            //}
            CmdMoveStuff(movement);
        }

        [Command]
        private void CmdMoveStuff(Vector3 _movement)
        {
            //

            RpcMoveStuff(_movement);

            //Walking
            //physics.SetGroundMovement(_movement);
            ////Restrict velocity while on the ground
            //if (isGrounded)
            //    physics.RestrictVelocity(0, rateOfRestriction * Time.deltaTime);
        }

        [ClientRpc]
        private void RpcMoveStuff(Vector3 _movement)
        {
            //Walking
            physics.SetGroundMovement(_movement);
            //Restrict velocity while on the ground
            if (isGrounded)
                physics.RestrictVelocity(0, rateOfRestriction * Time.deltaTime);
        }

        /// <summary>
        /// Use the dash, and after a timer, cancel all momentum
        /// </summary>
        private IEnumerator Dash()
        {
            isGrounded = false;
            physics.Dash(physics.GetDashDirection(horizontalDashOnly, input.GetGroundMovementVector(isGrounded).normalized), dashStrength);
            isDashing = true;
            yield return new WaitForSeconds(dashDuration);
            physics.RestrictVelocity(0, 1);
            isDashing = false;
        }

        /// <summary>
        /// On collision, check if it's hit the ground - if so, reset the jump counter
        /// Also, end any dash if the player is in teh middle of one
        /// </summary>
        /// <param name="_collision"></param>
        private void OnCollisionEnter(Collision _collision)
        {
            StopCoroutine(nameof(Dash));
            isDashing = false;
            //physics.RestrictVelocity(walkSpeed, rateOfRestriction);
            isGrounded = true;
            //if (isGrounded)
            jumpCounter = 0;
        }
    }
}