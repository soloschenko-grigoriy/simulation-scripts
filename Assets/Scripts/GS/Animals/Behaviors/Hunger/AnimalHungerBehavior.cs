using System;
using UnityEngine;

namespace GS.Animals.Behaviors.Hunger
{
    public class AnimalHungerBehavior : IAnimalHungerBehavior
    {
        private readonly IAnimal animal;
        
        public float Current { get; private set; }

        public AnimalHungerBehavior(IAnimal animal)
        {
            this.animal = animal;
            Current = 0f;
        }

        public void Reset()
        {
            Current = 0f;
        }

        public float Increase(AnimalStatChangeRate rate, float deltaTime)
        {
            Current += rate switch {
                AnimalStatChangeRate.Regular => animal.Characteristics.hungerChangeDefaultRate * deltaTime,
                AnimalStatChangeRate.Medium => animal.Characteristics.hungerChangeDefaultRate * 2 * deltaTime,
                AnimalStatChangeRate.Intense => animal.Characteristics.hungerChangeDefaultRate * 3 * deltaTime,
                _ => throw new ArgumentOutOfRangeException(nameof(rate), rate, null)
            };

            return Current;
        }

        public float Decrease(AnimalStatChangeRate rate, float deltaTime)
        {
            Current -= rate switch {
                AnimalStatChangeRate.Regular => animal.Characteristics.hungerChangeDefaultRate * deltaTime,
                AnimalStatChangeRate.Medium => animal.Characteristics.hungerChangeDefaultRate * 2 * deltaTime,
                AnimalStatChangeRate.Intense => animal.Characteristics.hungerChangeDefaultRate * 3 * deltaTime,
                _ => throw new ArgumentOutOfRangeException(nameof(rate), rate, null)
            };

            if (Current < 0)
            {
                Current = 0;
            }

            return Current;
        }
    }
}
