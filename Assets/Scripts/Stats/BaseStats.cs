using System;
using GameDevTV.Utils;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,20)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression;
        [SerializeField] bool shouldUseModifiers = false;
        Experience experience;

        LazyValue<int> currentLevel;

        [SerializeField] GameObject levelUpParticleEffect = null;
        public event Action onLevelUp;

        void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        void Start()
        {
            currentLevel.ForceInit();
        }

        void OnEnable()
        {
            if (experience != null) experience.OnExperienceGained += UpdateLevel;
        }

        void OnDisable()
        {
            if (experience != null) experience.OnExperienceGained -= UpdateLevel;
        }

        void UpdateLevel()
        {
            int newLevel = CalculateLevel();
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

        public float GetStat(Stat stat)
        {
            float modifiedStat = GetBaseStat(stat);
            if (shouldUseModifiers) modifiedStat = (modifiedStat + GetAdditiveModifier(stat)) * (1 + GetMultiplicativeModifier(stat));
            return modifiedStat;
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(characterClass, stat, GetLevel());
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }

        private int CalculateLevel()
        {
            if (experience == null) return startingLevel;
            
            float currentXP = experience.GetCurrentXP();
            int penultimateLevel = progression.GetLevels(characterClass, Stat.ExperienceToLevelUp);

            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUP = progression.GetStat(characterClass, Stat.ExperienceToLevelUp, level);
                if (XPToLevelUP > currentXP) return level;
            }

            return penultimateLevel + 1;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifiers in provider.GetAdditiveModifiers(stat))
                {
                    total += modifiers;
                }
            }
            return total;
        }

        private float GetMultiplicativeModifier(Stat stat)
        {
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifiers in provider.GetMultiplicativeModifiers(stat))
                {
                    total += modifiers;
                }
            }
            return total;
        }

    }
}
