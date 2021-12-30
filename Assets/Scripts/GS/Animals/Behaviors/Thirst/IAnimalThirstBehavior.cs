namespace GS.Animals.Behaviors.Thirst
{
    public interface IAnimalThirstBehavior
    {
        float Current { get; }
        
        float Increase(AnimalStatChangeRate rate, float deltaTime);
        float Decrease(AnimalStatChangeRate rate, float deltaTime);
        void Reset();
    }
}
