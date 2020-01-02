using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavGame.Character
{
    // CHARACTER EVENTS
    public delegate void OnHealthChangedEvent(int maxHealth, int currentHealth);
    public delegate void OnDiedEvent();

    // LOCOMOTION EVENTS
    public delegate void OnStartMoveToPointEvent(Vector3 point);
    public delegate void OnStartMoveToTargetEvent(Character target);
    public delegate void OnReachTargetEvent(Character target);
    public delegate void OnReachDestinationEvent();
    public delegate void OnCancelMoveEvent();

    // COMBAT EVENTS
    public delegate void OnMeleeAttackStartEvent();
    public delegate void OnMeleeAttackHitEvent();
    public delegate void OnAttackCooldownUpdateEvent(float remainingCooldown);
    public delegate void OnEnterCombatEvent();
    public delegate void OnLeaveCombatEvent();
}
