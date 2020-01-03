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

    LocomotionController locomotionController;
    MeleeCombatController combatController;

    void Start()
    {
        Cam = Camera.main;
        locomotionController = GetComponent<LocomotionController>();
        combatController = GetComponent<MeleeCombatController>();
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
                Target = null;
                locomotionController.MoveToPoint(hit.point);
            }

            if (Physics.Raycast(ray, out hit, RayRange, EnemyLayer))
            {
                Debug.Log("HIT Enemy " + hit.collider.name);
                Character enemy = hit.collider.GetComponent<Character>();
                if (enemy != null)
                {
                    Target = enemy;
                }
            }
        }

        if (Target != null) {
            locomotionController.MoveToCharacter(Target);
            locomotionController.FaceObjectFrame(Target.transform);
            float distance = Vector3.Distance(Target.transform.position, transform.position);
            if (distance <= Target.ContactRadius) {
                combatController.MeleeAttack(Target);
            }
        }
    }

    protected override void Die() {
        NavigationManager.instance.ReloadCurrentScene();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, ContactRadius);
    }
}
