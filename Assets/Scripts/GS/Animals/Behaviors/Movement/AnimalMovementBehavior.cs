using System;
using UnityEngine;

namespace GS.Animals.Behaviors.Movement
{
    public delegate void AnimalMovementBehaviorChangeEvent();
    
    public class AnimalMovementBehavior : IAnimalMovementBehavior
    {
        private readonly IAnimal animal;
        public bool InProgress { get; private set; }
        public event AnimalMovementBehaviorChangeEvent OnReachDestination;

        public AnimalMovementBehavior(IAnimal animal)
        {
            this.animal = animal;
        }
        
        public void Update()
        {
            if (!InProgress)
            {
                return;
            }

            if (animal.NavMeshAgent.pathPending)
            {
                return;
            }

            if (animal.NavMeshAgent.remainingDistance >= animal.NavMeshAgent.stoppingDistance)
            {
                return;
            }

            if (animal.NavMeshAgent.hasPath && animal.NavMeshAgent.velocity.sqrMagnitude != 0f)
            {
                return;
            }

            this.OnReachDestination?.Invoke();
            Stop();
        }

        public void BeginTowards(Vector3 destination)
        {
            animal.Animator.SetBool(AnimalAnimatorState.IsRunning.AsString(), true);
            animal.NavMeshAgent.SetDestination(destination);
            animal.NavMeshAgent.isStopped = false;
            InProgress = true;
        }

        public void Stop()
        {
            animal.Animator.SetBool(AnimalAnimatorState.IsRunning.AsString(), false);
            animal.NavMeshAgent.isStopped = true;
            InProgress = false;
        }
    }
}
