using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereGen : MonoBehaviour
{
    //Timer for generating the spheres
    [SerializeField]
    float timerMax = 0.25f;
    float timer;
    //Value of the air Resistance
    public float airResistance = 0.10f;
    //Offsets from point [0,0,0] where spheres can spawn
    [SerializeField]
    float[] offsets = new float[3];
    //Number of spheres created
    public int spheresCreated {private set; get;} = 0;
    //UI element that display how many sphere have been created
    public Text sphereCounter;
    //Original sphere prefab
    public GameObject spherePrefab;
    //Parent object for the spheres
    public Transform sphereContainer;
    //Value of the gravitation 1 = attract other objects; 2 = push away other objects
    public int interpolation{private set; get;} = 1;
    void Awake()
    {
        timer = timerMax;
    }
    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else if (spheresCreated < 250)
        {
            CreateSphere(NewPos());
            spheresCreated++;
            sphereCounter.text = spheresCreated.ToString();
            timer = timerMax;
            if(spheresCreated == 250) interpolation = -1;
        }
    }
    void CreateSphere(Vector3 position)
    {
        GameObject newSphere = Instantiate(spherePrefab, position, spherePrefab.transform.rotation, sphereContainer);
        newSphere.GetComponent<SpherePhysics>().sphereGen = this;
        newSphere.name = spheresCreated.ToString();
    }
    Vector3 NewPos()
    {
        float[] localOffsets = new float[3];
        for (int i = 0; i < 3; i++)
            localOffsets[i] = Random.Range(-offsets[i], offsets[i]);
        return new Vector3(localOffsets[0], localOffsets[1], localOffsets[2]);
    }
}
