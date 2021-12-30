using UnityEngine;

namespace GS.Animals.States
{
    public class LookingForFood : State
    {
        public LookingForFood(IAnimal animal) : base(animal) { }

        public override void OnEnter()
        {
            animal.Wander.Cancel();
            animal.Seek.Cancel();
            animal.Chase.Cancel();
        }

        public override void OnUpdate(float deltaTime)
        {
            animal.Hunger.Increase(AnimalStatChangeRate.Regular, deltaTime);
            animal.Thirst.Increase(AnimalStatChangeRate.Regular, deltaTime);
            animal.Stamina.Drain(AnimalStatChangeRate.Regular, deltaTime);

            if (animal.Seek.InProgress)
            {
                return;
            }

            if (animal.NearbyFood != null)
            {
                animal.Wander.Cancel();
                animal.Seek.Begin(animal.NearbyFood.transform.position);
                return;
            }

            if (animal.Wander.InProgress || animal.Wander.IsScheduled)
            {
                return;
            }

            animal.Wander.Begin();
        }

        public override void OnExit()
        {
            animal.Wander.Cancel();
            animal.Seek.Cancel();
        }
    }
}
