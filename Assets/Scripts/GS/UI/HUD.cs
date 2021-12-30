using UnityEngine;

namespace GS.UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private Item _rabbitTotal;
        [SerializeField] private Item _rabbitInfants;
        [SerializeField] private Item _rabbitChildren;
        [SerializeField] private Item _rabbitTeen;
        [SerializeField] private Item _rabbitYoungAdults;
        [SerializeField] private Item _rabbitAdults;
        [SerializeField] private Item _rabbitElderly;
        [SerializeField] private Item _rabbitResting;
        [SerializeField] private Item _rabbitLookingForFood;
        [SerializeField] private Item _rabbitEating;
        [SerializeField] private Item _rabbitLookingForWater;
        [SerializeField] private Item _rabbitDrinking;
        [SerializeField] private Item _rabbitLookingForSex;
        [SerializeField] private Item _rabbitBreeding;
        [SerializeField] private Item _rabbitRunning;
        [SerializeField] private Item _rabbitMales;
        [SerializeField] private Item _rabbitFemales;

        [SerializeField] private Item _wolfTotal;
        [SerializeField] private Item _wolfInfants;
        [SerializeField] private Item _wolfChildren;
        [SerializeField] private Item _wolfTeen;
        [SerializeField] private Item _wolfYoungAdults;
        [SerializeField] private Item _wolfAdults;
        [SerializeField] private Item _wolfElderly;
        [SerializeField] private Item _wolfResting;
        [SerializeField] private Item _wolfHunting;
        [SerializeField] private Item _wolfEating;
        [SerializeField] private Item _wolfLookingForWater;
        [SerializeField] private Item _wolfDrinking;
        [SerializeField] private Item _wolfLookingForSex;
        [SerializeField] private Item _wolfBreeding;
        [SerializeField] private Item _wolfMales;
        [SerializeField] private Item _wolfFemales;

        public void UpdateStats(Stats rabbits, Stats wolfs)
        {
            _rabbitTotal.Value = rabbits.total.ToString();
            _rabbitInfants.Value = rabbits.infants.ToString();
            _rabbitChildren.Value = rabbits.children.ToString();
            _rabbitTeen.Value = rabbits.teen.ToString();
            _rabbitYoungAdults.Value = rabbits.youngAdults.ToString();
            _rabbitAdults.Value = rabbits.adults.ToString();
            _rabbitElderly.Value = rabbits.elderly.ToString();
            _rabbitResting.Value = rabbits.resting.ToString();
            _rabbitLookingForFood.Value = rabbits.lookingForFood.ToString();
            _rabbitEating.Value = rabbits.eating.ToString();
            _rabbitLookingForWater.Value = rabbits.lookingForWater.ToString();
            _rabbitDrinking.Value = rabbits.drinking.ToString();
            _rabbitLookingForSex.Value = rabbits.lookingForSex.ToString();
            _rabbitBreeding.Value = rabbits.breeding.ToString();
            _rabbitRunning.Value = rabbits.running.ToString();
            _rabbitMales.Value = rabbits.males.ToString();
            _rabbitFemales.Value = rabbits.females.ToString();

            _wolfTotal.Value = wolfs.total.ToString();
            _wolfInfants.Value = wolfs.infants.ToString();
            _wolfChildren.Value = wolfs.children.ToString();
            _wolfTeen.Value = wolfs.teen.ToString();
            _wolfElderly.Value = wolfs.elderly.ToString();
            _wolfYoungAdults.Value = wolfs.youngAdults.ToString();
            _wolfAdults.Value = wolfs.adults.ToString();
            _wolfResting.Value = wolfs.resting.ToString();
            _wolfHunting.Value = wolfs.hunting.ToString();
            _wolfEating.Value = wolfs.eating.ToString();
            _wolfLookingForWater.Value = wolfs.lookingForWater.ToString();
            _wolfDrinking.Value = wolfs.drinking.ToString();
            _wolfLookingForSex.Value = wolfs.lookingForSex.ToString();
            _wolfBreeding.Value = wolfs.breeding.ToString();
            _wolfMales.Value = wolfs.males.ToString();
            _wolfFemales.Value = wolfs.females.ToString();
        }
    }
}
