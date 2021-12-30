namespace GS.Animals.Behaviors.Stamina
{
    public interface IAnimalStaminaBehavior
    {
        float Current { get; }
        float Drain(AnimalStatChangeRate rate, float deltaTime);
        float Replenish(AnimalStatChangeRate rate, float deltaTime);
        void Reset();
    }
}
