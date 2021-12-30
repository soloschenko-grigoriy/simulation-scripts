using System;
using GS.Animals.States;
using GS.StateMachine;

namespace GS.Animals
{
    public class StateMachine : StateMachina
    {
        public StateMachine(IAnimal animal)
        {
            // States
            // State beingAttacked = new BeingAttacked(animal);
            State breeding = new Breeding(animal);
            State drinking = new Drinking(animal);
            State dying = new Dying(animal);
            State eating = new Eating(animal);
            State lookingForFood = new LookingForFood(animal);
            State lookingForSex = new LookingForSex(animal);
            State lookingForWater = new LookingForWater(animal);
            State resting = new Resting(animal);
            State fleeing = new Fleeing(animal);
            State hunting = new Hunting(animal);

            // Conditions
            bool EnoughStamina()
            {
                return animal.Stamina.Current >= animal.Characteristics.staminaUpperThreshold;
            }

            bool IsTired()
            {
                return animal.Stamina.Current < animal.Characteristics.staminaLowerThreshold;
            }

            bool IsHungry()
            {
                return animal.Hunger.Current >= animal.Characteristics.hungerUpperThreshold;
            }

            bool IsThirsty()
            {
                return animal.Thirst.Current >= animal.Characteristics.thirstUpperThreshold;
            }

            bool SearchCompleted()
            {
                return animal.Seek.IsCompleted;
            }

            bool ChaseCompleted()
            {
                return animal.Chase.IsTargetReached;
            }

            bool IsReproductive()
            {
                return animal.Age.Current == AnimalAge.Adult ||
                       animal.Age.Current == AnimalAge.YoungAdult;
            }

            bool JustBreded()
            {
                return animal.Breed.JustBreded;
            }

            bool RanAway()
            {
                return animal.Flee.GotAway;
            }

            bool InDanger()
            {
                return animal.NearbyPredator != null;
            }

            // Transitions
            var dyingTransition = new Transition(dying, () => animal.Hunger.Current > 1 || animal.Thirst.Current > 1 || animal.BeingDevoured);
            // var runningTransition = new Transition(running, () => animal.InDanger);
            var restingTransition = new Transition(resting, IsTired);
            var lookingForFoodTransition =
                new Transition(animal.Characteristics.praysOn.Length > 0 ? hunting : lookingForFood,
                    () => EnoughStamina() && IsHungry());
            var lookingForWaterTransition = new Transition(lookingForWater, () => EnoughStamina() && IsThirsty());
            var fleeingTransition = new Transition(fleeing, InDanger);

            resting.Transitions = new[] {
                dyingTransition, lookingForFoodTransition, lookingForWaterTransition, new Transition(lookingForSex,
                    () => EnoughStamina() && !IsThirsty() && !IsHungry() && IsReproductive() && !JustBreded()),
                fleeingTransition
            };

            lookingForFood.Transitions = new[] {
                new Transition(eating, SearchCompleted), dyingTransition, restingTransition, dyingTransition, fleeingTransition
            };

            hunting.Transitions = new[] {new Transition(eating, ChaseCompleted), dyingTransition, restingTransition, fleeingTransition};

            lookingForWater.Transitions = new[] {
                new Transition(drinking, SearchCompleted), dyingTransition, restingTransition, fleeingTransition
            };

            eating.Transitions = new[] {
                new Transition(resting, () => animal.Hunger.Current <= animal.Characteristics.hungerLowerThreshold),
                dyingTransition,
                fleeingTransition
            };

            drinking.Transitions = new[] {
                new Transition(resting, () => animal.Thirst.Current <= animal.Characteristics.thirstLowerThreshold),
                dyingTransition, fleeingTransition
            };

            lookingForSex.Transitions = new[] {
                new Transition(resting, () => IsTired() || !IsReproductive() || JustBreded()), lookingForFoodTransition,
                lookingForWaterTransition, new Transition(breeding, ChaseCompleted), dyingTransition, fleeingTransition
            };

            fleeing.Transitions = new[] {
                // resting only if no stamina left oe finally got awat from the chaser
                new Transition(resting, () => IsTired() || RanAway()),

                // no luck, being devoured or starved or thirst to death
                dyingTransition
            };

            breeding.Transitions = new[] {new Transition(resting, JustBreded), dyingTransition};

            // beingAttacked.Transitions = new[] {new Transition(dying, () => animal.IsDead)};
            dying.Transitions = Array.Empty<Transition>();

            // Wrap up State Machine
            State[] states = {
                breeding, drinking, dying, eating, lookingForFood, lookingForSex, lookingForWater,
                resting, fleeing
            };

            this.states = states;
            Start(resting);
        }
    }
}
