using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using Framework.UI.Crosshair;

/// <summary>
/// Controller that changes the look  and visuals of the crosshair
/// Provides functionalities to change the look of crosshair during runtime as well
/// </summary>
public class CrosshairController : MonoBehaviour
{
    /*
     * Serializable
     */
    [Header("Crosshair Settings Configuration")]
    [Tooltip("Crosshair Settings Configuration")]
    [SerializeField]
    private CrosshairSettings crosshairSettings;

    [Header("References")]
    [SerializeField]
    private CrosshairReferences crosshairReferences;
    [SerializeField]
    private GameObject parent;

    #region MonoBehaviour
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        //Setup all the references
        crosshairReferences.Setup();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {

    }
    /// <summary>
    /// Calls this function whenever spomething is changed in the editor
    /// </summary>
    private void OnValidate()
    {
        //Setup all the references
        ApplySettings();
    }
    #endregion

    #region Helper Functions
    /// <summary>
    /// Handles the appearance and visual looks of the crosshair
    /// </summary>
    public void HandleDot()
    {
        if (crosshairReferences.Dot.CenterDot.Object == null)
            return;

        //Get the settings objects
        DotSettings settings = crosshairSettings.DotSettings;
        GeneralSettings generalSettings = crosshairSettings.GeneralSettings;

        //Set active the centerDot
        crosshairReferences.Dot.CenterDot.Object.SetActive(settings.centerDotEnabled);
        //Change the size delta on the screen based on the units on the screen
        crosshairReferences.Dot.CenterDot.RectTransform.localScale = Vector3.one * settings.centerDotThickness;
        //Change the color
        Color color = crosshairReferences.Dot.CenterDot.Image.color;
        color = generalSettings.useUniversalColor ? generalSettings.universalColor : settings.centerDotColor;
        color.a = settings.centerDotOpacity;
        crosshairReferences.Dot.CenterDot.Image.color = color;
    }
    /// <summary>
    /// Handles the appearance and visual looks of the inner lines
    /// </summary>
    public void HandleInnerLines()
    {
        //Get the settings objects
        InnerLineSettings settings = crosshairSettings.InnerLineSettings;
        GeneralSettings generalSettings = crosshairSettings.GeneralSettings;

        foreach (CrosshairPair pair in crosshairReferences.InnerLines.CrosshairPairs)
        {
            pair.Parent.Object.SetActive(settings.innerLineEnabled);
            //Set the units of the lines
            // pair.Parent.RectTransform.localScale = Vector3.one * settings.innerLineUnits;
            //Change the color
            Color color = pair.Line.Image.color;
            color = generalSettings.useUniversalColor ? generalSettings.universalColor : settings.innerLineColor;
            color.a = settings.innerLineOpacity;
            pair.Line.Image.color = color;

            //Positions
            Vector3 parentPosition = pair.Parent.RectTransform.anchoredPosition;
            Vector3 linePosition = pair.Line.RectTransform.anchoredPosition;
            //Offsets
            Vector3 lineOffset = ((pair.Line.RectTransform.localScale) * 0.5f);
            //Give a different offset if align with center dot
            Vector3 parentOffset = settings.alignWithCenterDot ? (crosshairReferences.Dot.CenterDot.RectTransform.localScale * 0.5f) + (Vector3.one * settings.innerLineOffset) : Vector3.one * settings.innerLineOffset;
            //Scale
            Vector3 parentScale = pair.Parent.RectTransform.localScale;
            //Set positions
            switch (pair.CrosshairSide)
            {
                case CrosshairPair.Side.Left:
                    //Parent Positioning
                    parentPosition.x = -parentOffset.x;
                    parentPosition.y = 0;
                    //Inner Line
                    linePosition.x = -lineOffset.x;
                    linePosition.y = 0;
                    //Scale
                    parentScale.x = settings.innerLineLength;
                    parentScale.y = settings.innerLineThickness;
                    break;
                case CrosshairPair.Side.Right:
                    //Parent Positioning
                    parentPosition.x = parentOffset.x;
                    parentPosition.y = 0;
                    //Inner Line Positioning
                    linePosition.x = lineOffset.x;
                    linePosition.y = 0;
                    //Scale
                    parentScale.x = settings.innerLineLength;
                    parentScale.y = settings.innerLineThickness;
                    break;
                case CrosshairPair.Side.Up:
                    //Parent Positioning
                    parentPosition.x = 0;
                    parentPosition.y = parentOffset.y;
                    //Inner Line Positioning
                    linePosition.x = 0;
                    linePosition.y = lineOffset.y;
                    //Scale
                    parentScale.x = settings.innerLineThickness;
                    parentScale.y = settings.innerLineLength;
                    break;
                case CrosshairPair.Side.Down:
                    //Parent Positioning
                    parentPosition.x = 0;
                    parentPosition.y = -parentOffset.y;
                    //Inner Line Positioning
                    linePosition.x = 0;
                    linePosition.y = -lineOffset.y;
                    //Scale
                    parentScale.x = settings.innerLineThickness;
                    parentScale.y = settings.innerLineLength;
                    break;
                default:
                    break;
            }
            //Positionings
            pair.Parent.RectTransform.anchoredPosition = parentPosition;
            pair.Line.RectTransform.anchoredPosition = linePosition;
            //Scale
            pair.Parent.RectTransform.localScale = parentScale;
        }
    }

    /// <summary>
    /// Wrapper function for applying all settings
    /// Should be used as a one time use and not in update!
    /// </summary>
    [EasyButtons.Button]
    public void ApplySettings()
    {
        //Apply settings from the scriptable and then alter the variables
        crosshairSettings.ApplySettings();

        //Apply scale to overall crosshair
        parent.transform.localScale = Vector3.one * crosshairSettings.GeneralSettings.sizeMultiplier;
        //Handle the visuals of dot and innerlines
        HandleDot();
        HandleInnerLines();
    }
    #endregion
}
