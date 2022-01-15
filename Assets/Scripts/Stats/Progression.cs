using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;

        [System.Serializable]
        public class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }

        [System.Serializable]
        public class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(CharacterClass characterClass, Stat stat, int level)
        {
            BuildLookup();
            float[] levels = lookupTable[characterClass][stat];
            if (levels.Length < level) return levels[levels.Length - 1];
            return levels[level - 1];
        }

        public int GetLevels(CharacterClass characterClass, Stat stat)
        {
            BuildLookup();
            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();
                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }
                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }

        // public float GetHealth(CharacterClass characterClass, int level)
        // {
        //     return GetStat(characterClass, Stat.Health, level);
        // }

        // public float GetExperience(CharacterClass characterClass, int level)
        // {
        //     return GetStat(characterClass, Stat.ExperienceReward, level);
        // }
    }
}