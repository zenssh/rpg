using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Transform foreground;
        [SerializeField] Canvas canvas;
        [SerializeField] Health health;

        private void Start()
        {
            UpdateHealthBar();
        }

        private void OnEnable() 
        {
            health.onTakeDamage += UpdateHealthBarEventAction;
            health.onDie += Disable;
        }

        private void OnDisable()
        {
            health.onDie -= Disable;
            health.onTakeDamage -= UpdateHealthBarEventAction;
        }

        void UpdateHealthBar()
        {
            foreground.localScale = new Vector3(
                health.GetFraction(),
                foreground.localScale.y,
                foreground.localScale.z
            );
        }

        void UpdateHealthBarEventAction(float damage) => UpdateHealthBar();

        public void Enable() => canvas.enabled = true;

        public void Disable() => canvas.enabled = false;
    }

}