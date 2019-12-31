using UnityEngine;

namespace NavGame.Character
{
    public delegate void OnHealthChangedCallBack(int maxHealth, int currentHealth);

    public delegate void OnDiedCallBack();

    public class Character : MonoBehaviour
    {
        public CharacterStats Stats;
        public int CurrentHealth { get; private set; }
        public OnHealthChangedCallBack OnHealthChanged;
        public OnDiedCallBack OnDied;

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
                if (OnDied != null) {
                    OnDied();
                }
            }
        }
    }
}
