using System;
using UnityEngine;

namespace GS.Animals.Behaviors.Stamina
{
    public class AnimalStaminaBehavior: IAnimalStaminaBehavior
    {
        public float Current { get; private set; }

        private readonly IAnimal animal;
        
        public AnimalStaminaBehavior(IAnimal animal)
        {
            this.animal = animal;
        }

        public float Drain(AnimalStatChangeRate rate, float deltaTime)
        {
            Current -= rate switch {
                AnimalStatChangeRate.Regular => animal.Characteristics.staminaChangeDefaultRate * deltaTime,
                AnimalStatChangeRate.Medium => animal.Characteristics.staminaChangeDefaultRate * 2 * deltaTime,
                AnimalStatChangeRate.Intense => animal.Characteristics.staminaChangeDefaultRate * 3 * deltaTime,
                _ => throw new ArgumentOutOfRangeException(nameof(rate), rate, null)
            };

            if (Current < 0)
            {
                Current = 0;
            }

            return Current;
        }

        public float Replenish(AnimalStatChangeRate rate, float deltaTime)
        {
            Current += rate switch {
                AnimalStatChangeRate.Regular => animal.Characteristics.staminaChangeDefaultRate * deltaTime,
                AnimalStatChangeRate.Medium => animal.Characteristics.staminaChangeDefaultRate * 2 * deltaTime,
                AnimalStatChangeRate.Intense => animal.Characteristics.staminaChangeDefaultRate * 3 * deltaTime,
                _ => throw new ArgumentOutOfRangeException(nameof(rate), rate, null)
            };

            if (Current > 1)
            {
                Current = 1;
            }

            return Current;
        }

        public void Reset()
        {
            Current = 1f;
        }
    }
}
