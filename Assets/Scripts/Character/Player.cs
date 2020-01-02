using UnityEngine;
using NavGame.Character;

[RequireComponent(typeof(LocomotionController))]
[RequireComponent(typeof(MeleeCombatController))]
public class Player : Character
{
    public int RayRange = 1000;
    public float FacingAngle = 30f;

    public LayerMask WalkableLayer;
    public LayerMask EnemyLayer;
    public LayerMask PickupLayer;

    Camera Cam;
    public Character Target;

    bool IsFocused = false;

    LocomotionController locomotionController;
    MeleeCombatController combatController;

    void Start()
    {
        Cam = Camera.main;
        locomotionController = GetComponent<LocomotionController>();
        combatController = GetComponent<MeleeCombatController>();
        combatController.OnLeaveCombat += LoseFocus;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, RayRange, WalkableLayer))
            {
                Debug.Log("HIT Walkable " + hit.collider.name);
                locomotionController.StartMoveToPoint(hit.point);
            }

            if (Physics.Raycast(ray, out hit, RayRange, EnemyLayer))
            {
                Debug.Log("HIT Enemy " + hit.collider.name);
                Character enemy = hit.collider.GetComponent<Character>();
                if (enemy != null)
                {
                    FocusTarget(enemy);
                }
            }
        }

        if (IsFocused)
        {
            locomotionController.StartMoveToTarget(Target);
            locomotionController.FaceTarget();
            float distance = Vector3.Distance(Target.transform.position, transform.position);
            if (distance <= Target.ContactRadius) {
                combatController.MeleeAttack(Target);
            }
        }
    }

    void FocusTarget(Character target) {
        Target = target;
        IsFocused = true;
    }

    void LoseFocus() {
        Target = null;
        IsFocused = false;
    }

    bool FacingTarget(Character target) {
        Vector3 playerToTarget = target.transform.position - transform.position;
        return Vector3.Angle(gameObject.transform.forward, playerToTarget) <= FacingAngle;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, ContactRadius);
    }
}
