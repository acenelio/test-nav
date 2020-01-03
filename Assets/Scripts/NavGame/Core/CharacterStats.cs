using System;

namespace NavGame.Core
{
    [Serializable]
    public class CharacterStats {
        public int MaxHealth = 100;

        public int Damage = 10;
        public int Armor = 1;

        public float AttackSpeed = 1f;

        public float MeleeRange = 1f;
        public float RangedRange = 5f;
    }
}
