using UnityEngine;
using NavGame.Core;
using NavGame.Managers;

[RequireComponent(typeof(LocomotionController))]
[RequireComponent(typeof(MeleeCombatController))]
public class AggroableEnemy : Character
{
    public float AggroRadius = 6f;
    public GameObject SavedParticles;

    public string PickupAudio;

    Character Target;
    Transform SaveTransform;

    LocomotionController locomotionController;
    MeleeCombatController combatController;

    bool IsSaved = false;

    public OnCharacterSavedEvent OnCharacterSaved;

    void Start()
    {
        Target = PlayerManager.instance.GetPlayer().GetComponent<PlayerRanged>();
        locomotionController = GetComponent<LocomotionController>();
        combatController = GetComponent<MeleeCombatController>();

        GameObject bus = GameObject.Find("Bus");
        if (bus == null)
        {
            Debug.LogError("AggroableEnemy could not find Bus object. Destroying gameObject");
            Destroy(gameObject);
        }
        SaveTransform = bus.transform;
    }

    void Update()
    {
        if (IsSaved || Target == null)
        {
            return;
        }

        float distance = Vector3.Distance(Target.transform.position, transform.position);

        if (distance <= AggroRadius)
        {
            locomotionController.MoveToCharacter(Target);
            locomotionController.FaceObjectFrame(Target.transform);
            if (distance <= Target.ContactRadius)
            {
                combatController.MeleeAttack(Target);
            }
        }
        else
        {
            locomotionController.CancelMove();
        }
    }

    protected override void Die()
    {
        IsSaved = true;
        locomotionController.MoveToPoint(SaveTransform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bus")
        {
            AudioManager.instance.Play(PickupAudio, transform.position);
            Instantiate(SavedParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
            if (OnCharacterSaved != null)
            {
                OnCharacterSaved();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AggroRadius);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, ContactRadius);
    }
}
