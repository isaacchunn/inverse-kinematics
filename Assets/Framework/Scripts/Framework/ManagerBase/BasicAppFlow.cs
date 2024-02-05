using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Application
{
    /// <summary>
    /// Describes a basic app flow in which an app would ..
    /// </summary>
    public class BasicAppFlow : MonoBehaviour
    {
        #region Type Definitions
        /// <summary>
        /// Enum for the states of the game.
        /// </summary>
        public enum State
        {
            MainMenu,
            Content,
            All
        }
        #endregion

        /*
         * Statics
         */
        /// <summary>
        /// Singleton Instance
        /// </summary>
        public static BasicAppFlow Instance { get; private set; }

        /*
         * Serializable
         */
        [SerializeField]
        [Tooltip("The first state of this basic app flow.")]
        private State defaultFirstState;
        [SerializeField]
        [Tooltip("List of states for app flow")]
        private List<AppFlowState> appFlowStates;

        /*
         * Properties
         */
        public State DefaultFirstState
        {
            get { return defaultFirstState; }
        }
        public AppFlowState DefaultAppFlowState
        {
            get
            {
                if (defaultAppFlow == null)
                {
                    stateMap.TryGetValue(defaultFirstState, out defaultAppFlow);
                }
                return defaultAppFlow;
            }
        }
        public AppFlowState CurrentAppFlow { get; private set; }

        /*
         * Members
         */
        private Dictionary<State, AppFlowState> stateMap;
        private AppFlowState defaultAppFlow;

        #region MonoBehaviour
        /// <summary>
        /// Initialization of resources
        /// </summary>
        private void Awake()
        {
            Cursor.visible = false;
            //Basic Singleton Initilization
            if (Instance != null)
            {
                Destroy(gameObject);
                Debug.LogWarning("More than one type of " + GetType() + " exists in the scene! Deleting this gameObject");
            }
            else
            {
                Instance = this;
            }

            //Set up the state map
            stateMap = new Dictionary<State, AppFlowState>();
            //Add each  from app flow states into the dictonary
            foreach (AppFlowState state in appFlowStates)
            {
                stateMap.Add(state.LinkedState, state);
            }

        }
        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        void Start()
        {
            //We setup all here to allow other references and awake initialization to occur first.
            SetupAll();

            //Find index of first state and call activate
            //Call first state's dictionary and activate
            AppFlowState firstState;
            stateMap.TryGetValue(defaultFirstState, out firstState);

            if (firstState != null)
                firstState.Activate();

            //Set current state to firstState
            CurrentAppFlow = firstState;
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                ChangeToState(State.MainMenu);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                ChangeToState(State.Content);
        }
        #endregion

        #region Helper Functions
        /// <summary>
        /// Setup and initialize all states
        /// </summary>
        private void SetupAll()
        {
            foreach (AppFlowState state in appFlowStates)
                state.Initialize();
        }

        public void ChangeToState(State newState, bool allowStateChangeIfSame = true)
        {
            //If both states are same
            if (CurrentAppFlow.LinkedState == newState)
            {
                //And change is allowed, we activate and deactivate
                if (allowStateChangeIfSame)
                {
                    //Call deactivate then activate
                    CurrentAppFlow.Deactivate();
                    CurrentAppFlow.Activate();
                }
                return;
            }
            AppFlowState state;
            stateMap.TryGetValue(newState, out state);
            if (state == null)
                return;

            //Else the states are different and we need to change
            CurrentAppFlow.Deactivate();
            CurrentAppFlow = state;
            CurrentAppFlow.Activate();

            //Invoke event of change state if needed.
        }
        #endregion
    }
}