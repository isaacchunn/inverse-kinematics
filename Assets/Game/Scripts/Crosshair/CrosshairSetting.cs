using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework.UI.Crosshair
{
    /// <summary>
    /// Contains variables and functions for the crosshair controller to base the crosshair on.
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Crosshair Setting", menuName = "Crosshair")]
    public class CrosshairSetting : ScriptableObject
    {
        /*
         * Serializable
         */
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
    }
    /// <summary>
    /// Settings class for the general settings of the crosshair to serve as a passthrough to the main controller later on
    /// </summary>
    [System.Serializable]
    public class GeneralSettings
    {
        [Header("Color")]
        [Tooltip("Whether this color is used in all categories of crosshair")]
        public bool useUniversalColor = false;
        [Tooltip("Universal Color of all crosshairs")]
        [ColorUsage(true, true)]
        public Color universalColor = Color.green;


        [Header("Size")]
        [Range(0, 10f)]
        [Tooltip("How much larger to increase the crosshair by in scale?")]
        public float sizeMultiplier = 1;
    }

    /// <summary>
    /// Settings class for the dot settings to serve as a passthrough to the main controller later on
    /// </summary>
    [System.Serializable]
    public class DotSettings
    {
        [Header("Center Dot")]
        [Tooltip("Is the center dot enabled?")]
        public bool centerDotEnabled = true;
        [SerializeField]
        [ColorUsage(true, true)]
        public Color centerDotColor = Color.red;
        [Range(0, 1f)]
        [Tooltip("How visible the center dot is")]
        public float centerDotOpacity = 1;
        [Tooltip("How thick the center dot is")]
        [Range(0, 6f)]
        public float centerDotThickness = 1f;
    }

    /// <summary>
    /// Settings class for the outlines to serve as a passthrough to the main controller later on
    /// </summary>
    [System.Serializable]
    public class OutlineSettings
    {
        [Header("Outline")]
        [Tooltip("Whether outlines are enabled on the crosshair")]
        public bool enableOutline = false;
        [SerializeField]
        [ColorUsage(true, true)]
        public Color outlineColor = Color.black;
        [Tooltip("How visible the outline is")]
        [Range(0, 1f)]
        public float outlineOpacity;
        [Tooltip("How thick the outline is")]
        public float outlineThickness;
    }

    /// <summary>
    /// Settings class for the inner lines to serve as a passthrough to the main controller later on
    /// </summary>
    [System.Serializable]
    public class InnerLineSettings
    {
        [Header("Inner Line")]
        [Tooltip("Are the inner lines enabled?")]
        public bool innerLineEnabled = true;
        [Tooltip("Does the inner line align with the center dot?")]
        public bool alignWithCenterDot = false;
        [SerializeField]
        [ColorUsage(true, true)]
        public Color innerLineColor = Color.green;
        [Tooltip("How visible the inner lines is")]
        [Range(0, 1f)]
        public float innerLineOpacity = 1f;
        [Tooltip("How much the inner lines extends its length")]
        [Range(0, 10f)]
        public float innerLineLength = 1f;
        [Tooltip("How thick the inner lines is")]
        [Range(0, 6f)]
        public float innerLineThickness = 1f;
        [Tooltip("The offset from the middle of the screen")]
        [Range(0, 10f)]
        public float innerLineOffset = 1f;
    }

    /// <summary>
    /// Settings class for the outer lines to serve as a passthrough to the main controller later on
    /// </summary>
    [System.Serializable]
    public class OuterLineSettings
    {
        //[Header("Outer Line")]
        //public bool outerLineEnabled = true;
        //public float outerLineOpacity;
        //public float outerLineLength;
        //public float outerLineThickness;
        //public float outerLineOffset;
        //[SerializeField]
        //[ColorUsage(true, true)]
        //public Color outerLineColor = Color.blue;
    }
}