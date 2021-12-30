namespace GS.Animals.Behaviors.Flee
{
    public interface IAnimalFleeBehavior
    {
        bool InProgress { get; }
        bool GotCaught { get; }
        bool GotAway { get; }
        void Begin(IPositionable target, float updateCutOff, float threshold);
        void Update(float time);
        void Cancel();
    }
}
