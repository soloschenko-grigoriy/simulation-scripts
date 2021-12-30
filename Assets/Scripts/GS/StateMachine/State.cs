using GS.Animals;

namespace GS.StateMachine
{
    public interface IState
    {
        Transition[] Transitions { get; }

        void OnEnter();
        void OnUpdate(float deltaTime);
        void OnExit();
    }
}
