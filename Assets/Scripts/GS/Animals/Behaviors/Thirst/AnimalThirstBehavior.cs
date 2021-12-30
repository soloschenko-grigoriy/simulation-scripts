using System;
using UnityEngine;

namespace GS.Animals.Behaviors.Thirst
{
    public class AnimalThirstBehavior : IAnimalThirstBehavior
    {
        public float Current { get; private set; }
        private readonly IAnimal animal;

        public AnimalThirstBehavior(IAnimal animal)
        {
            this.animal = animal;
            Current = 0f;
        }

        public float Increase(AnimalStatChangeRate rate, float deltaTime)
        {
            Current += rate switch {
                AnimalStatChangeRate.Regular => animal.Characteristics.thirstChangeDefaultRate * deltaTime,
                AnimalStatChangeRate.Medium => animal.Characteristics.thirstChangeDefaultRate * 2 * deltaTime,
                AnimalStatChangeRate.Intense => animal.Characteristics.thirstChangeDefaultRate * 3 * deltaTime,
                _ => throw new ArgumentOutOfRangeException(nameof(rate), rate, null)
            };

            return Current;
        }

        public float Decrease(AnimalStatChangeRate rate, float deltaTime)
        {
            Current -= rate switch {
                AnimalStatChangeRate.Regular => animal.Characteristics.thirstChangeDefaultRate * deltaTime,
                AnimalStatChangeRate.Medium => animal.Characteristics.thirstChangeDefaultRate * 2 * deltaTime,
                AnimalStatChangeRate.Intense => animal.Characteristics.thirstChangeDefaultRate * 3 * deltaTime,
                _ => throw new ArgumentOutOfRangeException(nameof(rate), rate, null)
            };

            if (Current < 0)
            {
                Current = 0;
            }

            return Current;
        }
        
        public void Reset()
        {
            Current = 0f;
        }
    }
}
