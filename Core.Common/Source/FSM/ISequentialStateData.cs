using System;

namespace Core.Common
{
    public interface ISequentialStateData : IFSMData
    {
        Action<object> GoToPreviousState { get; }
        Action<object> GoToNextState { get; }
    }
}