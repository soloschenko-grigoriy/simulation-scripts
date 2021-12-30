namespace GS.Animals.States
{
    public class Breeding : State
    {
        public Breeding(IAnimal animal) : base(animal) { }

        public override void OnEnter()
        {
            animal.Breed.Begin();
        }

        public override void OnUpdate(float deltaTime)
        {
            animal.Hunger.Increase(AnimalStatChangeRate.Intense, deltaTime);
            animal.Thirst.Increase(AnimalStatChangeRate.Intense, deltaTime);
            animal.Stamina.Drain(AnimalStatChangeRate.Intense, deltaTime);
        }

        public override void OnExit()
        {
            animal.Breed.Cancel();
        }
    }
}
