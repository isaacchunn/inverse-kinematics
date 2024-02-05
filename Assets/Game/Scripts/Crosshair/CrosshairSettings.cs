using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.UI.Crosshair;

/// <summary>
/// Class that exposes all the settings based on the input of a scriptable object
/// Allows runtime changing of scriptable objects and assigning of values
/// </summary>
[System.Serializable]
public class CrosshairSettings
{
    /*
     * Serializable
     */
    [Header("References")]
    [SerializeField]
    [Tooltip("The scriptable object to derive the values from")]
    private CrosshairSetting settings;

    [Header("Settings")]
    [SerializeField]
    [Tooltip("The general settings configuration for the crosshair")]
    private GeneralSettings generalSettings;
    [SerializeField]
    [Tooltip("The outline settings configuration for the crosshair")]
    private OutlineSettings outlineSettings;
    [SerializeField]
    [Tooltip("The dot settings configuration for the crosshair")]
    private DotSettings dotSettings;
    [SerializeField]
    [Tooltip("The inner line settings configuration for the crosshair")]
    private InnerLineSettings innerLineSettings;
    [SerializeField]
    [Tooltip("The outer line settings configuration for the crosshair")]
    private OuterLineSettings outerLineSettings;

    /*
     * Properties
     */
    /// <summary>
    /// Property to acquire the general settings of this crosshair
    /// </summary>
    public GeneralSettings GeneralSettings
    {
        get { return generalSettings; }
    }
    /// <summary>
    /// Property to acquire the outline settings of this crosshair
    /// </summary>
    public OutlineSettings OutlineSettings
    {
        get { return outlineSettings; }
    }
    /// <summary>
    /// Property to acquire the dot settings of this crosshair
    /// </summary>
    public DotSettings DotSettings
    {
        get { return dotSettings; }
    }
    /// <summary>
    /// Property to acquire the inner line settings of this crosshair
    /// </summary>
    public InnerLineSettings InnerLineSettings
    {
        get { return innerLineSettings; }
    }
    /// <summary>
    /// Property to acquire the outer line settings of this crosshair
    /// </summary>
    public OuterLineSettings OuterLineSettings
    {
        get { return outerLineSettings; }
    }

    /// <summary>
    /// Function that copies the values from the applied object into the current settings
    /// </summary>
    public bool ApplySettings()
    {
        if (settings == null)
            return false;

        //Assign it all from the scriptable object.
        generalSettings = settings.GeneralSettings;
        outlineSettings = settings.OutlineSettings;
        dotSettings = settings.DotSettings;
        innerLineSettings = settings.InnerLineSettings;
        outerLineSettings = settings.OuterLineSettings;

        return true;
    }

}
