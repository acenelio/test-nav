using UnityEngine;
using NavGame.Core;
using NavGame.Managers;

[RequireComponent(typeof(LocomotionController))]
public class KamikazeEnemy : Character
{
    public float Tolerance = 0.2f;
    public GameObject DieParticlesPrefab;

    public string DieAudio;

    Character Target;
    Transform SaveTransform;
    Vector3 FinalPoint;

    LocomotionController locomotionController;

    public OnCharacterSavedEvent OnCharacterSaved;

    void Start()
    {
        //Target = PlayerManager.instance.GetPlayer().GetComponent<PlayerRanged>();
        locomotionController = GetComponent<LocomotionController>();
        if (FinalPoint != null)
        {
            locomotionController.MoveToPoint(FinalPoint);
        }
        else
        {
            Debug.LogError("FinalPoint should have been init");
        }

        KamikazeContact kamikazeContact = GetComponentInChildren<KamikazeContact>();
        kamikazeContact.OnReachBossArea += Die;
    }

    void Update()
    {
        if (Target == null)
        {
            return;
        }

        float distance = Vector3.Distance(Target.transform.position, transform.position);

        locomotionController.MoveToCharacter(Target);
        locomotionController.FaceObjectFrame(Target.transform);
        if (distance <= Target.ContactRadius + Tolerance)
        {
            Target.TakeDamage(Stats.Damage);
            Die();
        }
    }

    public void Init(Vector3 finalPoint)
    {
        FinalPoint = finalPoint;
    }

    protected override void Die()
    {
        base.Die();
        AudioManager.instance.Play(DieAudio, transform.position);
        Instantiate(DieParticlesPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Friend")
        {
            Target = other.gameObject.GetComponent<Character>();
        }
    }
}
