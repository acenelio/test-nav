using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NavGame.Core
{
    [RequireComponent(typeof(RangedCombatController))]
    public class RangedCombatAnimator : LocomotionAnimator
    {
        RangedCombatController combatController;

        protected override void Start()
        {
            base.Start();
            combatController = GetComponent<RangedCombatController>();
            combatController.OnRangedAttackStart += TriggerRangedAttack;
        }

        protected override void Update()
        {
            base.Update();
            animator.SetBool("isInCombat", combatController.IsInCombat);
        }

        void TriggerRangedAttack()
        {
            animator.SetTrigger("tgRangedAttack");
        }
    }
}
