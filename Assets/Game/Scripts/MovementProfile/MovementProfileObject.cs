using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;


/// <summary>
/// Scriptable object class that holds the necessary information for altering of values
/// </summary>
[CreateAssetMenu(fileName = "New Movement Profile" , menuName = "Movement Profiles")]
public class MovementProfileObject : ScriptableObject
{
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
}
