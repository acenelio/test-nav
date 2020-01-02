using UnityEngine;
using NavGame.Character;

[RequireComponent(typeof(LocomotionController))]
[RequireComponent(typeof(MeleeCombatController))]
public class AggroableEnemy : Character
{
    public float AggroRadius = 6f;

    public float AngularSpeed = 5f;

    Character Target;

    LocomotionController locomotionController;
    MeleeCombatController combatController;

    void Start()
    {
        Target = PlayerManager.instance.GetPlayer().GetComponent<Player>();
        locomotionController = GetComponent<LocomotionController>();
        combatController = GetComponent<MeleeCombatController>();
    }

    void Update()
    {
        if (Target == null)
        {
            return;
        }

        float distance = Vector3.Distance(Target.transform.position, transform.position);

        if (distance <= AggroRadius)
        {
            locomotionController.StartMoveToTarget(Target);
            if (distance <= Target.ContactRadius) {
                combatController.MeleeAttack(Target);
            }
            locomotionController.FaceTarget();
        }
        else
        {
            locomotionController.CancelMove();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AggroRadius);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, ContactRadius);
    }
}
