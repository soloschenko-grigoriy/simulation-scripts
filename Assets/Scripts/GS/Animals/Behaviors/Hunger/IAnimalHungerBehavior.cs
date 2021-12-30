namespace GS.Animals.Behaviors.Hunger
{
    public interface IAnimalHungerBehavior
    {
        float Current { get; }
        
        float Increase(AnimalStatChangeRate rate, float deltaTime);
        float Decrease(AnimalStatChangeRate rate, float deltaTime);
        void Reset();
    }
}
