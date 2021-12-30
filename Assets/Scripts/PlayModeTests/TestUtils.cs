using System.Collections;
using System.Collections.Generic;
using GS.Animals;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayModeTests
{
    public static class TestUtils
    {
        private const string TestScenePath = "Assets/Scenes/TestScene.unity";

        
        public static void SetupScene()
        {
            EditorSceneManager.LoadSceneInPlayMode(TestScenePath,
                new LoadSceneParameters(LoadSceneMode.Single));
        }

        public static void CleanUp(List<GameObject> toCleanup)
        {
            foreach (var item in toCleanup)
            {
                Object.Destroy(item);
            }
        }
        
        public static IEnumerator TestIfWandersNotSeeks(IAnimal subject)
        {
            yield return new WaitUntil(() => subject.Wander.InProgress);
            Assert.IsTrue(subject.Wander.InProgress); // wanders
            Assert.IsTrue(subject.Movement.InProgress); // moves around
            Assert.IsFalse(subject.Seek.InProgress); // but not yet seeks
        }

        public static void TestIfSeeksNotWanders(IAnimal subject)
        {
            Assert.IsTrue(subject.Movement.InProgress); // moves around
            Assert.IsTrue(subject.Seek.InProgress); // and actually seeks
            Assert.IsFalse(subject.Wander.InProgress); // does not wanders anymore
        }

        public static void TestIfChasesNotWanders(IAnimal subject)
        {
            Assert.IsTrue(subject.Movement.InProgress); // moves around
            Assert.IsTrue(subject.Chase.InProgress); // and actually seeks
            Assert.IsFalse(subject.Wander.InProgress); // does not wanders anymore
        }

        public static void TestNotSeeksNorWanders(IAnimal subject)
        {
            Assert.IsFalse(subject.Movement.InProgress); // does not move around
            Assert.IsFalse(subject.Seek.InProgress); // and not seeks
            Assert.IsFalse(subject.Wander.InProgress); // does not wanders anymore
        }

        public static void TestNotChasesNorWanders(IAnimal subject)
        {
            Assert.IsFalse(subject.Movement.InProgress); // does not move around
            Assert.IsFalse(subject.Chase.InProgress); // and not chases
            Assert.IsFalse(subject.Wander.InProgress); // does not wanders anymore
        }

        public static IEnumerator TestIfReplenishResources(IAnimal subject)
        {
            // Hunger and Thirst decreases every frame, Stamina replenishes 
            var prevHunger = subject.Hunger.Current;
            var prevThirst = subject.Thirst.Current;
            var prevStamina = subject.Stamina.Current;
            yield return null; // Use yield to skip a frame.
            Assert.That(subject.Hunger.Current, Is.LessThan(prevHunger));
            Assert.That(subject.Thirst.Current, Is.LessThan(prevThirst));
            Assert.That(subject.Stamina.Current, Is.GreaterThan(prevStamina));
        }

        /// drains stamina, hunger and thirst increases each frame
        public static IEnumerator TestIfDrainsResources(IAnimal subject)
        {
            var prevHunger = subject.Hunger.Current;
            var prevThirst = subject.Thirst.Current;
            var prevStamina = subject.Stamina.Current;
            yield return null; // Use yield to skip a frame.
            Assert.That(subject.Hunger.Current, Is.GreaterThan(prevHunger));
            Assert.That(subject.Thirst.Current, Is.GreaterThan(prevThirst));
            Assert.That(subject.Stamina.Current, Is.LessThan(prevStamina));
        }

        public static GameObject InstantiateTestPrefab(string prefabPath, List<GameObject> toCleanup)
        {
            var go = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath));
            toCleanup.Add(go);

            return go;
        }

        public static Vector3 GetRandomPointNearby(IAnimal subject)
        {
            // var randomWaterPointNearby = animal.transform.position +
            //                              (Random.insideUnitSphere * (animal.Characteristics.awareness - 5f));
            // return new Vector3(randomWaterPointNearby.x, 0, randomWaterPointNearby.z);

            var radius = subject.Characteristics.awareness - 1f;
            var angle = Random.Range(0.1f, 1f) * Mathf.PI * 2;
            var x = Mathf.Cos(angle) * radius;
            var z = Mathf.Sin(angle) * radius;

            return subject.Position + new Vector3(x, subject.Position.y, z);
        }
    }
}

// [UnitySetUp]
// public IEnumerator OneTimeSetup()
// {
//     yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/TestScene.unity", new LoadSceneParameters(LoadSceneMode.Single));
//     
//     GameObject obj = Object.Instantiate(Resources.Load<GameObject>("Prefabs/RabbitWhite"));
//     animal = obj.GetComponent<Animal>();
//
//     yield return null;
//     
//     animal.SpawnAt(new Vector3(0, 0, 0));
//
//     yield return new WaitForSeconds(10f);
// }

// [SetUp] 
// public void SetUp()
// {
//     SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
//         
//     // cam = Object.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera"));
//     // navMesh = Object.Instantiate(Resources.Load<GameObject>("Prefabs/TestNavMesh"));
//
//     // navMesh.transform.position = new Vector3(0, 0, 0);
//     GameObject obj = Object.Instantiate(Resources.Load<GameObject>("Prefabs/RabbitWhite"));
//     animal = obj.GetComponent<Animal>();
// }

// [UnityTest]
// public IEnumerator AnimalWanderWithEnumeratorPasses()
// {
//     Assert.IsInstanceOf<Resting>(animal.StateMachine.CurrentState);
//     
//     yield return null;
//     
//     // animal.IncreaseHunger(AnimalHungerRate.Regular);
//     // animal.Hunger = animal.Characteristics.hungerUpperThreshold;
//     Assert.AreEqual(animal.Characteristics.hungerUpperThreshold, animal.Hunger.Current);
//     
//     yield return null; // Use yield to skip a frame.
//     
//     Assert.IsInstanceOf<LookingForFood>(animal.StateMachine.CurrentState);
// }
