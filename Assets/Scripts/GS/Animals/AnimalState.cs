using GS.StateMachine;

namespace GS.Animals
{
    public abstract class State : IState
    {
        protected readonly IAnimal animal;

        protected State(IAnimal animal)
        {
            this.animal = animal;
        }

        public virtual Transition[] Transitions { get; set; }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void OnUpdate(float deltaTime) { }
    }
}
