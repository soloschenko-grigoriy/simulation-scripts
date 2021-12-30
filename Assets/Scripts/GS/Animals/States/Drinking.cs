namespace GS.Animals.States
{
    public class Drinking : State
    {
        public Drinking(IAnimal animal) : base(animal) { }

        public override void OnUpdate(float deltaTime)
        {
            animal.Thirst.Decrease(AnimalStatChangeRate.Intense, deltaTime);
            animal.Hunger.Decrease(AnimalStatChangeRate.Regular, deltaTime);
            animal.Stamina.Replenish(AnimalStatChangeRate.Medium, deltaTime);
        }
    }
}
