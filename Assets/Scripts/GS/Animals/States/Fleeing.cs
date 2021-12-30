namespace GS.Animals.States
{
    public class Fleeing : State
    {
        public Fleeing(IAnimal animal) : base(animal)
        {
        }

        public override void OnEnter()
        {
            animal.Flee.Begin(animal.NearbyPredator, 0f, 2f);
        }

        public override void OnUpdate(float deltaTime)
        {
            animal.Stamina.Drain(AnimalStatChangeRate.Regular, deltaTime);
        }
        
        public override void OnExit()
        {
            animal.Flee.Cancel();
        }
    }
}
