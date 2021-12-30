using UnityEngine;

namespace GS.Animals.States
{
    public class Eating : State
    {
        public Eating(IAnimal animal) : base(animal) { }

        public override void OnEnter()
        {
            if (animal.NearbyPray != null)
            {
                animal.NearbyPray.BeingDevoured = true;
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            animal.Hunger.Decrease(AnimalStatChangeRate.Intense, deltaTime);
            animal.Thirst.Decrease(AnimalStatChangeRate.Regular, deltaTime);
            animal.Stamina.Replenish(AnimalStatChangeRate.Medium, deltaTime);
        }
    }
}
