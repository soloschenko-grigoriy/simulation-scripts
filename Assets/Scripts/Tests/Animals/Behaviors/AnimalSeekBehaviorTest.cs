using GS.Animals;
using GS.Animals.Behaviors.Movement;
using GS.Animals.Behaviors.Seek;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Animals.Behaviors
{
    public class AnimalSeekBehaviorTest
    {
        private IAnimal animal;
        private AnimalSeekBehavior seek;
        private IAnimalMovementBehavior movement;
        private readonly Vector3 target = new Vector3(10, 0, 10);

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            animal = Substitute.For<IAnimal>();
            movement = Substitute.For<IAnimalMovementBehavior>();
            animal.Movement.Returns(movement);
        }
        
        [SetUp]
        public void Setup()
        {
            seek = new AnimalSeekBehavior(animal);
            movement.ClearReceivedCalls();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            animal = null;
            seek = null;
            movement = null;
        }

        [Test]
        public void StopsWhenCanceled()
        {
            seek.Begin(target);

            movement.Received().BeginTowards(target);
            Assert.IsTrue(seek.InProgress);
            Assert.IsFalse(animal.Seek.IsCompleted);

            seek.Cancel();
            movement.Received().Stop();
            Assert.IsFalse(seek.InProgress);
            Assert.IsFalse(animal.Seek.IsCompleted);
        }
        
        [Test]
        public void StopsWhenReachesDestination()
        {
            seek.Begin(target);
            Assert.IsTrue(seek.InProgress);
            Assert.IsFalse(seek.IsCompleted);

            movement.OnReachDestination += Raise.Event<AnimalMovementBehaviorChangeEvent>();
            Assert.IsTrue(seek.IsCompleted);
        }
    }
}
