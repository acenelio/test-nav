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
            if (OnPickup != null) {
                OnPickup(this);
            }
            DoActionOnPickup();
        }

        protected abstract void DoActionOnPickup();
    }
}
