using System;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour //Trình tạo đối tượng liên tục
    {
        [SerializeField] private GameObject persistentObjectSpawner;
        static bool hasSpawned = false;

        private void Awake()
        {
            if(hasSpawned)return;
            SpawnPersistentObjects();
            hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistentObjectSpawner);
            DontDestroyOnLoad(persistentObject);
        }
    }
}