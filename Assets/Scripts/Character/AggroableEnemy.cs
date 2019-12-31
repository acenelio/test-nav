using UnityEngine;
using NavGame.Character;

[RequireComponent(typeof(BasicMotionController))]
public class AggroableEnemy : Character, INavigable
{
    [SerializeField]
    float contactRadius = 1.5f;
    public float AggroRadius = 6f;

    public float AngularSpeed = 5f;

    INavigable Target;

    BasicMotionController motionController;

    void Start()
    {
        Target = PlayerManager.instance.GetPlayer().GetComponent<Player>();
        motionController = GetComponent<BasicMotionController>();
    }

    void Update()
    {
        if (Target == null)
        {
            return;
        }

        float distance = Vector3.Distance(Target.ToGameObject().transform.position, transform.position);

        if (distance <= AggroRadius)
        {
            motionController.StartMoveToTarget(Target);
            FaceTarget();
        }
        else
        {
            motionController.CancelMove();
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (Target.ToGameObject().transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * AngularSpeed);
    }

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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AggroRadius);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, ContactRadius());
    }
}
