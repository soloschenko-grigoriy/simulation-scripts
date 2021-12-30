using System.Collections;
using System.Collections.Generic;
using GS.Animals;
using GS.Animals.States;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace PlayModeTests
{
    public class RabbitIntegrationTest
    {
        private const string RabbitPrefabPath = "Assets/Prefabs/Tests/RabbitWhite_Test.prefab";
        private const string FoodBankPrefabPath = "Assets/Prefabs/Tests/FoodBank_Test.prefab";
        private const string WaterBankPrefabPath = "Assets/Prefabs/Tests/WaterBank_Test.prefab";
        private Animal rabbit;
        private readonly List<GameObject> toCleanup = new List<GameObject>();

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            TestUtils.SetupScene();
        }

        [UnitySetUp]
        public IEnumerator Setup()
        {
            yield return null;
            rabbit = TestUtils.InstantiateTestPrefab(RabbitPrefabPath, toCleanup).GetComponent<Animal>();
            yield return null;
            rabbit.SpawnAt(new Vector3(0, 0, 0), "Shelagh", AnimalSex.Male);
        }

        [TearDown]
        public void Teardown()
        {
            TestUtils.CleanUp(toCleanup);
        }

        [UnityTest]
        public IEnumerator HungerFlow()
        {
            // resting state, movement only scheduled
            Assert.IsInstanceOf<Resting>(rabbit.StateMachine.CurrentState);
            Assert.IsFalse(rabbit.Movement.InProgress);
            Assert.IsTrue(rabbit.Wander.IsScheduled);

            var position = rabbit.Position;

            yield return new WaitUntil(() => rabbit.Wander.InProgress);
            Assert.IsFalse(rabbit.Wander.IsScheduled);
            Assert.IsTrue(rabbit.Movement.InProgress);

            // Destination should be in defined range
            var distance = Vector3.Distance(rabbit.NavMeshAgent.destination, position);
            Assert.That(Mathf.Ceil(distance), Is.GreaterThanOrEqualTo(rabbit.Characteristics.wanderInnerCircle));
            Assert.That(Mathf.Floor(distance), Is.LessThanOrEqualTo(rabbit.Characteristics.wanderOuterCircle));

            // Should become hungry when reaches the point 
            rabbit.Hunger.Increase(AnimalStatChangeRate.Regular, 40f);
            Assert.That(rabbit.Hunger.Current, Is.GreaterThanOrEqualTo(rabbit.Characteristics.hungerUpperThreshold));
            yield return null; // Use yield to skip a frame.
            Assert.IsInstanceOf<LookingForFood>(rabbit.StateMachine.CurrentState);
            yield return TestUtils.TestIfWandersNotSeeks(rabbit);
            yield return TestUtils.TestIfDrainsResources(rabbit);

            // Food bank found nearby so rabbit starts moving towards it
            var go = TestUtils.InstantiateTestPrefab(FoodBankPrefabPath, toCleanup);
            go.transform.position = new Vector3(rabbit.Characteristics.awareness - 2f, 0, rabbit.Position.z);
            yield return null;
            Assert.IsNotNull(rabbit.NearbyFood); // food detected
            TestUtils.TestIfSeeksNotWanders(rabbit);

            yield return new WaitUntil(() => rabbit.Seek.IsCompleted); // wait while rabbit gets to the food bank
            yield return null; // Use yield to skip a frame.

            // should stop moving and start eating
            TestUtils.TestNotSeeksNorWanders(rabbit);
            Assert.IsInstanceOf<Eating>(rabbit.StateMachine.CurrentState);

            yield return TestUtils.TestIfReplenishResources(rabbit);

            // Speedup hunger decrease to get to Resting state again
            rabbit.Hunger.Decrease(AnimalStatChangeRate.Regular, 40f);
            yield return null; // Use yield to skip a frame.
            Assert.IsInstanceOf<Resting>(rabbit.StateMachine.CurrentState);
        }

        [UnityTest]
        public IEnumerator ThirstFlow()
        {
            Assert.IsInstanceOf<Resting>(rabbit.StateMachine.CurrentState);

            // Increase Thirst to jump into Looking For Water state
            rabbit.Thirst.Increase(AnimalStatChangeRate.Regular, 40f);
            yield return null; // Use yield to skip a frame.
            Assert.IsInstanceOf<LookingForWater>(rabbit.StateMachine.CurrentState);
            yield return TestUtils.TestIfWandersNotSeeks(rabbit);
            yield return TestUtils.TestIfDrainsResources(rabbit);

            // Water bank found nearby so rabbit starts moving towards it
            var go = TestUtils.InstantiateTestPrefab(WaterBankPrefabPath, toCleanup);
            go.transform.position = new Vector3(rabbit.Characteristics.awareness - 2f, 0, rabbit.Position.z);
            yield return null;
            Assert.IsNotNull(rabbit.NearbyWater); // water detected
            TestUtils.TestIfSeeksNotWanders(rabbit);

            yield return new WaitUntil(() => rabbit.Seek.IsCompleted); // wait while rabbit gets to the food bank
            yield return null; // Use yield to skip a frame.

            // should stop moving and start drinking
            TestUtils.TestNotSeeksNorWanders(rabbit);
            Assert.IsInstanceOf<Drinking>(rabbit.StateMachine.CurrentState);

            yield return TestUtils.TestIfReplenishResources(rabbit);

            // Speedup hunger decrease to get to Resting state again
            rabbit.Thirst.Decrease(AnimalStatChangeRate.Regular, 40f);
            yield return null; // Use yield to skip a frame.
            Assert.IsInstanceOf<Resting>(rabbit.StateMachine.CurrentState);
        }

        [UnityTest]
        public IEnumerator BreedingFlow()
        {
            Assert.IsInstanceOf<Resting>(rabbit.StateMachine.CurrentState);

            // must be not hungry, thirsty or tired
            rabbit.Hunger.Decrease(AnimalStatChangeRate.Regular, 40f);
            rabbit.Thirst.Decrease(AnimalStatChangeRate.Regular, 40f);
            rabbit.Stamina.Replenish(AnimalStatChangeRate.Regular, 40f);

            // must be old enough
            rabbit.Age.Increase(80f);
            yield return null; 
            Assert.That(rabbit.Age.Current, Is.EqualTo(AnimalAge.YoungAdult));
            Assert.IsInstanceOf<LookingForSex>(rabbit.StateMachine.CurrentState);

            yield return TestUtils.TestIfWandersNotSeeks(rabbit);
            yield return TestUtils.TestIfDrainsResources(rabbit);

            // add another animal of the opposite sex nearby
            var otherAnimal = TestUtils.InstantiateTestPrefab(RabbitPrefabPath, toCleanup).GetComponent<Animal>();
            otherAnimal.SpawnAt(new Vector3(100, 0, 100), "Kyle", rabbit.Sex.GetOpposite());
            
            // add yet another animal of the opposite sex nearby
            var yetAnotherAnimal = TestUtils.InstantiateTestPrefab(RabbitPrefabPath, toCleanup).GetComponent<Animal>();
            yetAnotherAnimal.SpawnAt(new Vector3(-100, 0, -100), "Michael", rabbit.Sex.GetOpposite());

            // both must also be not hungry, thirsty or tired, old enough
            otherAnimal.Hunger.Decrease(AnimalStatChangeRate.Regular, 40f);
            otherAnimal.Thirst.Decrease(AnimalStatChangeRate.Regular, 40f);
            otherAnimal.Stamina.Replenish(AnimalStatChangeRate.Regular, 40f);
            otherAnimal.Age.Increase(80f);
            
            yetAnotherAnimal.Hunger.Decrease(AnimalStatChangeRate.Regular, 40f);
            yetAnotherAnimal.Thirst.Decrease(AnimalStatChangeRate.Regular, 40f);
            yetAnotherAnimal.Stamina.Replenish(AnimalStatChangeRate.Regular, 40f);
            yetAnotherAnimal.Age.Increase(80f);
            
            // and looking for sex
            yield return null; 
            Assert.That(otherAnimal.Age.Current, Is.EqualTo(AnimalAge.YoungAdult));
            Assert.That(otherAnimal.StateMachine.CurrentState, Is.InstanceOf<LookingForSex>());
            
            Assert.That(yetAnotherAnimal.Age.Current, Is.EqualTo(AnimalAge.YoungAdult));
            Assert.That(yetAnotherAnimal.StateMachine.CurrentState, Is.InstanceOf<LookingForSex>());
            
            // but this gets closer first and becomes partner
            otherAnimal.transform.position = new Vector3(rabbit.Characteristics.awareness - 2f, 0, rabbit.Position.z);
            yield return new WaitForSeconds(1f); 

            Assert.That(rabbit.NearbyPartner, Is.EqualTo(otherAnimal));
            Assert.That(otherAnimal.NearbyPartner, Is.EqualTo(rabbit));
            TestUtils.TestIfChasesNotWanders(rabbit);
            
            // and this has nothing else but just keep wandering
            yetAnotherAnimal.transform.position = new Vector3(rabbit.Characteristics.awareness - 2f, 0, rabbit.Position.z);
            yield return null;
            Assert.That(yetAnotherAnimal.StateMachine.CurrentState, Is.InstanceOf<LookingForSex>());
            Assert.That(yetAnotherAnimal.NearbyPartner, Is.Null);
            
            // wait while rabbit gets to the sole mate
            yield return new WaitUntil(() => rabbit.Chase.IsTargetReached && otherAnimal.Chase.IsTargetReached); 
            // should stop moving and start breeding
            TestUtils.TestNotChasesNorWanders(rabbit);
            TestUtils.TestNotChasesNorWanders(otherAnimal);
            yield return null;
            
            Assert.That(rabbit.StateMachine.CurrentState, Is.InstanceOf<Breeding>());
            Assert.That(otherAnimal.StateMachine.CurrentState, Is.InstanceOf<Breeding>());
            Assert.That(yetAnotherAnimal.StateMachine.CurrentState, Is.Not.InstanceOf<Breeding>());

            // Wait for new animal to be born
            yield return new WaitForSeconds(2);
            Assert.That(GameObject.FindObjectsOfType<Animal>().Length, Is.EqualTo(4));
            Assert.IsTrue(rabbit.Breed.JustBreded);
            Assert.IsTrue(otherAnimal.Breed.JustBreded);
            Assert.IsFalse(yetAnotherAnimal.Breed.JustBreded);
        }
    }
}
