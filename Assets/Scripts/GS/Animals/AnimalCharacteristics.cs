using UnityEngine;

namespace GS.Animals
{
    [CreateAssetMenu(menuName = "ScriptableObjects/AnimalCharacteristics", order = 1)]
    public class AnimalCharacteristics : ScriptableObject
    {
        public AnimalSpecimen specimen = default;
        [Range(10, 50)] public float awareness = 10f;
        [Range(0.001f, 0.05f)] public float agingSpeed = 0.005f;

        [Range(0.001f, 0.9f)] public float thirstChangeDefaultRate = 0.01f;
        [Range(0.1f, 0.9f)] public float thirstUpperThreshold = 0.4f;
        [Range(0.1f, 0.9f)] public float thirstLowerThreshold = 0.2f;

        [Range(0.001f, 0.9f)] public float hungerChangeDefaultRate = 0.01f;
        [Range(0.1f, 0.9f)] public float hungerUpperThreshold = 0.4f;
        [Range(0.1f, 0.9f)] public float hungerLowerThreshold = 0.2f;

        [Range(0.001f, 0.9f)] public float staminaChangeDefaultRate = 0.01f;
        [Range(0.1f, 0.9f)] public float staminaUpperThreshold = 0.5f;
        [Range(0.1f, 0.9f)] public float staminaLowerThreshold = 0.2f;

        [Range(6, 20)] public int wanderOuterCircle = 10;
        [Range(1, 6)] public int wanderInnerCircle = 5;

        public int wanderWaitMinTime = 1;
        public int wanderWaitMaxTime = 10;
        public float breedingCooldown = 10f;

        public string[] maleNamesPool = default;
        public string[] femaleNamesPool = default;
        public AnimalSpecimen[] praysOn = default;
    }
}
