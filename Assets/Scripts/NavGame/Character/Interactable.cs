using UnityEngine;

namespace NavGame.Character
{
    public abstract class Interactable : MonoBehaviour
    {
        public float InteractRadius = 2f;

        public abstract void Interact(GameObject other);


        void OnDrawGizmosSelected ()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, InteractRadius);
        }
    }
}
