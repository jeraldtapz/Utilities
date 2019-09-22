using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common;

namespace Console
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            FSM fsm = new FSM("The Ultimate FSM");

            FSMState idleState = new FSMState("Idle", null, null, null);
            idleState.AddOnEnterAction(() => System.Console.WriteLine("On Enter Idle"));
            idleState.AddOnExitAction(() => System.Console.WriteLine("Exiting Idle"));

            FSMState runState = new FSMState("Run", null, null, null);
            runState.AddOnEnterAction(() => System.Console.WriteLine("On Enter Run"));

            fsm.AddState(idleState);
            fsm.AddState(runState);

            fsm.AddTransition(idleState, runState);
        }
    }
}
