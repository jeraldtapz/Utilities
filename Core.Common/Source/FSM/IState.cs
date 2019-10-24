namespace Core.Common
{
    public interface IState
    {
        string Name { get; }

        IFSMData Data { get; }

        void OnEnter();

        void OnUpdate();

        void OnExit();
    }
}