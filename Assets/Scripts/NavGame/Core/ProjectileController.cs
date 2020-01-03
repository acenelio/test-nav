using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavGame.Core
{
    public class ProjectileController : MonoBehaviour
    {
        public int Damage = 10;
        public float Speed = 8f;
        public float AngularSpeed = 5f;
        public bool FaceTarget = false;
        public Character Target;
        Transform TargetHit;

        public GameObject HitParticles;

        void Update()
        {
            if (Target == null)
            {
                Debug.LogError("Projectile Target not set");
                Destroy(gameObject);
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, TargetHit.position, Speed * Time.deltaTime);
            if (FaceTarget)
            {
                FaceTargetFrame();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("ENTER " + other.gameObject);
            if (other.gameObject == Target.gameObject)
            {
                Character targetCharacter = other.gameObject.GetComponent<Character>();
                if (targetCharacter != null)
                {
                    targetCharacter.TakeDamage(Damage);
                }
                Instantiate(HitParticles, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }

        public void SetTarget(Character target)
        {
            Target = target;
            TargetHit = target.gameObject.transform.Find("HitPoint");
            if (TargetHit == null)
            {
                TargetHit = target.gameObject.transform;
            }
        }

        public void SetDamage(int damage)
        {
            Damage = damage;
        }

        void FaceTargetFrame()
        {
            Vector3 direction = (Target.gameObject.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * AngularSpeed);
        }
    }
}
