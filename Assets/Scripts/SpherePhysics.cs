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
    public void SetParents(Transform newParent)
    {
        this.transform.SetParent(newParent);
    }
    void OnCollisionEnter(Collision collision)
    {
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
        if (this.transform.localScale.x >= 50) { Explode(); return; }
        gravitationForce = size.x * gravitationForceOrg;
        gravitationSize = size.x * gravitationSizeOrg;
    }
    void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, gravitationSize);
        Rigidbody anotherRB;
        Vector3 newPos;
        float distance;
        foreach(Collider collider in colliders)
        {
            anotherRB = collider.gameObject.GetComponent<Rigidbody>();
            distance = Vector3.Distance(this.transform.position, anotherRB.position);
            newPos = Vector3.MoveTowards(anotherRB.position, this.transform.position, gravitationForce * Time.fixedDeltaTime / distance);
            anotherRB.MovePosition(newPos * sphereGen.interpolation);
        }

    }
    void Explode()
    {
        transform.localScale = new Vector3(1,1,1);
        //Add 49 spheres
    }
}
