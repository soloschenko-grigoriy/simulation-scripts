using System.Collections;
using System.Collections.Generic;
using GS.Animals;
using GS.Animals.States;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace PlayModeTests
{
    public class WolfIntegrationTest
    {
        private const string WolfPrefabPath = "Assets/Prefabs/Tests/WolfBlack_Test.prefab";
        private const string WaterBankPrefabPath = "Assets/Prefabs/Tests/WaterBank_Test.prefab";

        private Animal wolf;
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
            wolf = TestUtils.InstantiateTestPrefab(WolfPrefabPath, toCleanup).GetComponent<Animal>();
            yield return null;
            wolf.SpawnAt(new Vector3(0, 0, 0), "Hanz", AnimalSex.Male);
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
            Assert.IsInstanceOf<Resting>(wolf.StateMachine.CurrentState);
            Assert.IsFalse(wolf.Movement.InProgress);
            Assert.IsTrue(wolf.Wander.IsScheduled);

            var position = wolf.Position;

            yield return new WaitUntil(() => wolf.Wander.InProgress);
            Assert.IsFalse(wolf.Wander.IsScheduled);
            Assert.IsTrue(wolf.Movement.InProgress);
        }
        
          [UnityTest]
        public IEnumerator ThirstFlow()
        {
            Assert.IsInstanceOf<Resting>(wolf.StateMachine.CurrentState);

            // Increase Thirst to jump into Looking For Water state
            wolf.Thirst.Increase(AnimalStatChangeRate.Regular, 40f);
            yield return null; // Use yield to skip a frame.
            Assert.IsInstanceOf<LookingForWater>(wolf.StateMachine.CurrentState);
            yield return TestUtils.TestIfWandersNotSeeks(wolf);
            yield return TestUtils.TestIfDrainsResources(wolf);

            // Water bank found nearby so rabbit starts moving towards it
            var go = TestUtils.InstantiateTestPrefab(WaterBankPrefabPath, toCleanup);
            go.transform.position = new Vector3(wolf.Characteristics.awareness - 2f, 0, wolf.Position.z);
            yield return null;
            Assert.IsNotNull(wolf.NearbyWater); // water detected
            TestUtils.TestIfSeeksNotWanders(wolf);

            yield return new WaitUntil(() => wolf.Seek.IsCompleted); // wait while rabbit gets to the food bank
            yield return null; // Use yield to skip a frame.

            // should stop moving and start drinking
            TestUtils.TestNotSeeksNorWanders(wolf);
            Assert.IsInstanceOf<Drinking>(wolf.StateMachine.CurrentState);

            yield return TestUtils.TestIfReplenishResources(wolf);

            // Speedup hunger decrease to get to Resting state again
            wolf.Thirst.Decrease(AnimalStatChangeRate.Regular, 40f);
            yield return null; // Use yield to skip a frame.
            Assert.IsInstanceOf<Resting>(wolf.StateMachine.CurrentState);
        }

        [UnityTest]
        public IEnumerator BreedingFlow()
        {
            Assert.IsInstanceOf<Resting>(wolf.StateMachine.CurrentState);

            // must be not hungry, thirsty or tired
            wolf.Hunger.Decrease(AnimalStatChangeRate.Regular, 40f);
            wolf.Thirst.Decrease(AnimalStatChangeRate.Regular, 40f);
            wolf.Stamina.Replenish(AnimalStatChangeRate.Regular, 40f);

            // must be old enough
            wolf.Age.Increase(80f);
            yield return null; 
            Assert.That(wolf.Age.Current, Is.EqualTo(AnimalAge.YoungAdult));
            Assert.IsInstanceOf<LookingForSex>(wolf.StateMachine.CurrentState);

            yield return TestUtils.TestIfWandersNotSeeks(wolf);
            yield return TestUtils.TestIfDrainsResources(wolf);

            // add another animal of the opposite sex nearby
            var otherAnimal = TestUtils.InstantiateTestPrefab(WolfPrefabPath, toCleanup).GetComponent<Animal>();
            otherAnimal.SpawnAt(new Vector3(100, 0, 100), "Kyle", wolf.Sex.GetOpposite());
            
            // add yet another animal of the opposite sex nearby
            var yetAnotherAnimal = TestUtils.InstantiateTestPrefab(WolfPrefabPath, toCleanup).GetComponent<Animal>();
            yetAnotherAnimal.SpawnAt(new Vector3(-100, 0, -100), "Michael", wolf.Sex.GetOpposite());

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
            otherAnimal.transform.position = new Vector3(wolf.Characteristics.awareness - 2f, 0, wolf.Position.z);
            yield return new WaitForSeconds(1f); 

            Assert.That(wolf.NearbyPartner, Is.EqualTo(otherAnimal));
            Assert.That(otherAnimal.NearbyPartner, Is.EqualTo(wolf));
            TestUtils.TestIfChasesNotWanders(wolf);
            
            // and this has nothing else but just keep wandering
            yetAnotherAnimal.transform.position = new Vector3(wolf.Characteristics.awareness - 2f, 0, wolf.Position.z);
            yield return null;
            Assert.That(yetAnotherAnimal.StateMachine.CurrentState, Is.InstanceOf<LookingForSex>());
            Assert.That(yetAnotherAnimal.NearbyPartner, Is.Null);
            
            // wait while rabbit gets to the sole mate
            yield return new WaitUntil(() => wolf.Chase.IsTargetReached && otherAnimal.Chase.IsTargetReached); 
            // should stop moving and start breeding
            TestUtils.TestNotChasesNorWanders(wolf);
            TestUtils.TestNotChasesNorWanders(otherAnimal);
            yield return null;
            
            Assert.That(wolf.StateMachine.CurrentState, Is.InstanceOf<Breeding>());
            Assert.That(otherAnimal.StateMachine.CurrentState, Is.InstanceOf<Breeding>());
            Assert.That(yetAnotherAnimal.StateMachine.CurrentState, Is.Not.InstanceOf<Breeding>());

            // Wait for new animal to be born
            yield return new WaitForSeconds(2);
            Assert.That(GameObject.FindObjectsOfType<Animal>().Length, Is.GreaterThanOrEqualTo(4));
            Assert.IsTrue(wolf.Breed.JustBreded);
            Assert.IsTrue(otherAnimal.Breed.JustBreded);
            Assert.IsFalse(yetAnotherAnimal.Breed.JustBreded);
        }
    
    }
}
