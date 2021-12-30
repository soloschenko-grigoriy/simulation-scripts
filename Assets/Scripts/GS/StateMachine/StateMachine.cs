namespace GS.StateMachine
{
    public abstract class StateMachina
    {
        protected IState[] states;
        public IState CurrentState { get; private set; }

        protected void Start(IState currentState)
        {
            this.CurrentState = currentState;
            this.CurrentState.OnEnter();
        }

        public void Update(float deltaTime)
        {
            foreach (var t in CurrentState.Transitions)
            {
                if (!t.IsTriggered())
                {
                    continue;
                }

                CurrentState.OnExit();
                CurrentState = t.Target;
                CurrentState.OnEnter();

                break;
            }

            CurrentState.OnUpdate(deltaTime);
        }
    }
}
