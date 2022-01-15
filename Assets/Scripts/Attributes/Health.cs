using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using GameDevTV.Utils;
using UnityEngine.Events;
using System;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        LazyValue<float> healthPoints;
        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] AudioSource dieSound;

        public event Action onDie;
        public event Action<float> onTakeDamage;

        [SerializeField] float regenPercentage = 70;
        bool isDead = false;
        BaseStats baseStats;

        void Awake()
        {
            
            baseStats = GetComponent<BaseStats>();
            healthPoints = new LazyValue<float>(() => baseStats.GetStat(Stat.Health));
        }

        internal void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min(GetCurrentHealth() + healthToRestore, GetMaxHealth());
        }

        void Start()
        {
            healthPoints.ForceInit(); 
        }

        void OnEnable()
        {
            baseStats.onLevelUp += RegenerateHealth;
            onDie += playDieSound;
        }

        void OnDisable()
        {
            baseStats.onLevelUp -= RegenerateHealth;
            onDie -= playDieSound;
        }

        private void RegenerateHealth()
        {
            healthPoints.value = Mathf.Max(healthPoints.value, baseStats.GetStat(Stat.Health) * regenPercentage / 100);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " took damage " + damage);
            healthPoints.value = Mathf.Max(0, healthPoints.value - damage);
            takeDamage.Invoke(damage);
            onTakeDamage?.Invoke(damage);

            if (healthPoints.value == 0)
            {
                onDie.Invoke();
                AwardExperience(instigator);
                Die();
            }
        }

        private void AwardExperience(GameObject instigator)
        {
            float experience = baseStats.GetStat(Stat.ExperienceReward);
            Experience experienceComponent = instigator.GetComponent<Experience>();
            if (instigator != null && experienceComponent != null) experienceComponent.GainExperience(experience);
        }

        public float GetMaxHealth()
        {
            return baseStats.GetStat(Stat.Health);
        }

        public float GetCurrentHealth()
        {
            return healthPoints.value;
        }

        public float GetFraction()
        {
            return GetCurrentHealth() / GetMaxHealth();
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
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            if (healthPoints.value <= 0)
            {
                Die();
            }
        }

        void playDieSound() => dieSound.Play();
    }
}
