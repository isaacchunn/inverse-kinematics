using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Application
{
    /// <summary>
    /// Class that allows a state to be initialized and customised at will
    /// </summary>
    [System.Serializable]
    public class AppFlowState
    {
        /*
         * Serializable
         */
        [SerializeField]
        [Tooltip("Name of this state")]
        protected string name;
        [SerializeField]
        [Tooltip("Linked state of this app")]
        private BasicAppFlow.State linkedState;
        [SerializeField]
        [Tooltip("Content that this state is linked to.")]
        private GameObject content;
        /*
         * Properties
         */
        public BasicAppFlow.State LinkedState
        {
            get { return linkedState; }
            set { linkedState = value; }
        }


        #region Abstract Functions
        /// <summary>
        /// Function that is called when the game starts to initialize this state.
        /// </summary>
        public virtual void Initialize()
        {
            content.SetActive(false);
        }
        /// <summary>
        /// Function that is called when this state is being activated
        /// </summary>
        public virtual void Activate()
        {
            content.SetActive(true);
        }
        /// <summary>
        /// Function that is called when this state is being deactivated
        /// </summary>
        public virtual void Deactivate()
        {
            content.SetActive(false);
        }
        /// <summary>
        /// Function that is called when the game has come to an end.
        /// </summary>
        public virtual void Finish()
        {

        }
        #endregion
    }
}
