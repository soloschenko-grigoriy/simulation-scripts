namespace GS.Animals.Behaviors.Age
{
    public interface IAnimalAgeBehavior
    {
        AnimalAge Current { get; }
        
        void Increase(float deltaTime);
        void Reset();
    }
}
