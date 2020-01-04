using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NavGame.Core
{
    [RequireComponent(typeof(MeleeCombatController))]
    public class MeleeCombatAnimator : LocomotionAnimator
    {
        MeleeCombatController combatController;

        protected override void Start()
        {
            base.Start();
            combatController = GetComponent<MeleeCombatController>();
            combatController.OnMeleeAttackStart += TriggerMeleeAttack;
        }

        protected override void Update()
        {
            base.Update();
            animator.SetBool("isInCombat", combatController.IsInCombat);
        }

        void TriggerMeleeAttack()
        {
            animator.SetTrigger("tgMeleeAttack");
        }
    }
}
