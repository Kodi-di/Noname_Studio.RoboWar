using UnityEngine;
using Extensions;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float TimeToDestruct = 10;
    public bool RandomDamage = true;
    public float Damage = 20;
    public float minRandLimit = -5;
    public float maxRandLimit = 5;
    public bool DamageReduction = true;
    public float StartPoinOfDamageReduction = 20;
    public float FinalDamageInPercent = 20;
    public AnimationCurve DamageReductionGraph;
    public int StartSpeed = 500;
    public GameObject particleHit;
    private Rigidbody _rigidbody;

    Vector3 PreviousStep;
    float StartTime;
    float CurrentDamage;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Invoke("DestroyNow", TimeToDestruct);

        _rigidbody.velocity = transform.TransformDirection(Vector3.left * StartSpeed);

        PreviousStep = gameObject.transform.position;

        StartTime = Time.time;

        CurrentDamage = Damage;
        if (RandomDamage)
            CurrentDamage += Random.Range(minRandLimit, maxRandLimit);

        Keyframe[] ks;
        ks = new Keyframe[3];

        ks[0] = new Keyframe(0, 1);
        ks[1] = new Keyframe(StartPoinOfDamageReduction / 100, 1);
        ks[2] = new Keyframe(1, FinalDamageInPercent / 100);

        DamageReductionGraph = new AnimationCurve(ks);
    }

    void FixedUpdate()
    {
        Quaternion CurrentStep = gameObject.transform.rotation;

        transform.LookAt(PreviousStep, transform.forward);
        RaycastHit hit = new RaycastHit();
        float Distance = Vector3.Distance(PreviousStep, transform.position);
        if (Distance == 0.0f)
            Distance = 1e-05f;
        Debug.Log(Distance);
        if (Physics.Raycast(PreviousStep, transform.TransformDirection(Vector3.back), out hit, Distance * 0.9999f) && (hit.transform.gameObject != gameObject))
        {
            Instantiate(particleHit, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            SendDamage(hit.transform.gameObject);
        }

        gameObject.transform.rotation = CurrentStep;

        PreviousStep = gameObject.transform.position;
    }

    void DestroyNow()
    {
        Destroy(gameObject);
    }

    void SendDamage(GameObject Hit)
    {
        Hit.transform?.gameObject.HandleComponent<Characters>((component) => component.DealingDamage(CurrentDamage * GetDamageCoefficient()));
        //Hit.SendMessage("ApplyDamage", CurrentDamage * GetDamageCoefficient(), SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }

    float GetDamageCoefficient()
    {
        float Value = 1.0f;
        float CurrentTime = Time.time - StartTime;
        Value = DamageReductionGraph.Evaluate(CurrentTime / TimeToDestruct);

        return Value;
    }
}