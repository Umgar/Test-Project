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
    public GameObject spherePrefab;
    Vector3 movementForce;
    void Awake()
    {
        gravitationSizeOrg = gravitationSize;
        gravitationForceOrg = gravitationForce;
        rigidbody = this.GetComponent<Rigidbody>();
        spherePrefab = GameObject.FindObjectOfType<SphereGen>().spehrePrefab;
    }
    public void SetForce(Vector3 force)
    {
        this.movementForce = force;
        this.transform.localScale = new Vector3(1, 1, 1);
        this.transform.SetParent(this.transform.parent);
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
        { this.transform.SetParent(collision.transform); this.gameObject.SetActive(false); }
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
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, gravitationSize);
        Rigidbody anotherRB;
        SpherePhysics spherePhysics = null;
        Vector3 newPos;
        foreach (Collider collider in colliders)
        {
            collider.gameObject.TryGetComponent<SpherePhysics>(out spherePhysics);
            if (spherePhysics == null) continue;
            else anotherRB = spherePhysics.rigidbody;
            newPos = Vector3.MoveTowards(anotherRB.position, this.transform.position, gravitationForce * Time.fixedDeltaTime);
            anotherRB.MovePosition(newPos);
        }
        if (movementForce != null)
            rigidbody.MovePosition(rigidbody.position + movementForce * Time.deltaTime * 20f);
    }
    void Explode()
    {
        /*SpherePhysics[] childs = this.GetComponentsInChildren<SpherePhysics>();
        foreach (SpherePhysics child in childs)
        {
            child.gameObject.SetActive(true);
            child.SetForce(RandomVector3(1));
        }
        SetForce(RandomVector3(1));*/
    }
    Vector3 RandomVector3(float range)
    {
        float x, y, z;
        x = Random.Range(-range, range);
        y = Random.Range(-range, range);
        z = Random.Range(-range, range);
        return new Vector3(x, y, z);
    }
}
