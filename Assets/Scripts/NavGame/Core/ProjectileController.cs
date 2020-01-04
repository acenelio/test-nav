using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavGame.Managers;

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
        string ProjectHitSound;
        bool IsInit = false;

        public GameObject HitParticles;

        void Start()
        {
            if (!IsInit)
            {
                Debug.LogError("ProjectileController Init was not called. Destroying GameObject");
                Destroy(gameObject);
            }
        }

        void Update()
        {
            if (Target == null) // target may have died for another cause
            {
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
            if (Target == null) // target may have died for another cause
            {
                Destroy(gameObject);
                return;
            }
            if (other.gameObject == Target.gameObject)
            {
                Character targetCharacter = other.gameObject.GetComponent<Character>();
                if (targetCharacter != null)
                {
                    targetCharacter.TakeDamage(Damage);
                }
                AudioManager.instance.Play(ProjectHitSound, other.gameObject.transform.position);
                Instantiate(HitParticles, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }

        public void Init(Character target, int damage, string projectHitSound)
        {
            Target = target;
            TargetHit = target.gameObject.transform.Find("HitPoint");
            if (TargetHit == null)
            {
                TargetHit = target.gameObject.transform;
            }
            Damage = damage;
            ProjectHitSound = projectHitSound;
            IsInit = true;
        }

        void FaceTargetFrame()
        {
            Vector3 direction = (Target.gameObject.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * AngularSpeed);
        }
    }
}
