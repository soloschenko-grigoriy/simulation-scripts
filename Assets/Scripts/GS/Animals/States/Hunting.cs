using UnityEngine;

namespace GS.Animals.States
{
    public class Hunting : State
    {
        public Hunting(IAnimal animal) : base(animal)
        {
        }

        public override void OnEnter()
        {
            animal.Wander.Cancel();
            animal.Seek.Cancel();
            animal.Chase.Cancel();
        }
        
        public override void OnUpdate(float deltaTime)
        {
            animal.Thirst.Increase(AnimalStatChangeRate.Regular, deltaTime);
            animal.Stamina.Drain(AnimalStatChangeRate.Regular, deltaTime);

            if (animal.Chase.InProgress)
            {
                return;
            }

            if (animal.NearbyPray != null)
            {
                animal.Wander.Cancel();
                animal.Chase.Begin(animal.NearbyPray, 0f);
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
            animal.Chase.Cancel();
        }
    }
}
