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
    public int sphereAbsorb { get; private set; } = 1;
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
            Merge(otherTransform);
        else
        {
            Vector3 newPos;
            newPos = (this.transform.position + otherTransform.transform.position) / 2;
            sphereAbsorb += otherTransform.gameObject.GetComponent<SpherePhysics>().sphereAbsorb;
            GetBigger(newPos, this.sphereAbsorb);
        }
    }
    void GetBigger(Vector3 newPos, int size)
    {
        this.transform.position = newPos;
        this.transform.localScale = new Vector3(size, size, size);
        if (sphereAbsorb >= 50) { Explode(); return; }
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
        if (offCollider > 0)
            offCollider -= Time.deltaTime;
        else this.GetComponent<Collider>().enabled = true;
    }
    void Explode()
    {
        /*Transform child;
        while (this.transform.childCount > 0)
        {
            child = this.transform.GetChild(0);
            child.SetParent(this.transform.parent);
            child.GetComponent<Collider>().enabled = false;
            child.gameObject.SetActive(true);
            child.localScale = new Vector3(1,1,1);
            //child.GetComponent<SpherePhysics>().Burst(RandomDir(), 5f);
        }*/
        transform.localScale = new Vector3(1, 1, 1);
        sphereAbsorb = 1;
    }
    float offCollider = 0;
    Vector3 RandomDir()
    {
        float[] position = new float[3];
        for (int i = 0; i < 3; i++)
            position[i] = Random.Range(-1f, 1f);
        return new Vector3(position[0], position[1], position[2]);
    }
    //Function add force to the object
    public void Burst(Vector3 dir, float strength)
    {
        offCollider = 0.5f;
        this.rigidbody.AddForce(dir * strength, ForceMode.Impulse);
    }
    public void Merge(Transform gameObject)
    {
        Transform child;
        SpherePhysics spherePhysics;
        while (this.transform.childCount > 0)
        {
            child = this.transform.GetChild(0);
            spherePhysics = child.GetComponent<SpherePhysics>();
            spherePhysics.sphereAbsorb = 1;
            if (child.childCount > 0)
                spherePhysics.Merge(gameObject);
            child.gameObject.SetActive(false);
            child.SetParent(gameObject);
        }
        this.transform.position = gameObject.position;
        sphereAbsorb = 1;
        this.transform.SetParent(gameObject);
        this.gameObject.SetActive(false);
    }
}
