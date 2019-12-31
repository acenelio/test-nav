using UnityEngine;
using UnityEngine.AI;

namespace NavGame.Character 
{

    [RequireComponent(typeof(NavMeshAgent))]
    public class BasicMotionController : MonoBehaviour
    {
        public float AngularSpeed = 5f;

        protected NavMeshAgent Agent;

        void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();   
        }

        public void MoveToPoint(Vector3 point) {
            Agent.SetDestination(point); 
        }    
    }
}
