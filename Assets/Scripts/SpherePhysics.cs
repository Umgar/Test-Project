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
    //Const gravitation value
    [SerializeField]
    const float gravitationConstForce = 1;
    float massOrg;
    public SphereGen sphereGen;
    //List of Spheres that are merged in to this object
    public List<GameObject> merged;
    void Awake()
    {
        gravitationSizeOrg = gravitationSize;
        rigidbody = this.GetComponent<Rigidbody>();
        massOrg = rigidbody.mass;
        
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


        if (colliderNameVal < thisNameVal)
            Merge(otherTransform);
        else
        {
            Vector3 newPos;
            newPos = (this.transform.position + otherTransform.transform.position) / 2;
            int otherTransformMergedCount = otherTransform.GetComponent<SpherePhysics>().merged.Count;
            GetBigger(newPos, merged.Count + otherTransformMergedCount + 1);
        }
    }

    void GetBigger(Vector3 newPos, int size)
    {
        this.transform.localScale = new Vector3(1, 1, 1) * size;
        if (size >= 50) { Explode(); return; }
        gravitationSize = size * gravitationSizeOrg;
        rigidbody.mass = size * massOrg;
    }
    void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, gravitationSize);
        Vector3 direciton;
        float distance, gravitationForce;
        foreach (Collider collider in colliders)
        {
            distance = Vector3.Distance(this.transform.position, collider.transform.position);
            gravitationForce = gravitationConstForce * (this.rigidbody.mass * collider.attachedRigidbody.mass) / Mathf.Pow(distance, 2);
            direciton = collider.transform.position - this.transform.position;
            direciton *= sphereGen.interpolation;
            if (direciton == new Vector3(0, 0, 0) || gravitationForce == Mathf.Infinity) continue;
            rigidbody.AddForce(direciton.normalized * gravitationForce, ForceMode.Force);
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
        this.gravitationSize = gravitationSizeOrg;
        this.rigidbody.mass = massOrg;
        this.gameObject.AddComponent<SphereBurst>();
        merged.Clear();
    }

    public void Merge(Transform gameObject)
    {
        SpherePhysics spherePhysics;
        this.transform.position = gameObject.position;
        spherePhysics = gameObject.GetComponent<SpherePhysics>();
        spherePhysics.merged.AddRange(this.merged);
        merged.Clear();
        spherePhysics.merged.Add(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
