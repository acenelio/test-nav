using UnityEngine;
using UnityEngine.AI;

namespace NavGame.Character
{
    public delegate void OnStartMoveToPointCallBack(Vector3 point);
    public delegate void OnStartMoveToTargetCallBack(IReachable target);
    public delegate void OnReachTargetCallBack(IReachable target);
    public delegate void OnReachDestinationCallBack();
    public delegate void OnCancelMoveCallBack();

    [RequireComponent(typeof(NavMeshAgent))]
    public class BasicMotionController : MonoBehaviour
    {
        protected NavMeshAgent Agent;

        public IReachable Target { get; private set; }

        public OnStartMoveToPointCallBack OnStartMoveToPoint;
        public OnStartMoveToTargetCallBack OnStartMoveToTarget;
        public OnReachTargetCallBack OnReachTarget;
        public OnReachDestinationCallBack OnReachDestination;
        public OnCancelMoveCallBack OnCancelMove;

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

        public void StartMoveToTarget(IReachable target)
        {
            CancelMove();
            Target = target;
            Agent.stoppingDistance = target.ContactRadius();
            Agent.SetDestination(target.ToGameObject().transform.position);
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
        void FixDestinationSpinningBug() {
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
    }
}
