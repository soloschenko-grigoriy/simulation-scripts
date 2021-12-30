using System;
using GS.StateMachine;

namespace GS.Animals
{
    public class Transition : StateTransition
    {
        public Transition(IState target, Func<bool> isTriggered) : base(target, isTriggered)
        {
        }
    }
}
