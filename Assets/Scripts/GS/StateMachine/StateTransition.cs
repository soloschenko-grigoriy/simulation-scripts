using System;

namespace GS.StateMachine
{
    public abstract class StateTransition
    {
        public StateTransition(IState target, Func<bool> isTriggered)
        {
            Target = target;
            IsTriggered = isTriggered;
        }

        public Func<bool> IsTriggered { get; }

        public IState Target { get; }
    }
}
