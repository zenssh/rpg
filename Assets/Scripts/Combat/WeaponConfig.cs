using RPG.Attributes;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] float damage = 5f;
        [SerializeField] float damageBonus = 0f;
        [SerializeField] float range = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            Weapon weapon = null;
            if (equippedPrefab != null)
            {
                weapon = Instantiate(equippedPrefab, isRightHanded ? rightHand : leftHand);
                weapon.gameObject.name = weaponName;
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null) animator.runtimeAnimatorController = animatorOverride;
            else if (overrideController != null) animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            return weapon;
        }

        public void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null) oldWeapon = leftHand.Find(weaponName);
            if (oldWeapon == null) return;
            
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile,
                                                        isRightHanded ? rightHand.position : leftHand.position,
                                                        Quaternion.identity);
            // projectileInstance.SetTarget(target, instigator, damage);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public bool HasProjectile() => projectile != null;

        // public Projectile GetProjectile() => projectile;

        public float GetDamage() => damage;

        public float GetDamageBonus() => damageBonus;

        public float GetRange() => range;

        public float GetTimeBetweenAttacks() => timeBetweenAttacks;
        
    }
}