using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats stats;

        void Awake()
        {
            stats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        void Update()
        {
            GetComponent<Text>().text = String.Format(
                "{0:0}",
                stats.GetLevel()
            );
        }
    }
}
