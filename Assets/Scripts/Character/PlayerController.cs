using UnityEngine;
using UnityEngine.AI;
using NavGame.Character;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : BasicMotionController
{
    public int RayRange = 1000;

    Camera Cam;

    void Start()
    {
        Cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, RayRange)) {
                Debug.Log("HIT " + hit.collider.gameObject.name);
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Walkable")) {
                    MoveToPoint(hit.point);
                }
            }
        }
    }
}
