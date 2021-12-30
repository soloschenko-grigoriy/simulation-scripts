namespace GS.Animals.Behaviors.Breed
{
    public interface IAnimalBreedBehavior
    {
        bool JustBreded { get; }

        void Update(float time);
        void Begin();
        void Cancel();
    }
}
