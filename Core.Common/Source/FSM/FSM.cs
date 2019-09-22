using System;
using System.Collections.Generic;

namespace Core.Common
{
    public class FSMStateNotFoundException : Exception
    {
        public FSMStateNotFoundException(string fsmName, string stateName)
            : base($"FSM with name {fsmName} doesn't contain a state with a name {stateName}.")
        {
        }
    }

    public class FSMNotInitializedException : Exception
    {
        public FSMNotInitializedException(string message) : base(message)
        {
        }
    }

    public class FSMTransitionNotFoundException : Exception
    {
        public FSMTransitionNotFoundException(string message) : base(message)
        {
        }
    }

    public class FSM : ILogger
    {
        #region Public Properties

        public string FsmName { get; }
        public IState CurrentState { get; protected set; }
        public bool IsRunning { get; protected set; }

        #endregion Public Properties

        #region Private Fields

        private readonly Dictionary<int, IState> statesLookupMap;

        /// <summary>
        /// A mapping between a state and it's transitions
        /// Each transition is a mapping between the name of the transition and the state to transition to
        /// </summary>
        private readonly Dictionary<int, Dictionary<int, int>> stateTransitionMap;

        #endregion Private Fields

        #region ILogger

        private readonly Subject<string> logObservable;
        public IObservable<string> LogObservable => logObservable;

        #endregion ILogger

        #region Constructor

        public FSM(string fsmName)
        {
            statesLookupMap = new Dictionary<int, IState>();
            stateTransitionMap = new Dictionary<int, Dictionary<int, int>>();
            logObservable = new Subject<string>();
            FsmName = fsmName;
        }

        #endregion Constructor

        #region Public Methods

        public void RunFsm()
        {
            if (CurrentState == null)
                throw new FSMNotInitializedException("The FSM doesn't have an initial state");

            CurrentState.OnEnter();
        }

        public void AddState(IState state, bool isInitialState = false)
        {
            statesLookupMap.Add(state.Name.GetHashCode(), state);
            logObservable.OnNext($"Successfully added state {state} to FSM {FsmName}");

            if (isInitialState)
                CurrentState = state;
        }

        public void AddTransition(IState fromState, IState toState, string transitionName)
        {
            ThrowIfStatesDoesNotExist(fromState.Name, toState.Name);

            int fromStateHash = fromState.Name.GetHashCode();
            int toStateHash = toState.Name.GetHashCode();

            if (!stateTransitionMap.ContainsKey(fromStateHash))
            {
                stateTransitionMap[fromStateHash] = new Dictionary<int, int>();
            }

            if (!stateTransitionMap[fromStateHash].ContainsValue(toStateHash))
            {
                stateTransitionMap[fromStateHash].Add(transitionName.GetHashCode(), toStateHash);
            }
        }

        public void RaiseTransition(string transitionName)
        {
            int transitionNameHash = transitionName.GetHashCode();
            int currentStateHash = CurrentState.Name.GetHashCode();

            if (stateTransitionMap[currentStateHash].ContainsKey(transitionNameHash))
            {
                TransitionTo(stateTransitionMap[currentStateHash][transitionNameHash]);
            }
            else
            {
                throw new FSMTransitionNotFoundException($"Transition from current state {CurrentState.Name} with transition name {transitionName} not found");
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void TransitionTo(int stateHashCode)
        {
            CurrentState.OnExit();

            CurrentState = statesLookupMap[stateHashCode];

            CurrentState.OnEnter();
        }

        private void ThrowIfStatesDoesNotExist(string fromState, string toState)
        {
            int fromStateHash = fromState.GetHashCode();
            int toStateHash = toState.GetHashCode();

            if (!statesLookupMap.ContainsKey(fromStateHash))
                throw new FSMStateNotFoundException(FsmName, fromState);

            if (!statesLookupMap.ContainsKey(toStateHash))
                throw new FSMStateNotFoundException(FsmName, toState);
        }

        #endregion Private Methods
    }
}