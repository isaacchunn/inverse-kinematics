using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player class that allows players to 
/// </summary>
public class Player : DynamicEntity
{
    #region Singleton Instance
    public static Player Instance { get; private set; }
    #endregion

    /*
     * Members
     */
    private CharacterMovement characterMovement;

    #region MonoBehaviour
    /// <summary>
    /// Initialization
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        if (Instance != null)
        {
            Debug.LogWarning("There exists more than one object of " + GetType() + " in the scene! Destroying this object.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        //References
        //Get reference to character movement profile
        characterMovement = GetComponent<CharacterMovement>();
    }
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    protected override void Start()
    {
        base.Start();

        //Set the default profile to be ground profile.
        characterMovement.CurrentProfile = movementProfiles.DefaultProfile;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// If something has changed in the editor... (only in editor, to auto update values), will add a way to disable this
    /// </summary>
    private void OnValidate()
    {
        //Update the values
        movementProfiles.SetupMap();
    }
    #endregion
}
