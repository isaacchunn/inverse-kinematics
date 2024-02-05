using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIK : MonoBehaviour
{
    /*
     * Serializable
     */
    [Header("Feet IK")]
    [SerializeField]
    [Tooltip("Whether feet IK is enabled")]
    private bool enableFeetIK = true;
    [SerializeField]
    [Range(0, 2f)]
    [Tooltip("How high the raycast is from the ground")]
    private float heightFromGroundRaycast = 1.14f;
    [SerializeField]
    [Range(0, 2f)]
    private float rayLength = 1.5f;
    [SerializeField]
    private LayerMask environmentLayer;
    [SerializeField]
    private float pelvisOffset = 0f;
    [SerializeField]
    [Range(0, 1f)]
    private float pelvisUpAndDownSpeed = 0.28f;
    [SerializeField]
    [Range(0, 1f)]
    private float feetToIKPositionSpeed = 0.5f;

    [Header("Advanced Feet IK")]
    public bool useProIKFeature = false;
    public bool showSolverDebug = true;
    public string leftFootAnimVariableName = "leftFoot";
    public string rightFootAnimVariableName = "rightFoot";


    [Header("Head IK")]
    [SerializeField]
    [Tooltip("Whether head Ik is enabled")]
    private bool enableHeadIK = true;
    [SerializeField]
    [Tooltip("What target to look at")]
    private Transform targetOfInterest;
    [SerializeField]
    [Tooltip("Head weight")]
    private float headIKWeight = 2;
    [SerializeField]
    [Tooltip("Body weight")]
    private float headIKBodyWeight = 1;

    [Header("Hand IK")]
    [SerializeField]
    [Tooltip("Whether hand Ik is enabled")]
    private bool enableHandIK = true;
    [SerializeField]
    [Tooltip("The layers the character detects for IK")]
    private LayerMask interactableLayer;
    [SerializeField]
    [Tooltip("Offset for the left shoulder")]
    private Vector3 leftShoulderOffset;
    [SerializeField]
    [Tooltip("Offset for the right shoulder")]
    private Vector3 rightShoulderOffset;
    [SerializeField]
    [Range(0.1f, 2f)]
    [Tooltip("Detection Range for IK")]
    private float handDetectionRange = 1f;
    [SerializeField]
    [Range(0.1f, 2f)]
    [Tooltip("Hand Speed to reach IK")]
    private float handToIKPositionSpeed = 1f;
    [SerializeField]
    [Range(0.1f, 1f)]
    [Tooltip("Raycast Accuracy")]
    private float raycastAccuracy = 0.5f;

    /*
     * Members
     */
    //Feet IK
    private Vector3 rightFootPos, leftFootPos, leftFootIKPosition, rightFootIKPosition;
    private Quaternion leftFootIKRotation, rightFootIKRotation;
    private float lastPelvisPositionY, lastRightFootPositionY, lastLeftFootPositionY;

    //Hand IK
    private Transform leftShoulder, rightShoulder;
    private Vector3 leftHandPos, rightHandPos, leftHandIKPosition, rightHandIKPosition;
    private Quaternion leftHandIKRotation, rightHandIKRotation;
    private float leftHandWeight, rightHandWeight;
    private Vector3 lastRightHandPosition, lastLeftHandPosition;
    /*
     * Components
     */
    private Animator anim;

    /*
     * Properties
     */
    public bool EnableHeadIK
    {
        get { return enableHeadIK; }
        set { enableHeadIK = value; }
    }
    public bool EnableHandIK
    {
        get { return enableHandIK; }
        set { enableHandIK = value; }
    }
    public bool EnableFeetIK
    {
        get { return enableFeetIK; }
        set { enableFeetIK = value; }
    }

    #region MonoBehaviour
    /// <summary>
    ///  Start is called before the first frame update
    /// </summary>
    void Start()
    {
        anim = GetComponent<Animator>();
        leftShoulder = anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        rightShoulder = anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {

    }

    /// <summary>
    /// This is called more frequently than Update
    /// </summary>
    private void FixedUpdate()
    {
        if (anim != null)
        {
            //If IK is enabled, we update the feet targets and find position of each foot
            if (enableFeetIK)
            {
                AdjustFeetTarget(ref leftFootPos, HumanBodyBones.LeftFoot);
                AdjustFeetTarget(ref rightFootPos, HumanBodyBones.RightFoot);

                //Find and raycast to the ground to find position
                FeetPositionSolver(rightFootPos, ref rightFootIKPosition, ref rightFootIKRotation);
                FeetPositionSolver(leftFootPos, ref leftFootIKPosition, ref leftFootIKRotation);
            }

            if (enableHeadIK)
            {
                leftHandPos = anim.GetBoneTransform(HumanBodyBones.LeftHand).position;
                rightHandPos = anim.GetBoneTransform(HumanBodyBones.RightHand).position;

                // find raycast positions
                HandIKSolver(leftShoulder.position, ref leftHandIKPosition, ref leftHandIKRotation, -transform.forward);
                HandIKSolver(rightShoulder.position, ref rightHandIKPosition, ref rightHandIKRotation, -transform.forward);
            }
        }
    }
    #endregion

    private void OnAnimatorIK(int layerIndex)
    {
        if (anim != null)
        {
            if (enableFeetIK)
            {
                MovePelvisHeight();

                //Right foot ik position and rotation
                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);

                if (useProIKFeature)
                {
                    anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, anim.GetFloat(rightFootAnimVariableName));
                    anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, anim.GetFloat(leftFootAnimVariableName));
                }

                //Move both feets to ik point
                MoveFeetToIKPoint(AvatarIKGoal.RightFoot, rightFootIKPosition, rightFootIKRotation, ref lastRightFootPositionY);
                MoveFeetToIKPoint(AvatarIKGoal.LeftFoot, leftFootIKPosition, leftFootIKRotation, ref lastLeftFootPositionY);

            }

            if (enableHeadIK)
            {
                HeadRotationSolver();
            }

            if (enableHandIK)
            {
                // distance between hands and raycast hit
                float distanceRightArmObject = Vector3.Distance(anim.GetBoneTransform(HumanBodyBones.RightShoulder).position, rightHandIKPosition);
                float distanceLeftArmObject = Vector3.Distance(anim.GetBoneTransform(HumanBodyBones.LeftShoulder).position, leftHandIKPosition);

                //Debug.Log(distanceLeftArmObject + "/" + distanceRightArmObject);
                // blend weight based on the distance
                leftHandWeight = Mathf.Clamp01(1 - distanceLeftArmObject);
                rightHandWeight = Mathf.Clamp01(1 - distanceRightArmObject);

                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);

                //MoveHandToIKPoint(AvatarIKGoal.LeftHand, leftHandIKPosition, leftHandIKRotation, ref lastLeftHandPosition);
                //MoveHandToIKPoint(AvatarIKGoal.RightHand, rightHandIKPosition, rightHandIKRotation, ref lastRightHandPosition);
                // set the position of the hand
                anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKPosition);
                anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKPosition);

                // set the rotation of the hand
                anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKRotation);
                anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandIKRotation);
            }
        }
    }

    #region Helper Functions

    #region Feet IK
    void MoveFeetToIKPoint(AvatarIKGoal foot, Vector3 positionIKHolder, Quaternion rotationIKHolder, ref float lastFootPositionY)
    {
        Vector3 targetIKPosition = anim.GetIKPosition(foot);
        if (positionIKHolder != Vector3.zero)
        {
            targetIKPosition = transform.InverseTransformPoint(targetIKPosition);
            positionIKHolder = transform.InverseTransformPoint(positionIKHolder);

            float yVariable = Mathf.Lerp(lastFootPositionY, positionIKHolder.y, feetToIKPositionSpeed);
            targetIKPosition.y += yVariable;

            lastFootPositionY = yVariable;

            targetIKPosition = transform.TransformPoint(targetIKPosition);
            anim.SetIKRotation(foot, rotationIKHolder);
        }

        anim.SetIKPosition(foot, targetIKPosition);
    }

    /// <summary>
    /// Adjusts the pelvis height
    /// </summary>
    private void MovePelvisHeight()
    {
        if (rightFootIKPosition == Vector3.zero || leftFootIKPosition == Vector3.zero || lastPelvisPositionY == 0)
        {
            lastPelvisPositionY = anim.bodyPosition.y;
            return;
        }

        float leftOffsetPosition = leftFootIKPosition.y - transform.position.y;
        float rightOffsetPosition = rightFootIKPosition.y - transform.position.y;

        float totalOffset = leftOffsetPosition < rightOffsetPosition ? leftOffsetPosition : rightOffsetPosition;

        Vector3 newPelvisPosition = anim.bodyPosition + Vector3.up * totalOffset;

        newPelvisPosition.y = Mathf.Lerp(lastPelvisPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed);
        anim.bodyPosition = newPelvisPosition;
        lastPelvisPositionY = anim.bodyPosition.y;
    }

    /// <summary>
    /// Locates the feet position using raycasts and then solving for the feet position
    /// </summary>
    /// <param name="fromSkyPosition">From sky position</param>
    /// <param name="feetIKPosition">Reference feet IK position</param>
    /// <param name="feetIKRotation">Reference feet IK rotation</param>
    private void FeetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIKPosition, ref Quaternion feetIKRotation)
    {
        //Raycast handling section
        RaycastHit hit;

        if (Physics.Raycast(fromSkyPosition, Vector3.down, out hit, rayLength + heightFromGroundRaycast, environmentLayer))
        {
            if (showSolverDebug)
                Debug.DrawLine(fromSkyPosition, hit.point, Color.red);

            //Finding out feet ik positions from sky
            feetIKPosition = fromSkyPosition;
            feetIKPosition.y = hit.point.y + pelvisOffset;
            feetIKRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;
            return;
        }

        feetIKPosition = Vector3.zero;
    }

    private void AdjustFeetTarget(ref Vector3 feetPosition, HumanBodyBones foot)
    {
        feetPosition = anim.GetBoneTransform(foot).position;
        feetPosition.y = transform.position.y + heightFromGroundRaycast;
    }
    #endregion

    #region Head IK
    /// <summary>
    /// Solves the head position based on a target item to look at
    /// </summary>
    public void HeadRotationSolver()
    {
        if (targetOfInterest != null)
        {
            // distance between face and object to look at
            float distanceFaceObject = Vector3.Distance(anim.GetBoneTransform(HumanBodyBones.Head).position, targetOfInterest.position);
            anim.SetLookAtPosition(targetOfInterest.position);
            // blend based on the distance
            anim.SetLookAtWeight(Mathf.Clamp01(headIKWeight - distanceFaceObject), Mathf.Clamp01(headIKBodyWeight - distanceFaceObject));
        }
    }
    #endregion


    #region Hand IK
    public void HandIKSolver(Vector3 position, ref Vector3 targetPosition, ref Quaternion targetRotation, Vector3 direction)
    {
        RaycastHit hit;
        // raycast in the given direction
        if (Physics.Raycast(position, -direction, out hit, handDetectionRange, interactableLayer))
        {
            // the hit point is the position of the hand/foot
            targetPosition = Vector3.Lerp(position, hit.point, raycastAccuracy);
            // then rotate based on the hit normal
            Quaternion rot = Quaternion.LookRotation(transform.forward);
            targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * rot;
            //targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;
            Debug.DrawLine(position, targetPosition, Color.red);
        }
        else
        {
            Debug.DrawRay(position, -direction * handDetectionRange, Color.green);
            targetPosition = Vector3.zero;
        }
    }

    void MoveHandToIKPoint(AvatarIKGoal hand, Vector3 positionIKHolder, Quaternion rotationIKHolder, ref Vector3 lastPosition)
    {
        //Vector3 targetIKPosition = anim.GetIKPosition(hand);
        //if (positionIKHolder != Vector3.zero)
        //{
        //    targetIKPosition = transform.InverseTransformPoint(targetIKPosition);
        //    positionIKHolder = transform.InverseTransformPoint(positionIKHolder);

        //    Vector3 resultVector = Vector3.Lerp(lastPosition, positionIKHolder, handToIKPositionSpeed);
        //    targetIKPosition += resultVector;
        //    lastPosition = resultVector;

        //    targetIKPosition = transform.TransformPoint(targetIKPosition);
        //    anim.SetIKRotation(hand, rotationIKHolder);
        //}

        //anim.SetIKPosition(hand, targetIKPosition);
    }
    #endregion
    #endregion


}
