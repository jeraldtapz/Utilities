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

    public class FSM : ILogger
    {
        public string FsmName { get; }

        private readonly Dictionary<int, IState> statesLookupMap;
        private readonly Dictionary<int, List<int>> stateTransitionMap;

        private readonly Subject<string> logObservable;
        public IObservable<string> LogObservable => logObservable;


        public FSM(string fsmName)
        {
            statesLookupMap = new Dictionary<int, IState>();
            stateTransitionMap = new Dictionary<int, List<int>>();
            logObservable = new Subject<string>();
            FsmName = fsmName;
        }

        public void AddState(IState state)
        {
            statesLookupMap.Add(state.Name.GetHashCode(), state);
            logObservable.OnNext($"Successfully added state {state} to FSM {FsmName}");
        }

        public void AddTransition(IState fromState, IState toState)
        {
            int fromStateHash = fromState.Name.GetHashCode();
            int toStateHash = toState.Name.GetHashCode();

            if(!statesLookupMap.ContainsKey(fromStateHash))
                throw new FSMStateNotFoundException(FsmName, fromState.Name);

            if (!statesLookupMap.ContainsKey(toStateHash))
                throw new FSMStateNotFoundException(FsmName, toState.Name);

            List<int> stateTransitions = stateTransitionMap[fromStateHash];

            if (stateTransitions == null)
            {
                stateTransitions = new List<int>();
                stateTransitionMap[fromStateHash] = stateTransitions;
            }

            if (!stateTransitions.Contains(toStateHash))
            {
                stateTransitions.Add(toStateHash);
            }
        }
    }
}