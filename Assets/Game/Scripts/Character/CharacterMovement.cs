using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Automatically handles player movement and the animation that a particular model should play.
/// </summary>
public class CharacterMovement : MonoBehaviour
{
    /*
     * Serializable
     */
    [Header("Configuration")]
    [SerializeField]
    [Tooltip("What layer can this character walk on?")]
    private LayerMask walkableLayers;
    [SerializeField]
    [Tooltip("How fast this character's animation plays")]
    private float animationSpeed = 1;

    [Header("Raycast Configuration")]
    [SerializeField]
    [Tooltip("Offset from center when raycasting")]
    private Vector3 raycastOffset;
    [SerializeField]
    [Tooltip("The length of raycast to find the floor")]
    private float raycastLength;
    [SerializeField]
    [Tooltip("The offset from the floor")]
    private float floorOffsetY;


    [Header("Debug Status")]
    [SerializeField]
    [Tooltip("Is this character grounded?")]
    private bool isGrounded;
    [SerializeField]
    [Tooltip("Is this character running?")]
    private bool isRunning = false;

    /*
     * Members
     */
    private Vector3 moveDirection; //Move direction of the character
    private float inputAmount;
    private Vector3 gravity;                //Gravity
    private Vector3 combinedRaycast;
    private float currentSpeed;
    //Character IK
    private CharacterIK characterIK;

    /*
     * Components
     */
    private Rigidbody rb;
    private Animator anim;

    /*
     * Properties
     */
    public MovementProfile CurrentProfile { get; set; }

    #region MonoBehaviour
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        characterIK = GetComponent<CharacterIK>();

    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        //Check if the character is running
        isRunning = Input.GetKey(KeyCode.LeftShift) && CurrentProfile.canRun;

        moveDirection = Vector3.zero;
        float vertical = Input.GetAxis("Vertical");
        //Get vertical and horizontal movement inputs
        float horizontal = Input.GetAxis("Horizontal");

        //Base movement on camera
        Vector3 correctedVertical = vertical * CameraCache.Main.transform.forward;
        Vector3 correctedHorizontal = horizontal * CameraCache.Main.transform.right;

        Vector3 combinedInput = correctedVertical + correctedHorizontal;
        //Normalize so diagonal movement isn't twice as fast, clear the y so player doesnt try to walk into the floor/sky
        Vector3 combinedNormalized = combinedInput.normalized;
        moveDirection = new Vector3(combinedNormalized.x, 0, combinedNormalized.z);

        //Make sure input doesn't go negative ro above 1
        float inputMagnitude = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

        inputAmount = isRunning ? Mathf.Clamp01(inputMagnitude) : Mathf.Clamp(inputMagnitude, 0f, 0.5f);
        float resultantRotateSpeed = isRunning ? CurrentProfile.runningRotateSpeed : CurrentProfile.walkingRotateSpeed;

