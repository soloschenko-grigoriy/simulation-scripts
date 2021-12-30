namespace GS.Animals.Behaviors.Chase
{
    public interface IAnimalChaseBehavior
    {
        bool InProgress { get; }
        bool IsTargetReached { get; }
        void Begin(IPositionable target, float updateCutOff);
        void Update(float time);
        void Cancel();
    }
}
