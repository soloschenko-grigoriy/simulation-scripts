using UnityEngine;

namespace GS.Animals.States
{
    public class LookingForSex : State
    {
        public LookingForSex(IAnimal animal) : base(animal) { }
        
        public override void OnUpdate(float deltaTime)
        {
            animal.Hunger.Increase(AnimalStatChangeRate.Regular, deltaTime);
            animal.Thirst.Increase(AnimalStatChangeRate.Regular, deltaTime);
            animal.Stamina.Drain(AnimalStatChangeRate.Regular, deltaTime);

            if (animal.Chase.InProgress)
            {
                return;
            }

            if (animal.NearbyPartner != null)
            {
                animal.Wander.Cancel();
                animal.Chase.Begin(animal.NearbyPartner, 0f);
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
            animal.Chase.Cancel();
        }
    }
}
