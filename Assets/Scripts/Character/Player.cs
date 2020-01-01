using UnityEngine;
using NavGame.Character;

public class Player : Character, IReachable
{
    [SerializeField]
    float contactRadius = 1.5f;

    public float ContactRadius()
    {
        return contactRadius;
    }

    public GameObject ToGameObject()
    {
        return this.gameObject;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, ContactRadius());
    }
}
