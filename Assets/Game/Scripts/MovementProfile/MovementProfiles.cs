using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all the movement profiles that this character has
/// </summary>
[System.Serializable]
public class MovementProfiles
{
    #region Type Definitions
    /// <summary>
    /// The enum for the type of movement profiles
    /// </summary>
    public enum Profiles
    {
        Ground
    }

    #endregion
    /*
     * Serializable
     */
    [Header("Movement Profiles")]
    [SerializeField]
    private Profiles defaultProfile;
    [SerializeField]
    private List<MovementProfile> movementProfiles;

    /*
     * Members
     */
    private Dictionary<Profiles, MovementProfile> profileMap;

    /*
     * Properties
     */
    /// <summary>
    /// Property that returns the default profile
    /// </summary>
    public MovementProfile DefaultProfile
    {
        get
        {
            return TryGetProfile(defaultProfile);
        }
    }

    #region Helper Functions
    /// <summary>
    /// Setups the dictionary for the profiles
    /// </summary>
    public void SetupMap()
    {
        if (profileMap == null)
            profileMap = new Dictionary<Profiles, MovementProfile>();

        //Clear the profile map
        profileMap.Clear();

        //add each element from list into profile
        foreach (MovementProfile profile in movementProfiles)
        {
            //Set the values of each profile
            profile.SetValues();
            //Add each profile as a reference
            profileMap.Add(profile.profileType, profile);
        }
    }


    /// <summary>
    /// Attempts to get a said profile based on type
    /// </summary>
    /// <param name="profileType">Type of profile to get</param>
    /// <returns>Movement profile of given type if exists, else return null</returns>
    public MovementProfile TryGetProfile(Profiles profileType)
    {
        MovementProfile profile;
        profileMap.TryGetValue(profileType, out profile);
        return profile;
    }

    #endregion
}