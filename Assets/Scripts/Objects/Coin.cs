using UnityEngine;
using NavGame.Character;

public class Coin : Interactable
{
    public override void Interact(GameObject other) {
        Debug.Log(gameObject.name + " interacted with " + other.name);
    }
}
