﻿using System;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] private GameObject targetToDestroy = null;

        private void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                if (targetToDestroy != null)
                    Destroy(targetToDestroy);
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}