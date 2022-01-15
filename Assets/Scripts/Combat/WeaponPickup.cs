using System.Collections;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon;
        [SerializeField] float healthToRestore = 0;
        [SerializeField] float respawnTime = 5;

        void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            Pickup(other.gameObject);
        }

        private void Pickup(GameObject subject)
        {
            if (weapon != null) subject.GetComponent<Fighter>().EquipWeapon(weapon);
            if (healthToRestore > 0) subject.GetComponent<Health>().Heal(healthToRestore);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            foreach (Transform child in transform) child.gameObject.SetActive(shouldShow);
            GetComponent<Collider>().enabled = shouldShow;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.gameObject);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
