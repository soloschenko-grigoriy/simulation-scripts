using GS.Animals.Behaviors.Movement;
using UnityEngine;

namespace GS.Animals.Behaviors.Seek
{
    public class AnimalSeekBehavior : IAnimalSeekBehavior
    {
        public bool InProgress { get; private set; }
        public bool IsCompleted { get; private set; }

        private readonly IAnimal animal;

        public AnimalSeekBehavior(IAnimal animal)
        {
            this.animal = animal;
            
            animal.Movement.OnReachDestination += () => {
                if (!InProgress)
                {
                    return;
                }

                InProgress = false;
                IsCompleted = true;
            };
        }

        public void Begin(Vector3 target)
        {
            animal.Movement.BeginTowards(new Vector3(
                target.x,
                animal.Position.y,
                target.z
            ));
            
            InProgress = true;
            IsCompleted = false;
        }

        public void Cancel()
        {
            animal.Movement.Stop();
            InProgress = false;
            IsCompleted = false;
        }
    }
}
