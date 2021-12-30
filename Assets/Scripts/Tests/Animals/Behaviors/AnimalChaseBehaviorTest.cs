using GS.Animals;
using GS.Animals.Behaviors.Chase;
using GS.Animals.Behaviors.Movement;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Animals.Behaviors
{
    public class AnimalChaseBehaviorTest
    {
        private IAnimal animal;
        private AnimalChaseBehavior chase;
        private IPositionable target;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            animal = Substitute.For<IAnimal>();
            animal.Movement.Returns(Substitute.For<IAnimalMovementBehavior>());
            target = Substitute.For<IPositionable>();
        }

        [SetUp]
        public void Setup()
        {
            target.Position.Returns(new Vector3(10, 0, 10));
            chase = new AnimalChaseBehavior(animal);
            animal.Movement.ClearReceivedCalls();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            animal = null;
            chase = null;
        }

        [Test]
        public void StopsWhenCanceled()
        {
            chase.Begin(target, 0f);
            animal.Movement.Received().BeginTowards(target.Position);
            Assert.IsFalse(chase.IsTargetReached);
            
            chase.Cancel();
            animal.Movement.Received().Stop();
            Assert.IsFalse(chase.IsTargetReached);
        }

        [Test]
        public void StopsWhenReachesDestination()
        {
            chase.Begin(target, 0f);
            Assert.IsFalse(chase.IsTargetReached);

            animal.Movement.OnReachDestination += Raise.Event<AnimalMovementBehaviorChangeEvent>();
            Assert.IsTrue(chase.IsTargetReached);
        }

        [Test]
        public void ChangesDestinationOnUpdate()
        {
            chase.Begin(target, 20f);
            
            // threshold not yet met, skipping
            target.Position.Returns(new Vector3(20, 0, 20));
            chase.Update(10f);
            animal.Movement.DidNotReceive().Stop();
            animal.Movement.DidNotReceive().BeginTowards(target.Position);
            Assert.IsTrue(chase.InProgress);
            Assert.IsFalse(chase.IsTargetReached);
            animal.Movement.ClearReceivedCalls();
            
            // now should update
            target.Position.Returns(new Vector3(30, 0, 0));
            chase.Update(20f);
            animal.Movement.Received().Stop();
            animal.Movement.Received().BeginTowards(target.Position);
            Assert.IsFalse(chase.IsTargetReached);
            animal.Movement.ClearReceivedCalls();
            
            // skips again since not enough time has passed
            target.Position.Returns(new Vector3(20, 0, 20));
            chase.Update(30f);
            animal.Movement.DidNotReceive().Stop();
            animal.Movement.DidNotReceive().BeginTowards(target.Position);
            Assert.IsTrue(chase.InProgress);
            Assert.IsFalse(chase.IsTargetReached);
            animal.Movement.ClearReceivedCalls();
            
            // but now should fo it
            target.Position.Returns(new Vector3(20, 0, 20));
            chase.Update(40f);
            animal.Movement.Received().Stop();
            animal.Movement.Received().BeginTowards(target.Position);
            Assert.IsTrue(chase.InProgress);
            Assert.IsFalse(chase.IsTargetReached);
            
            // and again after the next threshold
            target.Position.Returns(new Vector3(20, 0, 20));
            chase.Update(61f);
            animal.Movement.Received().Stop();
            animal.Movement.Received().BeginTowards(target.Position);
            Assert.IsTrue(chase.InProgress);
            Assert.IsFalse(chase.IsTargetReached);
        }
    }
}
