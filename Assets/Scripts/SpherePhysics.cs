using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePhysics : MonoBehaviour
{
    [SerializeField]
    public new Rigidbody rigidbody { get; protected set; }
    //Size of gravitation sphere
    [SerializeField]
    float gravitationSize = 1;
    float gravitationSizeOrg;
    //Attracting/pushing force value
    public float gravitationForce = 1;
    float gravitationForceOrg;
    public SphereGen sphereGen;
    //List of Spheres that are merged in to this object
    public List<GameObject> merged;
    void Awake()
    {
        gravitationSizeOrg = gravitationSize;
        gravitationForceOrg = gravitationForce;
        rigidbody = this.GetComponent<Rigidbody>();
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


        if (colliderNameVal < thisNameVal)
            Merge(otherTransform);
        else
        {
            Vector3 newPos;
            newPos = (this.transform.position + otherTransform.transform.position) / 2;
            int otherTransformMergedCount = otherTransform.GetComponent<SpherePhysics>().merged.Count;
            GetBigger(newPos, merged.Count + otherTransformMergedCount + 2);
        }
    }

    void GetBigger(Vector3 newPos, int size)
    {
        this.transform.position = newPos;
        this.transform.localScale = new Vector3(1, 1, 1) * size;
        if (size == 50) { Explode(); return; }
        gravitationForce = size * gravitationForceOrg;
        gravitationSize = size * gravitationSizeOrg;
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
        foreach (GameObject o in merged)
        {
            o.transform.position = this.transform.position;
            o.transform.localScale = new Vector3(1, 1, 1);
            o.SetActive(true);
            o.AddComponent<SphereBurst>();
        }
        transform.localScale = new Vector3(1, 1, 1);
        this.gameObject.AddComponent<SphereBurst>();
        merged.Clear();
    }

    public void Merge(Transform gameObject)
    {
        SpherePhysics spherePhysics;
        this.transform.position = gameObject.position;
        spherePhysics = gameObject.GetComponent<SpherePhysics>();
        foreach (GameObject o in merged)
        {
            spherePhysics.merged.Add(o);
        }
        merged.Clear();
        spherePhysics.merged.Add(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
