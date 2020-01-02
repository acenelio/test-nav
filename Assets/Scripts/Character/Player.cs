using UnityEngine;
using NavGame.Character;

public class Player : Character
{
    public int RayRange = 1000;

    public LayerMask WalkableLayer;
    public LayerMask EnemyLayer;
    public LayerMask PickupLayer;

    Camera Cam;
    Character Target;

    LocomotionController locomotionController;

    void Start()
    {
        Cam = Camera.main;
        locomotionController = GetComponent<LocomotionController>();
        //locomotionController.OnStartMoveToTarget += SetTarget;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, RayRange, WalkableLayer))
            {
                locomotionController.StartMoveToPoint(hit.point);
            }

            if (Physics.Raycast(ray, out hit, RayRange, EnemyLayer))
            {
                Character enemy = hit.collider.GetComponent<Character>();
                //if (interactable.Distance(this)) {
                //    RangeAttack(interactable);
                //}
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, ContactRadius);
    }
}
