using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FramedWok.PlayerController
{
    /// <summary>
    /// Attach this script to a player model to give it controls designed for a first person platformer.
    /// You are able to adjust the values for walking, jumping, and air dashing.
    /// It's recommended to make the main camera a child of the object this script is attached to.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerPhysics))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerInput input;
        private PlayerPhysics physics;

        /// <summary>
        /// How quickly the player accelerates
        /// </summary>
        [SerializeField, Min(0)] private float walkAcceleration = 100.0f;
        /// <summary>
        /// The player's theoretical max velocity
        /// </summary>
        [SerializeField, Min(0)] private float maxVelocity = 10.0f;
        /// <summary>
        /// The rate at which the player's current velocity is brought down 
        /// </summary>
        [SerializeField, Range(0, 1), Tooltip("How quickly the player's velocity is restricted to maximum. 0 means it's never used, 1 means it's used immediately")] private float rateOfRestriction = 0.5f;

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
        private float groundCheckCounter = 0.0f;

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

        // Start is called before the first frame update
        void Start()
        {
            input = GetComponent<PlayerInput>();
            physics = GetComponent<PlayerPhysics>();
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update()
        {
            //Walking
            physics.AddGroundAcceleration(input.GetGroundMovementVector() * walkAcceleration * Time.deltaTime * (isGrounded ? 1 : airControl));
            //Restrict velocity while on the ground
            if(isGrounded)
                physics.RestrictVelocity(maxVelocity, rateOfRestriction * Time.deltaTime);
            //Set the camera angle
            physics.Rotate(input.GetCameraRotation());

            //Jumping
            if (Input.GetKeyDown(input.jumpKey) && canJump && jumpCounter < numberOfJumps)
            {
                physics.Jump(jumpStrength);
                jumpCounter++;
                isGrounded = false;
            }
            groundCheckCounter += Time.deltaTime;
            if(groundCheckCounter > 0.1f)
            {
                isGrounded = physics.IsGrounded();
                groundCheckCounter = 0;
            }

            //Dashing
            if(Input.GetKeyDown(input.dashKey) && canDash && !isDashing && dashTimer <= 0)
            {
                dashTimer = dashCooldown;
                StartCoroutine(nameof(Dash));
            }
            if(dashTimer > 0)
            {
                dashTimer -= Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            //Remove the walking physics from Update and put them in here
        }

        /// <summary>
        /// Use the dash, and after a timer, cancel all momentum
        /// </summary>
        private IEnumerator Dash()
        {
            physics.Dash(physics.GetDashDirection(horizontalDashOnly, input.GetGroundMovementVector().normalized), dashStrength);
            isDashing = true;
            yield return new WaitForSeconds(dashDuration);
            physics.RestrictVelocity(maxVelocity, 1);
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
            physics.RestrictVelocity(maxVelocity, rateOfRestriction);
            isGrounded = physics.IsGrounded();
            if (isGrounded)
                jumpCounter = 0;
        }
    }
}