using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavGame.Core
{
    // CHARACTER EVENTS
    public delegate void OnHealthChangedEvent(int maxHealth, int currentHealth);
    public delegate void OnDiedEvent();


    // COMBAT EVENTS
    public delegate void OnMeleeAttackStartEvent();
    public delegate void OnMeleeAttackHitEvent();
    public delegate void OnRangedAttackStartEvent();
    public delegate void OnRangedAttackCastEvent();
    public delegate void OnRangedAttackHitEvent();
    public delegate void OnAttackCooldownUpdateEvent(float remainingCooldown);
    public delegate void OnEnterCombatEvent();
    public delegate void OnLeaveCombatEvent();

    // COLLECTIBLE EVENTS
    public delegate void OnPickupEvent(Collectible collectible);

    // MISC
    public delegate void OnCharacterSavedEvent(Character character);
}
