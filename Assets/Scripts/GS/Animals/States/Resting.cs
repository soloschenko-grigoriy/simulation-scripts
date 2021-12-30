namespace GS.Animals.States
{
    public class Resting : State
    {
        public Resting(IAnimal animal) : base(animal) { }

        public override void OnEnter() {
            animal.Wander.Begin();
            animal.Seek.Cancel();
            animal.Chase.Cancel();
        }

        public override void OnUpdate(float deltaTime)
        {
            animal.Hunger.Increase(AnimalStatChangeRate.Regular, deltaTime);
            animal.Thirst.Increase(AnimalStatChangeRate.Regular, deltaTime);
            animal.Stamina.Replenish(AnimalStatChangeRate.Intense, deltaTime);
        }

        public override void OnExit() => animal.Wander.Cancel();
    }
}
