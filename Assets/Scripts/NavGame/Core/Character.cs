﻿using UnityEngine;

namespace NavGame.Core
{
    public abstract class Character : MonoBehaviour
    {
        public CharacterStats Stats;
        public float ContactRadius = 1.5f;
        public int CurrentHealth;

        public OnHealthChangedEvent OnHealthChanged;
        public OnDiedEvent OnDied;

        public bool IsDead { get; private set; } = false;

        void Awake()
        {
            CurrentHealth = Stats.MaxHealth;
        }

        public void TakeDamage(int damage)
        {

            if (IsDead)
            {
                return;
            }

            damage -= Stats.Armor;
            damage = Mathf.Clamp(damage, 1, int.MaxValue);

            CurrentHealth -= damage;

            if (OnHealthChanged != null)
            {
                OnHealthChanged(Stats.MaxHealth, CurrentHealth);
            }

            if (CurrentHealth <= 0)
            {
                IsDead = true;
                Die();
            }
        }

        protected virtual void Die()
        {
            if (OnDied != null)
            {
                OnDied();
            }
        }
    }
}
