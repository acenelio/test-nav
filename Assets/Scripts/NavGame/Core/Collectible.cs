using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavGame.Core
{
    public abstract class Collectible : MonoBehaviour
    {
        public float ContactRadius = 1.5f;
        public OnPickupEvent OnPickup;

        public void Pickup() {
            DoActionOnPickup();
            if (OnPickup != null) {
                OnPickup();
            }
        }

        protected abstract void DoActionOnPickup();
    }
}
