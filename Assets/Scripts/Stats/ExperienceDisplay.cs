using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;
        BaseStats stats;

        void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            stats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        void Update()
        {
            GetComponent<Text>().text = String.Format(
                "{0:0}",
                experience.GetCurrentXP()
            );
        }
    }
}
