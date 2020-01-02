using System.Collections;
using UnityEngine;

namespace NavGame.Character
{
    /// <summary>
    /// This class serves a Character, providing MeleeAttack(target) funcionality, and 
    /// internally handling attack and combat cooldowns, as well as IsInCombat state  
    /// </summary>
    [RequireComponent(typeof(Character))]
    public class MeleeCombatController : MonoBehaviour
    {
        public string HitSound;
        public Transform PunchPoint;
        public GameObject PunchEffectPrefab;
        public float PunchHitDelay = 0.5f;
        public float CombatCooldown = 5f;
        public bool IsInCombat { get; private set; }
        Character Self;
        float AttackCooldown = 0f;
        float LastAttackTime;

        public OnMeleeAttackStartEvent OnMeleeAttackStart;
        public OnMeleeAttackHitEvent OnMeleeAttackHit;
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

        public void MeleeAttack(Character target)
        {
            if (AttackCooldown <= 0f)
            {
                StartCoroutine(DoMeleeAttackAfterDelay(target, PunchHitDelay));
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

                if (OnMeleeAttackStart != null)
                {
                    OnMeleeAttackStart();
                }
            }
        }

        IEnumerator DoMeleeAttackAfterDelay(Character targetCharacter, float delay)
        {
            yield return new WaitForSeconds(delay);
            Strike(targetCharacter);
        }

        void Strike(Character targetCharacter)
        {
            if (targetCharacter != null) // target may have died during o delay
            {

                targetCharacter.TakeDamage(Self.Stats.Damage);

                AudioManager.instance.Play(HitSound, transform.position);

                if (PunchPoint != null && PunchEffectPrefab != null)
                {
                    Instantiate(PunchEffectPrefab, PunchPoint.position, Quaternion.identity);
                }

                if (OnMeleeAttackHit != null)
                {
                    OnMeleeAttackHit();
                }

                if (targetCharacter.CurrentHealth <= 0)
                {
                    LeaveCombat();
                }
            }
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
            if (Time.time - LastAttackTime > CombatCooldown)
            {
                LeaveCombat();
            }
        }

        void LeaveCombat()
        {
            IsInCombat = false;
            if (OnLeaveCombat != null)
            {
                OnLeaveCombat();
            }
        }
    }
}
