using UnityEngine;

namespace GS.Animals.States
{
    public class LookingForWater : State
    {
        public LookingForWater(IAnimal animal) : base(animal) { }
        
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

            if (animal.NearbyWater != null)
            {
                animal.Wander.Cancel();
                animal.Seek.Begin((Vector3)animal.NearbyWater.transform.position);
                return;
            }

            if (animal.Wander.InProgress)
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
