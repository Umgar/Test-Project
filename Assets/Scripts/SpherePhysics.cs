using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePhysics : MonoBehaviour
{
    [SerializeField]
    public new Rigidbody rigidbody{get; protected set;}
    [SerializeField]
    float gravitationSize = 1;
    public float gravitationForce = 3;
    public GameObject spherePrefab;
    void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        spherePrefab = GameObject.FindObjectOfType<SphereGen>().spehrePrefab;
    }

    void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, gravitationSize);
        Rigidbody anotherRB;
        Vector3 newPos;
        foreach(Collider collider in colliders)
        {
            anotherRB = collider.gameObject.GetComponent<SpherePhysics>().rigidbody;
            newPos = Vector3.MoveTowards(anotherRB.position, this.transform.position, gravitationForce * Time.fixedDeltaTime);
            anotherRB.MovePosition(newPos);
        }
    }
}
