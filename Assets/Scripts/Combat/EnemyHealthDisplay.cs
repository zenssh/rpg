using System;
using RPG.Attributes;
using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] Fighter fighter;

        void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        void Update()
        {
            Health health = fighter.GetTarget();
            string text;
            if (health != null)
            {
                text = String.Format("{0:0}/{1:0}", health.GetCurrentHealth(), health.GetMaxHealth());
            }
            else
            {
                text = String.Format("-");
            }
            GetComponent<Text>().text = text;   
        }
    }
}
