using UnityEngine;
using NavGame.Character;

[RequireComponent(typeof(BasicMotionController))]
public class PlayerInputController : MonoBehaviour
{
    public int RayRange = 1000;

    public LayerMask WalkableLayer;
    public LayerMask EnemyLayer;
    public LayerMask PickupLayer;

    Camera Cam;

    BasicMotionController motionController;

    void Start()
    {
        Cam = Camera.main;
        motionController = GetComponent<BasicMotionController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, RayRange, WalkableLayer)) {
                motionController.StartMoveToPoint(hit.point);
            }

            if (Physics.Raycast(ray, out hit, RayRange, EnemyLayer)) {
                //Interactable interactable = hit.collider.GetComponent<Interactable>();
                //if (interactable.Distance(this)) {
                //    RangeAttack(interactable);
                //}
            }
        }
    }   
}
