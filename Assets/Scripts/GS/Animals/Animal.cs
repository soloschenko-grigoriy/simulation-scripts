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
using GS.Helpers;
using UnityEngine;
using UnityEngine.AI;

namespace GS.Animals
{
    public delegate void AnimalEvent(Animal animal);

    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class Animal : MonoBehaviour, IAnimal
    {
        [SerializeField] private AnimalCharacteristics characteristics;
        [SerializeField] private Animal prefab;
        private RTSCameraController cameraController;

        private void Awake()
        {
            Movement = new AnimalMovementBehavior(this);
            Hunger = new AnimalHungerBehavior(this);
            Thirst = new AnimalThirstBehavior(this);
            Wander = new AnimalWanderBehavior(this);
            Seek = new AnimalSeekBehavior(this);
            Chase = new AnimalChaseBehavior(this);
            Flee = new AnimalFleeBehavior(this);
            Stamina = new AnimalStaminaBehavior(this);
            Breed = new AnimalBreedBehavior(this);
            Age = new AnimalAgeBehavior(this, Characteristics.agingSpeed);

            NavMeshAgent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
            cameraController = Camera.main.GetComponent<RTSCameraController>();
        }

        private void Update()
        {
            StateMachine?.Update(Time.deltaTime);
            Movement?.Update();
            Age?.Increase(Time.deltaTime);
            Breed.Update(Time.time);
            Chase.Update(Time.time);
            Flee.Update(Time.time);
        }

        private void OnMouseDown()
        {
            cameraController.SetTarget(transform);
            IsSelected = true;
        }

        public StateMachine StateMachine { get; private set; }

        public Animal AnimalPrefab => prefab;

        public AnimalCharacteristics Characteristics => characteristics;
        public NavMeshAgent NavMeshAgent { get; private set; }
        public Animator Animator { get; private set; }

        public AnimalSex Sex { get; private set; }
        public IAnimalWanderBehavior Wander { get; private set; }
        public IAnimalSeekBehavior Seek { get; private set; }
        public IAnimalChaseBehavior Chase { get; private set; }
        public IAnimalFleeBehavior Flee { get; private set; }
        public IAnimalBreedBehavior Breed { get; private set; }
        public IAnimalHungerBehavior Hunger { get; private set; }
        public IAnimalThirstBehavior Thirst { get; private set; }
        public IAnimalStaminaBehavior Stamina { get; private set; }
        public IAnimalMovementBehavior Movement { get; private set; }
        public IAnimalAgeBehavior Age { get; private set; }

        public bool BeingDevoured { get; set; } = false;

        public Vector3 Position => transform.position;

        public FoodBank NearbyFood { get; set; }
        public WaterBank NearbyWater { get; set; }
        public IAnimal NearbyPartner { get; set; }
        public IAnimal NearbyPray { get; set; }
        public IAnimal NearbyPredator { get; set; }

        public bool IsSelected { get; private set; }

        public Coroutine RunCoroutine(IEnumerator enumerator)
        {
            return StartCoroutine(enumerator);
        }

        public void Die()
        {
            OnDie?.Invoke(this);
            Destroy(gameObject);
        }

        public static event AnimalEvent OnBorn;
        public static event AnimalEvent OnDie;

        private static AnimalSex GetRandomSex()
        {
            return Random.Range(0, 100) <= 50 ? AnimalSex.Male : AnimalSex.Female;
        }

        public void SpawnAt(Vector3 motherPosition, string pseudonym = null, AnimalSex? sex = null)
        {
            Hunger.Reset();
            Thirst.Reset();
            Stamina.Reset();
            Age.Reset();
            Sex = sex ?? GetRandomSex();
            name = pseudonym ?? GetName();

            transform.position = motherPosition;
            NavMeshAgent.enabled = true;

            StateMachine = new StateMachine(this);

            OnBorn?.Invoke(this);
        }

        private string GetName()
        {
            var pool = Sex == AnimalSex.Female ? characteristics.femaleNamesPool : characteristics.maleNamesPool;
            return pool[Random.Range(0, pool.Length)];
        }
    }
}
