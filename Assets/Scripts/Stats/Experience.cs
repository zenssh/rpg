using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        public event Action OnExperienceGained;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            OnExperienceGained();
        }

        public float GetCurrentXP()
        {
            return experiencePoints;
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            if (state == null) experiencePoints = 0;
            else experiencePoints = (float)state;
        }
    }
}