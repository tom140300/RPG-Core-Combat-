using System;
using GameDevTV.Utils;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)] [SerializeField] private int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] private Progression progression = null;
        [SerializeField] private GameObject levelUpParticleEffect = null;
        [SerializeField] private bool shouldUseModifiers = false;
        LazyValue<int>  currentLevel;
        public Action onLevelUp;
        private Experiences experiences;

        private void Awake()
        {
            experiences = GetComponent<Experiences>();
            currentLevel = new LazyValue<int>(CaculateLevel);
        }

        private void OnEnable()
        {
            if (experiences != null)
            {
                experiences.OnExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experiences != null)
            {
                experiences.OnExperienceGained -= UpdateLevel;
            }
        }

        private void Start()
        {
            currentLevel.ForceInit();
        }

        public void UpdateLevel()
        {
            int newLevel = CaculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        private int GetLevel()
        {
            return currentLevel.value;
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetPercentageModifier(Stat stat)
        {
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageMidifier(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifier(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        public int CaculateLevel()
        {
            Experiences experiences = GetComponent<Experiences>();
            if (experiences == null) return startingLevel;
            float currentXP = experiences.GetPoint();
            int penultimateLevel =
                progression.GetLevels(Stat.ExperienceToLevelUp, characterClass); // cap cao nhat co the dat dc
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}