using System;
using System.Collections.Generic;

namespace Core.Common
{
    public class FSMState : IState
    {
        public string Name { get; protected set; }
        public IFSMData Data { get; protected set; }
        private readonly List<Action> onEnterActions;
        private readonly List<Action> onUpdateActions;
        private readonly List<Action> onExitActions;

        public FSMState(string name, IFSMData data)
        {
            Name = name;
            Data = data;
            onEnterActions = new List<Action>(1);
            onUpdateActions =  new List<Action>(1);
            onExitActions = new List<Action>(1);
        }

        public void OnEnter()
        {
            onEnterActions.ForEach(x => x.Invoke());
        }

        public void OnUpdate()
        {
            onUpdateActions.ForEach(x => x.Invoke());
        }

        public void OnExit()
        {
            onExitActions.ForEach(x => x.Invoke());
        }

        public void AddOnEnterAction(Action action)
        {
            onEnterActions.Add(action);
        }

        public void AddOnUpdateAction(Action action)
        {
            onUpdateActions.Add(action);
        }

        public void AddOnExitAction(Action action)
        {
            onExitActions.Add(action);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}