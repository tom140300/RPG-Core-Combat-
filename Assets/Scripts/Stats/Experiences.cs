using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experiences : MonoBehaviour, ISaveable
    {
        [SerializeField] private float experiencePoints = 0;

       

        public Action  OnExperienceGained;// Đạt được kinh nghiệm
        public void GainExperiences(float experiences) //Đạt được kinh nghiệm
        {
            experiencePoints += experiences;
            OnExperienceGained();
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }

        public float GetPoint()
        {
            return experiencePoints;
        }
    }
}