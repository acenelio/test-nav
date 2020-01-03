using UnityEngine;
using NavGame.Core;
using NavGame.Managers;

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
    public Character EnemyTarget;
    public Collectible PickupTarget;

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

            if (Physics.Raycast(ray, out hit, RayRange, EnemyLayer))
            {
                Character enemy = hit.collider.GetComponent<Character>();
                if (enemy != null)
                {
                    PickupTarget = null;
                    EnemyTarget = enemy;
                }
            }
            else if (Physics.Raycast(ray, out hit, RayRange, PickupLayer))
            {
                Collectible obj = hit.collider.GetComponent<Collectible>();
                if (obj != null)
                {
                    EnemyTarget = null;
                    PickupTarget = obj;
                }
            }
            else if (Physics.Raycast(ray, out hit, RayRange, WalkableLayer))
            {
                EnemyTarget = null;
                PickupTarget = null;
                locomotionController.MoveToPoint(hit.point);
            }
        }

        if (EnemyTarget != null)
        {
            locomotionController.MoveToCharacter(EnemyTarget);
            locomotionController.FaceObjectFrame(EnemyTarget.transform);
            float distance = Vector3.Distance(EnemyTarget.transform.position, transform.position);
            if (distance <= EnemyTarget.ContactRadius)
            {
                combatController.MeleeAttack(EnemyTarget);
            }
        }
        else if (PickupTarget != null)
        {
            locomotionController.MoveToCollectible(PickupTarget);
            float distance = Vector3.Distance(PickupTarget.transform.position, transform.position);
            if (distance <= PickupTarget.ContactRadius)
            {
                PickupTarget.Pickup();
                PickupTarget = null;
            }
        }
    }

    protected override void Die()
    {
        NavigationManager.instance.ReloadCurrentScene();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, ContactRadius);
    }
}
