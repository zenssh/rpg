using System;
using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;

        void Awake()
        {
            GameObject player = GameObject.FindWithTag("Player");
            health = player.GetComponent<Health>();
        }

        void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", health.GetCurrentHealth(), health.GetMaxHealth());
        }
    }
}
