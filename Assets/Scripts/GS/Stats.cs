using System;
using System.Collections.Generic;
using GS.Animals;
using GS.Animals.States;

namespace GS
{
    public class Stats
    {
        public int adults;
        public int breeding;
        public int children;
        public int drinking;
        public int eating;
        public int elderly;
        public int females;
        public int hunting;
        public int infants;
        public int lookingForFood;
        public int lookingForSex;
        public int lookingForWater;
        public int males;
        public int resting;
        public int running;
        public int teen;
        public int total;
        public int underAttack;
        public int youngAdults;

        public void Reset()
        {
            total = 0;
            infants = 0;
            children = 0;
            teen = 0;
            youngAdults = 0;
            adults = 0;
            elderly = 0;
            resting = 0;
            lookingForFood = 0;
            hunting = 0;
            eating = 0;
            lookingForWater = 0;
            drinking = 0;
            lookingForSex = 0;
            breeding = 0;
            running = 0;
            underAttack = 0;
            males = 0;
            females = 0;
        }

        public void Recount(List<IAnimal> animals)
        {
            Reset();
            animals.ForEach(Recount);
        }

        private void Recount(IAnimal animal)
        {
            total += 1;
            var state = animal.StateMachine.CurrentState;
            if (state is BeingAttacked)
            {
                underAttack += 1;
            }
            else if (state is Breeding)
            {
                breeding += 1;
            }
            else if (state is Drinking)
            {
                drinking += 1;
            }
            else if (state is Eating)
            {
                eating += 1;
            }
            else if (state is LookingForFood)
            {
                lookingForFood += 1;
            }
            else if (state is Hunting)
            {
                hunting += 1;
            }
            else if (state is LookingForSex)
            {
                lookingForSex += 1;
            }
            else if (state is LookingForWater)
            {
                lookingForWater += 1;
            }
            else if (state is Resting)
            {
                resting += 1;
            }
            else if (state is Fleeing)
            {
                running += 1;
            }

            switch (animal.Age.Current)
            {
                case AnimalAge.Infant:
                    infants += 1;
                    break;
                case AnimalAge.Child:
                    children += 1;
                    break;
                case AnimalAge.Teen:
                    teen += 1;
                    break;
                case AnimalAge.YoungAdult:
                    youngAdults += 1;
                    break;
                case AnimalAge.Adult:
                    adults += 1;
                    break;
                case AnimalAge.Elderly:
                    elderly += 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (animal.Sex == AnimalSex.Male)
            {
                males++;
            }
            else
            {
                females++;
            }
        }
    }
}
