using UnityEngine;
using UnityEngine.AI;

namespace NavGame.Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class LocomotionController : MonoBehaviour
    {
        protected NavMeshAgent Agent;
        public float AngularSpeed = 5f;

        public OnStartMoveToPointEvent OnStartMoveToPoint;
        public OnStartMoveToTargetEvent OnStartMoveToTarget;
        public OnReachTargetEvent OnReachTarget;
        public OnReachDestinationEvent OnReachDestination;
        public OnCancelMoveEvent OnCancelMove;

        Character Target;

        protected virtual void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
        }

        protected virtual void LateUpdate()
        {
            if (ReachedDestination())
            {
                if (OnReachDestination != null)
                {
                    OnReachDestination();
                }

                if (Target != null)
                {
                    if (OnReachTarget != null)
                    {
                        OnReachTarget(Target);
                    }
                }

                CancelMove();
            }

            //FixDestinationSpinningBug(); // weird behaviour when pushing another character
        }

        public void CancelMove()
        {
            Target = null;
            Agent.isStopped = true;
            Agent.ResetPath();
            Agent.stoppingDistance = 0f;
            if (OnCancelMove != null)
            {
                OnCancelMove();
            }
        }

        public void StartMoveToTarget(Character target)
        {
            CancelMove();
            Target = target;
            Agent.stoppingDistance = target.ContactRadius;
            Agent.SetDestination(target.transform.position);
            if (OnStartMoveToTarget != null)
            {
                OnStartMoveToTarget(target);
            }
        }

        public void StartMoveToPoint(Vector3 point)
        {
            CancelMove();
            Agent.SetDestination(point);
            if (OnStartMoveToPoint != null)
            {
                OnStartMoveToPoint(point);
            }
        }

        //https://answers.unity.com/questions/324589/how-can-i-tell-when-a-navmesh-has-reached-its-dest.html
        bool ReachedDestination()
        {
            if (!Agent.pathPending)
            {
                if (Agent.remainingDistance <= Agent.stoppingDistance)
                {
                    if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //https://www.reddit.com/r/Unity3D/comments/7yww7e/navmesh_agent_cant_reach_a_goal_and_spins_around/
        //https://answers.unity.com/questions/1086857/navmeshagent-spins-on-the-spot-when-close-to-desti.html
        void FixDestinationSpinningBug()
        {
            if (Agent.hasPath && (Agent.steeringTarget == Agent.destination))
            {
                //Debug.Log ("Has path and steering target is destination");
                if (Mathf.Approximately(Agent.velocity.magnitude, 0))
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

        public void FaceTarget()
        {
            if (Target == null) {
                return;
            }
            Vector3 direction = (Target.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * AngularSpeed);
        }
    }
}
