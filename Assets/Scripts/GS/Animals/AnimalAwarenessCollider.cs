using System.Linq;
using GS.Animals.States;
using GS.Environment;
using UnityEngine;

namespace GS.Animals
{
    internal class AnimalAwarenessCollider : MonoBehaviour
    {
        [SerializeField] private Animal animal;
        private SphereCollider sphereCollider;

        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.radius = animal.Characteristics.awareness;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("FoodBank") && animal.Characteristics.specimen == AnimalSpecimen.Rabbit)
            {
                animal.NearbyFood = other.GetComponent<FoodBank>();
            }
            else if (other.CompareTag("WaterBank"))
            {
                animal.NearbyWater = other.GetComponent<WaterBank>();
            }
            else if (other.CompareTag("Animal"))
            {
                var otherAnimal = other.GetComponent<Animal>();
                if (animal.Characteristics.praysOn.Contains(otherAnimal.Characteristics.specimen))
                {
                    animal.NearbyPray = otherAnimal;
                    otherAnimal.NearbyPredator = animal;
                    return;
                }
                
                // animal already has significant other
                if (animal.NearbyPartner != null)
                {
                    return;
                }
                
                // only because this is biological breeding
                if (otherAnimal.Sex == animal.Sex)
                {
                    return;
                }
                
                // it must be consensual! 
                if (!(otherAnimal.StateMachine.CurrentState is LookingForSex))
                {
                    return;
                }

                // other does not have partner already... monogamy you know...
                if (otherAnimal.NearbyPartner != null && otherAnimal.NearbyPartner as Animal != animal)
                {
                    return;
                }
                
                animal.NearbyPartner = otherAnimal;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("FoodBank"))
            {
                animal.NearbyFood = null;
            }
            else if (other.CompareTag("WaterBank"))
            {
                animal.NearbyWater = null;
            }
            else if (other.CompareTag("Animal"))
            {
                var otherAnimal = other.GetComponent<Animal>();
                if (animal.Characteristics.specimen.IsEqual(otherAnimal.Characteristics.specimen))
                {
                    animal.NearbyPartner = null;
                }
                else if (animal.Characteristics.praysOn.Contains(otherAnimal.Characteristics.specimen))
                {
                    animal.NearbyPray = null;
                    otherAnimal.NearbyPredator = null;
                }
            }
        }
    }
}
