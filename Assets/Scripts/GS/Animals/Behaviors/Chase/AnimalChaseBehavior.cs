using UnityEngine;

namespace GS.Animals.Behaviors.Chase
{
    public class AnimalChaseBehavior : IAnimalChaseBehavior
    {
        private readonly IAnimal animal;
        private IPositionable target;
        private float cutOff;
        private float lastTime;

        public AnimalChaseBehavior(IAnimal animal)
        {
            this.animal = animal;

            animal.Movement.OnReachDestination += () => {
                if (!InProgress)
                {
                    return;
                }

                InProgress = false;
                IsTargetReached = true;
            };
        }

        public bool InProgress { get; private set; }
        public bool IsTargetReached { get; private set; }

        public void Begin(IPositionable t, float updateCutOff)
        {
            target = t;
            cutOff = updateCutOff;

            animal.Movement.BeginTowards(new Vector3(
                target.Position.x,
                animal.Position.y,
                target.Position.z
            ));

            InProgress = true;
            IsTargetReached = false;
        }

        public void Update(float time)
        {
            if (!InProgress)
            {
                return;
            }
            
            if (time < cutOff + lastTime)
            {
                return;
            }

            lastTime = time;
            // update destination to get to current position of target
            animal.Movement.Stop();
            animal.Movement.BeginTowards(new Vector3(
                target.Position.x,
                animal.Position.y,
                target.Position.z
            ));
        }

        public void Cancel()
        {
            animal.Movement.Stop();
            InProgress = false;
            IsTargetReached = false;
        }
    }
}
