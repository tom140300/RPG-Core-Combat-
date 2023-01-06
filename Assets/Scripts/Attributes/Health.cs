using System;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        LazyValue<float> healthPoint;
        private bool isDead;
        [SerializeField] private float regenerateHealth = 70;

        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] private UnityEvent onDie;
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        private void Awake()
        {
            healthPoint = new LazyValue<float>(GetInitialHealth);
        }

        float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            healthPoint.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        private void RegenerateHealth()
        {
            float regenerateHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * regenerateHealth / 100;
            healthPoint.value = Mathf.Max(healthPoint.value, regenerateHealthPoints);
        }
        public void Heal(float healthToRestore)
        {   
            healthPoint.value = Mathf.Min(healthPoint.value + healthToRestore, GetMaxHealthPoints());
        }

        public bool IsDead()
        {
            return isDead;
        }

        public float GetHealthPoints()
        {
            return healthPoint.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        
        public float GetPercenttage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return healthPoint.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        public void TakeDamage(GameObject instigator, float damage) //
        {
            healthPoint.value = Mathf.Max(healthPoint.value - damage, 0);
            if (healthPoint.value <= 0)
            {
                onDie.Invoke();
                Die();
                AwardExperiences(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        private void AwardExperiences(GameObject instigator)
        {
            Experiences experiences = instigator.GetComponent<Experiences>();
            if (experiences == null) return;
            experiences.GainExperiences(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoint.value;
        }

        public void RestoreState(object state)
        {
            healthPoint.value = (float)state;
            if (healthPoint.value <= 0)
            {
                Die();
            }
        }
    }
}