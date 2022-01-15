using System;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] bool isHoming = false;
        [SerializeField] float speed = 1f;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10f;
        [SerializeField] float lifeAfterImpact = 2f;
        [SerializeField] GameObject[] destroyOnHit = null;
        Health target;
        GameObject instigator;
        float damage = 0;

        [SerializeField] AudioSource projectileHitSound;

        void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (target == null || speed <= 0) return;
            if (isHoming && !target.IsDead()) transform.LookAt(GetAimLocation());
            // Vector3 translation = (transform.position - target.position);
            // translation.Normalize();
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject istigator, float damage)
        {
            this.target = target;
            this.instigator = istigator;
            this.damage = damage;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider capsule = target.GetComponent<CapsuleCollider>();
            // if (target.IsDead())
            // {
            //     return target.transform.position * 2;
            // }
            if (capsule == null) return target.transform.position;
            return target.transform.position + Vector3.up * capsule.height/2;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;
            target.TakeDamage(instigator, damage);
            
            projectileHitSound.Play();
            
            speed = 0;

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnHit) Destroy(toDestroy);

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}