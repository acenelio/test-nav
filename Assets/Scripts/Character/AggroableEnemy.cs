using UnityEngine;
using NavGame.Core;

[RequireComponent(typeof(LocomotionController))]
[RequireComponent(typeof(MeleeCombatController))]
public class AggroableEnemy : Character
{
    public float AggroRadius = 6f;

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
            locomotionController.MoveToCharacter(Target);
            locomotionController.FaceObjectFrame(Target.transform);
            if (distance <= Target.ContactRadius) {
                combatController.MeleeAttack(Target);
            }
        }
        else
        {
            locomotionController.CancelMove();
        }
    }

    protected override void Die() {
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AggroRadius);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, ContactRadius);
    }
}
