using System;
using System.Collections.Generic;
using GS.Animals;
using GS.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GS
{
    public class Gameplay : MonoBehaviour
    {
        [SerializeField] private HUD hud;
        [SerializeField] private Animal rabbitPrefab;
        [SerializeField] private Animal wolfPrefab;
        [SerializeField] private Transform initSpawnPosition;
        [SerializeField] [Range(1, 1000)] private int initAmountOfRabbits = 10;
        [SerializeField] [Range(1, 1000)] private int initAmountOfWolfs = 5;

        private readonly List<IAnimal> rabbits = new List<IAnimal>();
        private readonly List<IAnimal> wolfs = new List<IAnimal>();
        private readonly Stats rabbitStats = new Stats();
        private readonly Stats wolfStats = new Stats();

        private void Start()
        {
            rabbitStats.Reset();
            wolfStats.Reset();

            Animal.OnBorn += animal => GetProperList(animal).Add(animal);
            Animal.OnDie += animal => GetProperList(animal).Remove(animal);

            for (var i = 0; i < initAmountOfRabbits; i++)
            {
                var at = initSpawnPosition.position + (Random.insideUnitSphere * 100);
                Instantiate(rabbitPrefab).SpawnAt(new Vector3(at.x, 0, at.z));
            }

            for (var i = 0; i < initAmountOfWolfs; i++)
            {
                var at = initSpawnPosition.position + (Random.insideUnitSphere * 100);
                Instantiate(wolfPrefab).SpawnAt(new Vector3(at.x, 0, at.z));
            }

            Cursor.visible = true;
        }

        private void Update()
        {
            rabbitStats.Recount(rabbits);
            wolfStats.Recount(wolfs);

            hud?.UpdateStats(rabbitStats, wolfStats);
            
            // ListenForInput();
        }

        private List<IAnimal> GetProperList(IAnimal animal)
        {
            return animal.Characteristics.specimen switch {
                AnimalSpecimen.Rabbit => rabbits,
                AnimalSpecimen.Wolf => wolfs,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
