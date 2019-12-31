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

        protected virtual void LateUpdate() {
            //https://www.reddit.com/r/Unity3D/comments/7yww7e/navmesh_agent_cant_reach_a_goal_and_spins_around/
            //https://answers.unity.com/questions/1086857/navmeshagent-spins-on-the-spot-when-close-to-desti.html
            if (Agent.hasPath && (Agent.steeringTarget == Agent.destination)) {
                //Debug.Log ("Has path and steering target is destination");
                if (Mathf.Approximately (Agent.velocity.magnitude, 0))
                {
                    //Debug.Log ("Velocity was zero, so using desired velocity (max speed)");
                    Agent.velocity = Agent.desiredVelocity;
                }
                else
                {
                    //Debug.Log ("Velocity was non-zero, so using desired velocity direction with velocity magnitude");
                    Agent.velocity = (Agent.desiredVelocity.normalized * Agent.velocity.magnitude);
                }
            }
        }

        public void MoveToPoint(Vector3 point) {
            Agent.SetDestination(point); 
        }    
    }
}
