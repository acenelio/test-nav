using UnityEngine;
using NavGame.Core;
using NavGame.Managers;

[RequireComponent(typeof(LocomotionController))]
[RequireComponent(typeof(RangedCombatController))]
public class PlayerRanged : Character
{
    public int RayRange = 1000;
    public float Tolerance = 0.2f;
    public float FacingAngle = 30f;
    public int Ammo = 0;

    public LayerMask WalkableLayer;
    public LayerMask EnemyLayer;
    public LayerMask PickupLayer;

    Camera Cam;
    public Character EnemyTarget;
    public Collectible PickupTarget;

    LocomotionController locomotionController;
    RangedCombatController combatController;

    void Start()
    {
        Cam = Camera.main;
        locomotionController = GetComponent<LocomotionController>();
        combatController = GetComponent<RangedCombatController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
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

        if (EnemyTarget != null && EnemyTarget.IsDead == false)
        {
            locomotionController.MoveToCharacter(EnemyTarget);
            locomotionController.FaceObjectFrame(EnemyTarget.transform);
            float distance = Vector3.Distance(EnemyTarget.transform.position, transform.position);
            if (distance <= EnemyTarget.ContactRadius + Stats.RangedRange + Tolerance)
            {
                if (Ammo > 0)
                {
                    locomotionController.CancelMove();
                    combatController.RangedAttack(EnemyTarget);
                }
                else
                {
                    AudioManager.instance.Play("Error1", transform.position);
                    LevelManager.instance.ShowWarning("Out of books", 2f);
                    EnemyTarget = null;
                }
            }
        }
        else if (PickupTarget != null)
        {
            locomotionController.MoveToCollectible(PickupTarget);
            locomotionController.FaceObjectFrame(PickupTarget.transform);
            float distance = Vector3.Distance(PickupTarget.transform.position, transform.position);
            if (distance <= PickupTarget.ContactRadius + Tolerance)
            {
                PickupTarget.Pickup();
                PickupTarget = null;
            }
        }
    }

    protected override void Die()
    {
        base.Die();
        NavigationManager.instance.ReloadCurrentScene();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, ContactRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Stats.RangedRange);
    }
}
