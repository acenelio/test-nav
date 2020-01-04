using System.Collections;
using UnityEngine;
using NavGame.Managers;

namespace NavGame.Core
{
    public class RangedCombatController : MonoBehaviour
    {
        public string CastSound;
        public string ProjectHitSound;
        public Transform CastTransform;
        public GameObject ProjectilePrefab;
        public float CastDelay = 0.5f;
        public float CombatCooldown = 5f;
        public bool IsInCombat { get; private set; }
        Character Self;
        float AttackCooldown = 0f;
        float LastAttackTime;

        public OnRangedAttackStartEvent OnRangedAttackStart;
        public OnRangedAttackCastEvent OnRangedAttackCast;
        public OnRangedAttackHitEvent OnRangedAttackHit;
        public OnAttackCooldownUpdateEvent OnAttackCooldownUpdate;
        public OnEnterCombatEvent OnEnterCombat;
        public OnLeaveCombatEvent OnLeaveCombat;

        protected virtual void Awake()
        {
            Self = GetComponent<Character>();
        }

        protected virtual void Update()
        {
            DecreaseAttackCooldown();
            CheckCombatCooldown();
        }


        public void RangedAttack(Character target)
        {
            if (AttackCooldown <= 0f)
            {
                StartCoroutine(DoRangedAttackAfterDelay(target, CastDelay));
                AttackCooldown = 1f / Self.Stats.AttackSpeed;

                if (!IsInCombat)
                {
                    IsInCombat = true;
                    if (OnEnterCombat != null)
                    {
                        OnEnterCombat();
                    }
                }

                LastAttackTime = Time.time;

                if (OnRangedAttackStart != null)
                {
                    OnRangedAttackStart();
                }
            }
        }

        IEnumerator DoRangedAttackAfterDelay(Character targetCharacter, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (targetCharacter != null) // target may have died during delay
            {
                AudioManager.instance.Play(CastSound, CastTransform.position);
                Cast(targetCharacter);
                if (OnRangedAttackCast != null)
                {
                    OnRangedAttackCast();
                }
            }
        }

        void Cast(Character targetCharacter)
        {
            GameObject projectile = Instantiate(ProjectilePrefab, CastTransform.position, Quaternion.identity) as GameObject;
            ProjectileController controller = projectile.GetComponent<ProjectileController>();
            controller.Init(targetCharacter, Self.Stats.Damage, ProjectHitSound);
        }

        void DecreaseAttackCooldown()
        {
            if (AttackCooldown == 0f)
            {
                return;
            }
            AttackCooldown -= Time.deltaTime;
            if (AttackCooldown < 0f)
            {
                AttackCooldown = 0f;
            }
            if (OnAttackCooldownUpdate != null)
            {
                OnAttackCooldownUpdate(AttackCooldown);
            }
        }

        void CheckCombatCooldown()
        {
            if (IsInCombat)
            {
                if (Time.time - LastAttackTime > CombatCooldown)
                {
                    LeaveCombat();
                }
            }
        }

        public void LeaveCombat()
        {
            IsInCombat = false;
            if (OnLeaveCombat != null)
            {
                OnLeaveCombat();
            }
        }
    }
}
