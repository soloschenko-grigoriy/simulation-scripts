using System.Collections;
using UnityEngine;

namespace GS.Animals.Behaviors.Breed
{
    public class AnimalBreedBehavior: IAnimalBreedBehavior
    {
        public bool JustBreded { get; private set; }
        
        private readonly IAnimal animal;
        private bool coroutineScheduled = false;
        private float timeOfLastBreeding;

        public AnimalBreedBehavior(IAnimal animal)
        {
            this.animal = animal;
        }

        public void Update(float time)
        {
            if (JustBreded && time >= timeOfLastBreeding + animal.Characteristics.breedingCooldown)
            {
                JustBreded = false;
            }
        }

        public void Begin()
        {
            animal.RunCoroutine(Breed());
            coroutineScheduled = true;
        }

        public void Cancel()
        {
            coroutineScheduled = false;
        }
        
        private IEnumerator Breed()
        {
            yield return new WaitForSeconds(2);
            
            if (!coroutineScheduled)
            {
                yield break;
            }

            if (animal.Sex == AnimalSex.Female)
            {
                GameObject
                    .Instantiate(animal.AnimalPrefab)
                    .SpawnAt(new Vector3(animal.Position.x, 0, animal.Position.z), "Child");
            }
            
            coroutineScheduled = false;
            
            JustBreded = true;
            timeOfLastBreeding = Time.time;
        }
    }
}
