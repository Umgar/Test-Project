using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePhysics : MonoBehaviour
{
    [SerializeField]
    public new Rigidbody rigidbody { get; protected set; }
    [SerializeField]
    float gravitationSize = 1;
    float gravitationSizeOrg;
    public float gravitationForce = 1;
    float gravitationForceOrg;
    public SphereGen sphereGen;
    void Awake()
    {
        gravitationSizeOrg = gravitationSize;
        gravitationForceOrg = gravitationForce;
        rigidbody = this.GetComponent<Rigidbody>();

    }
    void Start()
    {
        rigidbody.drag = sphereGen.airResistance;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (sphereGen.interpolation != 1) return;
        Transform otherTransform = collision.gameObject.transform;
        int colliderNameVal, thisNameVal;
        if (!int.TryParse(collision.gameObject.name, out colliderNameVal))
            Debug.LogWarning(collision.gameObject.name + " can't convert name to number");
        if (!int.TryParse(this.name, out thisNameVal))
            Debug.LogWarning(this.gameObject.name + " can't convert name to number");
        if (colliderNameVal > thisNameVal)
            Destroy(this.gameObject);
        else
        {
            Vector3 size, newPos;
            newPos = (this.transform.position + otherTransform.transform.position) / 2;
            size = this.transform.localScale + otherTransform.transform.localScale;
            GetBigger(newPos, size);
        }
    }
    void GetBigger(Vector3 newPos, Vector3 size)
    {
        this.transform.position = newPos;
        this.transform.localScale = size;
        if (gravitationSize / gravitationSizeOrg >= 50) { Explode(); return; }
        gravitationForce = size.x * gravitationForceOrg;
        gravitationSize = size.x * gravitationSizeOrg;
    }
    void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, gravitationSize);
        Rigidbody anotherRB;
        Vector3 newPos, direciton;
        float distance;
        foreach (Collider collider in colliders)
        {
            anotherRB = collider.gameObject.GetComponent<Rigidbody>();
            distance = Vector3.Distance(this.transform.position, anotherRB.position);
            if (sphereGen.interpolation == 1)
                newPos = Vector3.MoveTowards(anotherRB.position, this.transform.position, gravitationForce * Time.fixedDeltaTime / distance);
            else
            {
                direciton = -(this.transform.position - anotherRB.position);
                newPos = Vector3.MoveTowards(anotherRB.position, (anotherRB.position + direciton), gravitationForce * Time.fixedDeltaTime / distance);
            }
            anotherRB.MovePosition(newPos);
        }

    }
    void Explode()
    {
        transform.localScale = new Vector3(1, 1, 1);
        //Create 49 spheres
    }
    //Function add force to the object
    public void Burst(Vector3 dir, float strength)
    {
        this.rigidbody.AddForce(dir * strength, ForceMode.Impulse);
    }
}
