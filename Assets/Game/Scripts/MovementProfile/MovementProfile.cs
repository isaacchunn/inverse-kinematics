using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Movement Profile class that takes it's values from a scriptable object to prevent run time 
/// altering of asset values.
/// </summary>
[System.Serializable]
public class MovementProfile
{
    [Header("References")]
    [SerializeField]
    [Tooltip("The reference to the object that stores these values")]
    private MovementProfileObject movementProfile;
    /*
     * Serializable
     */
    [Header("Profile Type")]
    public MovementProfiles.Profiles profileType;

    [Header("Movement Configuration")]
    [Tooltip("Can this character walk?")]
    public bool canWalk = true;
    [Tooltip("How fast this character walks")]
    public float walkingSpeed = 1;
    [Tooltip("How fast this character rotates to desired input while walking")]
    public float walkingRotateSpeed = 2.5f;
    [Tooltip("Can this character run?")]
    public bool canRun = true;
    [Tooltip("How fast this character runs")]
    public float runningSpeed = 1;
    [Tooltip("How fast this character rotates to desired input while running")]
    public float runningRotateSpeed = 5f;

    [Header("Jumping Configuration")]
    [Tooltip("Can this character jump?")]
    public bool canJump = true;
    [Tooltip("Jump falloff")]
    public float jumpFallOff = 2f;
    [Tooltip("Jump power of this character")]
    public float jumpPower = 10f;

    [Header("Sloping Configuration")]
    [Tooltip("Whether this entity is affected by slopes.")]
    public bool affectedBySlope = false;
    [Tooltip("What is the max slope this character can go up on..")]
    [Range(0f, 180f)]
    public float slopeLimit = 60f;
    [Tooltip("How much the slope influences speed")]
    public float slopeInfluence = 5f;
    [Tooltip("Current slope amount using dot product")]
    public float slopeAmount;

    /*
     * Members
     */
    [HideInInspector]
    public Vector3 floorNormal;    //This must be updated by the developer himself for other calculations to work.

    /// <summary>
    /// Set values from the existing movement profile
    /// </summary>
    public void SetValues()
    {
        if (movementProfile == null)
        {
            Debug.Log("No movement profile set on object, will revert to default values.");
            return;
        }

        //Profile Types
        profileType = movementProfile.profileType;

        //Movement 
        canWalk = movementProfile.canWalk;
        walkingSpeed = movementProfile.walkingSpeed;
        walkingRotateSpeed = movementProfile.walkingRotateSpeed;
        canRun = movementProfile.canRun;
        runningSpeed = movementProfile.runningSpeed;
        runningRotateSpeed = movementProfile.runningRotateSpeed;

        //Jump
        canJump = movementProfile.canJump;
        jumpFallOff = movementProfile.jumpFallOff;
        jumpPower = movementProfile.jumpPower;

        //Slopes
        affectedBySlope = movementProfile.affectedBySlope;
        slopeLimit = movementProfile.slopeLimit;
        slopeInfluence = movementProfile.slopeInfluence;
    }

    /// <summary>
    /// Sets values based on another's profile, to simulate real time changes
    /// </summary>
    /// <param name="movementProfile">Other movement profile object</param>
    public void SetValues(MovementProfileObject movementProfile)
    {
        if (movementProfile == null)
            return;

        this.movementProfile = movementProfile;
        SetValues();
    }
}
