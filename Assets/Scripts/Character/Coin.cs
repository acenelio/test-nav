using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavGame.Core;
using NavGame.Managers;

public class Coin : Collectible
{
    public string PickupSound;

    protected override void DoActionOnPickup() {
        AudioManager.instance.Play(PickupSound, transform.position);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, ContactRadius);
    }
}
