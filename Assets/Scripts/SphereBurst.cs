using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBurst : MonoBehaviour
{
    [SerializeField]
    float timerMax = 0.5f;
    new Rigidbody rigidbody;
    SphereGen sphereGen;
    void Awake()
    {
        this.gameObject.GetComponent<Collider>().enabled = false;
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
        sphereGen = FindObjectOfType<SphereGen>();
        Burst(RandomDir(), 30f);
        rigidbody.drag = sphereGen.airResistance;
    }
    void Update()
    {
        if(timerMax > 0) timerMax -= Time.deltaTime;
        else
        {
            this.gameObject.GetComponent<Collider>().enabled = true;
            Destroy(this);
        }
    }
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
        this.rigidbody.AddForce(dir * strength, ForceMode.Impulse);
    }

    
}
