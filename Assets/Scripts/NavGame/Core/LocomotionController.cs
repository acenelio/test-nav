using UnityEngine;
using UnityEngine.AI;

namespace NavGame.Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class LocomotionController : MonoBehaviour
    {
        protected NavMeshAgent Agent;
        public float AngularSpeed = 5f;

        protected virtual void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
        }

        public void CancelMove()
        {
            Agent.isStopped = true;
            Agent.ResetPath();
            Agent.stoppingDistance = 0f;
        }

        public void MoveToCharacter(Character character)
        {
            Agent.stoppingDistance = character.ContactRadius;
            Agent.SetDestination(character.transform.position);
        }

        public void MoveToCollectible(Collectible collectible)
        {
            Agent.stoppingDistance = collectible.ContactRadius;
            Agent.SetDestination(collectible.transform.position);
        }

        public void MoveToPoint(Vector3 point)
        {
            Agent.stoppingDistance = 0f;
            Agent.SetDestination(point);
        }

        public void FaceObjectFrame(Transform destination)
        {
            Vector3 direction = (destination.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * AngularSpeed);
        }

        public void FaceObjectNow(Transform destination)
        {
            Vector3 direction = (destination.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = lookRotation;
        }
    }
}
