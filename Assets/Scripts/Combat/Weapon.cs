using System;
using UnityEngine;

namespace RPG.Combat
{   
    public class Weapon : MonoBehaviour
    {
        [SerializeField] AudioSource hitSound;

        public void OnHit()
        {
            if (hitSound != null) hitSound.Play();
        }
    }
}
