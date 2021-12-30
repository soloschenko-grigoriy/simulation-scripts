using System;
using UnityEngine;

namespace GS.Animals.Behaviors.Movement
{
    public interface IAnimalMovementBehavior
    {
        event AnimalMovementBehaviorChangeEvent OnReachDestination;
        
        bool InProgress { get; }
        void BeginTowards(Vector3 goal);
        void Stop();
        void Update();
    }
}
