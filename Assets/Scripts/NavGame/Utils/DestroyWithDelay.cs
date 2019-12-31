using UnityEngine;

namespace NavGame.Utils
{
    public class DestroyWithDelay : MonoBehaviour
    {
        public float delay = 2f;

        void Start()
        {
            Destroy(gameObject, delay);
        }
    }
}
