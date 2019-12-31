using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public int RayRange = 1000;

    Camera Cam;

    NavMeshAgent Agent;

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>(); 
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
                    Agent.SetDestination(hit.point); 
                }
            }
        }
    }
}
