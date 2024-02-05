using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dynamic entity class that every class should inherit from if it's a moving object
/// </summary>
public class DynamicEntity : Entity
{
    [Header("Movement Profile Configuration")]
    /*
     * Movement Profiles
     */
    [Header("Movement Profiles")]
    [SerializeField]
    [Tooltip("Serializable movement profiles for this dynamic entity")]
    protected MovementProfiles movementProfiles;

    /*
     * Properties
     */
    public MovementProfiles MovementProfiles
    {
        get { return movementProfiles; }
    }

    #region MonoBehaviour
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    protected override void Start()
    {
        base.Start();
        //Set up the movement profiles
        movementProfiles.SetupMap();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }
    #endregion
}
