using UnityEngine;

namespace NavGame.Character
{
    public abstract class Character : MonoBehaviour
    {
        public CharacterStats Stats;
        public float ContactRadius = 1.5f;
        public int CurrentHealth;
        
        public OnHealthChangedEvent OnHealthChanged;
        public OnDiedEvent OnDied;

        void Awake () {
            CurrentHealth = Stats.MaxHealth;
        }

        public void TakeDamage(int damage) {

            damage -= Stats.Armor;
            damage = Mathf.Clamp(damage, 1, int.MaxValue);

            CurrentHealth -= damage;
            Debug.Log(transform.name + " takes " + damage + " damage");

            if (OnHealthChanged != null) {
                OnHealthChanged(Stats.MaxHealth, CurrentHealth);
            }

            if (CurrentHealth <= 0) {
                Die();
                if (OnDied != null) {
                    OnDied();
                }
            }
        }

        protected abstract void Die();
    }
}
