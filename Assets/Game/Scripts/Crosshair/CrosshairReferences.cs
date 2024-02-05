using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.UI
{
    /// <summary>
    /// Wrapper class that contains all the rqeuired crosshair references 
    /// </summary>
    [System.Serializable]
    public class CrosshairReferences
    {
        [Header("Crosshair Dot Reference")]
        [SerializeField]
        private CrosshairDot crosshairDot;
        [Header("Crosshair Inner Line Reference")]
        [SerializeField]
        private CrosshairInnerLines crosshairInnerLines;
        [Header("Crosshair Outer Line Reference")]
        [SerializeField]
        private CrosshairOuterLines crosshairOuterLines;

        /*
         * Properties
         */
        /// <summary>
        /// Property for acquiring the dot
        /// </summary>
        public CrosshairDot Dot
        {
            get { return crosshairDot; }
        }
        public CrosshairInnerLines InnerLines
        {
            get { return crosshairInnerLines; }
        }
        public CrosshairOuterLines OuterLines
        {
            get { return crosshairOuterLines; }
        }

        #region Helper Functions
        /// <summary>
        /// Wrapper functions to setup references
        /// </summary>
        public void Setup()
        {
            Dot.Setup();
            InnerLines.Setup();
            OuterLines.Setup();
        }
        #endregion
    }

    /// <summary>
    /// Contains references for the crosshair dot
    /// </summary>
    [System.Serializable]
    public class CrosshairDot
    {
        /*
         * Serializable
         */
        [SerializeField]
        private CrosshairObject centerDot;

        /*
         * Properties
         */
        /// <summary>
        /// Property for acquiring the dot object of this crosshair
        /// </summary>
        public CrosshairObject CenterDot
        {
            get { return centerDot; }
        }

        public void Setup()
        {
            if (centerDot != null)
                centerDot.Setup();
        }
    }
    /// <summary>
    /// Contains references for the crosshair inner line
    /// </summary>
    [System.Serializable]
    public class CrosshairInnerLines
    {
        /*
         * Serializable
         */
        [Header("Configuration")]
        [SerializeField]
        private List<CrosshairPair> crosshairPairs;

        /*
         * Properties 
         */
        /// <summary>
        /// Property for acquiring the list of crosshair pairs
        /// </summary>
        public List<CrosshairPair> CrosshairPairs
        {
            get { return crosshairPairs; }
        }

        #region Helper Functions
        /// <summary>
        /// Sets up the appropriate references in each pair
        /// </summary>
        public void Setup()
        {
            //Setup each pair
            foreach (CrosshairPair pair in crosshairPairs)
            {
                pair.Setup();
            }
        }
        #endregion
    }
    /// <summary>
    /// Contains references for the crosshair outer line
    /// </summary>
    [System.Serializable]
    public class CrosshairOuterLines
    {
        #region Helper Functions
        /// <summary>
        /// Sets up the appropriate references in each pair
        /// </summary>
        public void Setup()
        {
        }
        #endregion
    }

    [System.Serializable]
    public class CrosshairPair
    {
        #region Type Definitions
        /// <summary>
        /// Enum for which side this crosshair is on for lines
        /// </summary>
        public enum Side
        {
            Left,
            Right,
            Up,
            Down
        }
        #endregion
        /*
         * Serializable
         */
        [Header("Crosshair Pair")]
        [SerializeField]
        private Side side;
        [SerializeField]
        private CrosshairObject parent;
        [SerializeField]
        private CrosshairObject line;

        /// <summary>
        /// Helper function that helps set up the crosshair objects references
        /// </summary>
        public void Setup()
        {
            //Setup the parent and line
            parent.Setup();
            line.Setup();
        }
        /// <summary>
        /// Property for acquiring the line object
        /// </summary>
        public CrosshairObject Line
        {
            get { return line; }
            set { line = value; }
        }
        /// <summary>
        /// Property for acquiring the parent object
        /// </summary>
        public CrosshairObject Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        /// <summary>
        /// Property for acquiring the side this crosshair is on
        /// </summary>
        public Side CrosshairSide
        {
            get { return side; }
            set { side = value; }
        }

    }

    /// <summary>
    /// Class that holds references to the main object, and contains references to the rect and image transforms;
    /// </summary>
    [System.Serializable]
    public class CrosshairObject
    {
        /*
         * Serializable
         */
        [SerializeField]
        private GameObject crosshairObject;
        [HideInInspector]
        private RectTransform rectTransform;
        [HideInInspector]
        private Image crosshairImage;

        /*
         * Properties
         */
        /// <summary>
        /// Property for acquiring the object in this crosshair pair
        /// </summary>
        public GameObject Object
        {
            get { return crosshairObject; }
        }
        /// <summary>
        /// Property for acquiring the rect transform for the object  in this crosshair pair
        /// </summary>
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                    rectTransform = crosshairObject.GetComponent<RectTransform>();

                return rectTransform;
            }
        }
        /// <summary>
        /// Property for acquiring the image component for the object  in this crosshair pair
        /// </summary>
        public Image Image
        {
            get
            {
                if (crosshairImage == null)
                    crosshairImage = crosshairObject.GetComponent<Image>();

                return crosshairImage;
            }
        }

        #region Helper Functions
        /// <summary>
        /// Sets up the references of the crosshair objects
        /// </summary>
        public void Setup()
        {
            if (crosshairObject == null)
                return;

            rectTransform = crosshairObject.GetComponent<RectTransform>();
            crosshairImage = crosshairObject.GetComponent<Image>();
        }
        #endregion
    }
}
