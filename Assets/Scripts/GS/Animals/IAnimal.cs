using System.Collections;
using GS.Animals.Behaviors.Age;
using GS.Animals.Behaviors.Breed;
using GS.Animals.Behaviors.Chase;
using GS.Animals.Behaviors.Flee;
using GS.Animals.Behaviors.Hunger;
using GS.Animals.Behaviors.Movement;
using GS.Animals.Behaviors.Seek;
using GS.Animals.Behaviors.Stamina;
using GS.Animals.Behaviors.Thirst;
using GS.Animals.Behaviors.Wander;
using GS.Environment;
using UnityEngine;
using UnityEngine.AI;

namespace GS.Animals
{
    public interface IAnimal : IPositionable
    {
        bool BeingDevoured { get; set; }
        AnimalCharacteristics Characteristics { get; }
        NavMeshAgent NavMeshAgent { get; }
        Animator Animator { get; }

        StateMachine StateMachine { get; }
        Animal AnimalPrefab { get; }
        AnimalSex Sex { get; }
        IAnimalSeekBehavior Seek { get; }
        IAnimalChaseBehavior Chase { get; }
        IAnimalFleeBehavior Flee { get; }
        IAnimalWanderBehavior Wander { get; }
        IAnimalBreedBehavior Breed { get; }
        IAnimalHungerBehavior Hunger { get; }
        IAnimalThirstBehavior Thirst { get; }
        IAnimalStaminaBehavior Stamina { get;  }
        IAnimalMovementBehavior Movement { get; }
        IAnimalAgeBehavior Age { get; }
        FoodBank NearbyFood { get; }
        WaterBank NearbyWater { get; }
        IAnimal NearbyPartner { get; }
        IAnimal NearbyPray { get; }
        IAnimal NearbyPredator { get; }
        bool IsSelected { get;  }
        Coroutine RunCoroutine(IEnumerator enumerator);
        void Die();
    }
}
