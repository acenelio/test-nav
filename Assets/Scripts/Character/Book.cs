using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavGame.Core;
using NavGame.Managers;

public class Book : Collectible
{
    public string PickupSound;
    public GameObject ParticlesPrefab;

    protected override void DoActionOnPickup() {
        AudioManager.instance.Play(PickupSound, transform.position);
        Instantiate(ParticlesPrefab, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, ContactRadius);
    }
}
