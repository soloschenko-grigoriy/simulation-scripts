namespace GS.Animals.Behaviors.Wander
{
    public interface IAnimalWanderBehavior
    {
        bool InProgress { get; }
        bool IsScheduled { get; }

        void Begin();
        void Cancel();
    }
}
