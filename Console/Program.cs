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
            runState.AddOnEnterAction(() => System.Console.WriteLine("OnEnter Run"));

            fsm.AddState(idleState, true);
            fsm.AddState(runState);

            fsm.AddTransition(idleState, runState, "IdleToRun");

            fsm.RunFsm();
            string input = string.Empty;
            while (input != "quit")
            {
                input = System.Console.ReadLine();

                if (input == "transition")
                {
                    fsm.RaiseTransition("IdleToRun");
                }

                System.Console.WriteLine($"Current State is {fsm.CurrentState}");
            }
        }
    }
}