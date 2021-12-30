using GS.Helpers;
using UnityEngine;

namespace GS.Animals.Behaviors.Flee
{
    public class AnimalFleeBehavior: IAnimalFleeBehavior
    {
        private readonly IAnimal animal;
        private IPositionable target;
        private float cutOff;
        private float threshold;
        private float lastTime;

        public AnimalFleeBehavior(IAnimal animal)
        {
            this.animal = animal;
        }

        public bool InProgress { get; private set; }
        public bool GotAway { get; private set; }
        public bool GotCaught { get; private set; }

        public void Begin(IPositionable target, float updateCutOff, float threshold)
        {
            this.target = target;
            cutOff = updateCutOff;
            this.threshold = threshold;

            animal.Movement.BeginTowards(Destination.GetOppositeDirection(animal, target, animal.Characteristics.awareness));

            InProgress = true;
            GotAway = false;
            GotCaught = false;
        }

        public void Update(float time)
        {
            if (!InProgress)
            {
                return;
            }
            
            // Stop if got away from target
            if (animal.NearbyPredator == null)
            {
                Cancel();
                GotAway = true;
                return;
            }
            
            // Stop if gets caught by target
            if ((target.Position - animal.Position).magnitude <= threshold)
            {
                Cancel();
                GotCaught = true;
                return;
            }
            
            if (time < cutOff + lastTime)
            {
                return;
            }
            
            lastTime = time;
            // update destination to get to current position of target
            animal.Movement.Stop();
            animal.Movement.BeginTowards(Destination.GetOppositeDirection(animal, target, animal.Characteristics.awareness));
        }

        public void Cancel()
        {
            animal.Movement.Stop();
            InProgress = false;
            GotAway = false;
            GotCaught = false;
        }
    }
}
