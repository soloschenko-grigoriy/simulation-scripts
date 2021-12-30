using System.Collections;
using UnityEngine;

namespace GS.Animals.Behaviors.Wander
{
    public class AnimalWanderBehavior : IAnimalWanderBehavior
    {
        private readonly IAnimal animal;
        public bool IsScheduled { get; private set; }

        public bool InProgress { get; private set; }

        public AnimalWanderBehavior(IAnimal animal)
        {
            this.animal = animal;
            
            animal.Movement.OnReachDestination += () => {
                InProgress = false;
            };
        }

        public void Begin()
        {
            IsScheduled = true;
            animal.RunCoroutine(Wander());
        }

        public void Cancel()
        {
            animal.Movement.Stop();
            IsScheduled = false;
            InProgress = false;
        }
        
        private IEnumerator Wander()
        {
            yield return new WaitForSeconds(Random.Range(
                animal.Characteristics.wanderWaitMinTime,
                animal.Characteristics.wanderWaitMaxTime
            ));

            if (!IsScheduled)
            {
                yield break;
            }

            IsScheduled = false;
            InProgress = true;
            
            animal.Movement.BeginTowards(goal:GetRandomPosition());
        }

        private Vector3 GetRandomPosition()
        {
            int radius = Random.Range(animal.Characteristics.wanderInnerCircle, animal.Characteristics.wanderOuterCircle);

            float angle = Random.Range(0.1f, 1f) * Mathf.PI * 2;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            return animal.Position + new Vector3(x, animal.Position.y, z);
        }
    }
}