        //Rotate player to movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(moveDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * inputAmount * resultantRotateSpeed);
            transform.rotation = targetRotation;
        }

        //Handle jumping
        if (Input.GetKey(KeyCode.Space) && CurrentProfile.canJump)
        {
            Jump();
        }

        //Handle animation blendtree for walking
        anim.SetFloat("Velocity", inputAmount, 0.2f, animationSpeed * Time.deltaTime);
        anim.SetFloat("SlopeNormal", CurrentProfile.slopeAmount, 0.2f, Time.deltaTime);

  
    }

    /// <summary>
    /// Called by Unity more often than Update
    /// </summary>
    private void FixedUpdate()
    {
        bool groundedThisFrame = IsGrounded();
        //Check whether character is grounded
        if (isGrounded == false && groundedThisFrame)
            anim.SetTrigger("JumpFinish");

        isGrounded = groundedThisFrame;
        //Only enable IK when grounded is true
        characterIK.EnableFeetIK = isGrounded;
        //if not grounded then increase down force over time
        if (!isGrounded)
        {
            gravity += Vector3.up * Physics.gravity.y * CurrentProfile.jumpFallOff * Time.fixedDeltaTime;
            ////if not affected by slope then
            //if (CurrentProfile.affectedBySlope)
            //{
            //    if (CurrentProfile.slopeAmount >= 0.1f)
            //    {
            //        gravity += Vector3.up * Physics.gravity.y * CurrentProfile.jumpFallOff * Time.fixedDeltaTime;
            //    }
            //    else
            //    {
            //        gravity += Vector3.up * Physics.gravity.y * CurrentProfile.jumpFallOff * Time.fixedDeltaTime;
            //    }
            //}
            //else
            //{
            //    gravity += Vector3.up * Physics.gravity.y * CurrentProfile.jumpFallOff * Time.fixedDeltaTime;
            //}
        }
        currentSpeed = isRunning ? CurrentProfile.runningSpeed : CurrentProfile.walkingSpeed;
        //Actual movement of rigidbody + extra down force
        rb.velocity = (moveDirection * GetMoveSpeed() * inputAmount) + gravity;

        //Find the y position via raycasts
        Vector3 floorMovement = new Vector3(rb.position.x, FindFloor().y + floorOffsetY, rb.position.z);

        //Only stick to floor when grounded
        if (IsGrounded() && floorMovement != rb.position && rb.velocity.y <= 0)
        {
            //Move rigidbody to the floor
            rb.MovePosition(floorMovement);
            gravity.y = 0;
        }
    }
    #endregion

    #region Helper Functions

    /// <summary>
    /// Acquires the floor position
    /// </summary>
    /// <returns></returns>
    private Vector3 FindFloor()
    {
        //Width of raycasts around the center of character
        float raycastWidth = 0.25f;

        //Check floor on 5 raycasts, get the average when not Vector3.zero
        int floorAverage = 1;

        combinedRaycast = FloorRaycasts(raycastOffset.x, raycastOffset.y, raycastOffset.z, raycastLength);

        floorAverage += GetFloorAverage(raycastWidth, raycastOffset.y, 0, raycastLength)
                                + GetFloorAverage(-raycastWidth, raycastOffset.y, 0, raycastLength)
                                + GetFloorAverage(0, raycastOffset.y, raycastWidth, raycastLength)
                                + GetFloorAverage(0, raycastOffset.y, -raycastWidth, raycastLength);

        return combinedRaycast / floorAverage;
    }

    /// <summary>
    /// Gets the floor average and adds it accordingly
    /// </summary>
    /// <param name="offsetX">Offset on the x-axis</param>
    /// <param name="offsetY">Offset on the y-axis</param>
    /// <param name="offsetZ">Offset on the z-axis</param>
    /// <param name="raycastLength">Length of raycast</param>
    /// <returns>One if raycast was detected, else returns 0</returns>
    int GetFloorAverage(float offsetX, float offsetY, float offsetZ, float raycastLength)
    {
        Vector3 result = FloorRaycasts(offsetX, offsetY, offsetZ, raycastLength);
        if (result != Vector3.zero)
        {
            combinedRaycast += result;
            return 1;
        }
        return 0;

    }
    public float currentAngle;
    /// <summary>
    /// Function that returns a hit point based on offset if collided.
    /// </summary>
    /// <param name="offsetX">Offset on the x-axis</param>
    /// <param name="offsetY">Offset on the y-axis</param>
    /// <param name="offsetZ">Offset on the z-axis</param>
    /// <param name="raycastLength">Length of raycast</param>
    /// <returns>Hit point if raycast was detected, else returns a zero vector.</returns>
    private Vector3 FloorRaycasts(float offsetX, float offsetY, float offsetZ, float raycastLength)
    {
        RaycastHit hit;
        //Move raycast
        Vector3 raycastFloorPos = transform.TransformPoint(offsetX, offsetY, offsetZ);
        if (Physics.Raycast(raycastFloorPos, -Vector3.up, out hit, raycastLength, walkableLayers))
        {
            CurrentProfile.floorNormal = hit.normal;

            //If this character is affected by slope
            if (CurrentProfile.affectedBySlope)
            {
                //If affected by slope
                currentAngle = Vector3.Angle(hit.normal, Vector3.up);
                //if angle is less
                if (currentAngle < CurrentProfile.slopeLimit)
                {
                    float distance = Vector3.Distance(raycastFloorPos, hit.point);
                    Debug.DrawRay(raycastFloorPos, Vector3.down * distance, Color.magenta);
                    return hit.point;
                }

                return Vector3.zero;
            }

            //Else return hit.point by default.
            return hit.point;
        }
        else
        {
            Debug.DrawRay(raycastFloorPos, Vector3.down * raycastLength, Color.cyan);
        }
        return Vector3.zero;

    }

    float GetMoveSpeed()
    {
        return Mathf.Clamp(currentSpeed + (CurrentProfile.slopeAmount * CurrentProfile.slopeInfluence), 0, currentSpeed + 1);
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            gravity.y = CurrentProfile.jumpPower;
            anim.SetTrigger("Jumping");
        }
    }

    bool IsGrounded()
    {
        if (FloorRaycasts(raycastOffset.x, raycastOffset.y, raycastOffset.z, raycastLength) != Vector3.zero)
        {
            if (CurrentProfile.affectedBySlope)
                CurrentProfile.slopeAmount = Vector3.Dot(transform.forward, CurrentProfile.floorNormal);

            return true;
        }

        return false;
    }
    #endregion
}
