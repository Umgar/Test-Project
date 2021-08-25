using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereGen : MonoBehaviour
{
    [SerializeField]
    float timerMax = 0.25f;
    public float airResistance = 0.10f;
    [SerializeField]
    float[] offsets = new float[3];
    float timer;
    public int spheresCreated {private set; get;} = 0;
    int sphereID = 0;
    public Text sphereCounter;
    public GameObject spherePrefab;
    public Transform sphereContainer;
    public int interpolation{private set; get;} = 1;
    void Awake()
    {
        timer = timerMax;
    }

    // Update is called once per frame
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
    public void CreateSphere(Vector3 position, bool addForce = false)
    {
        GameObject newSphere = Instantiate(spherePrefab, position, spherePrefab.transform.rotation, sphereContainer);
        newSphere.GetComponent<SpherePhysics>().sphereGen = this;
        newSphere.name = sphereID.ToString();
        sphereID++;
    }
    Vector3 NewPos()
    {
        float[] localOffsets = new float[3];
        for (int i = 0; i < 3; i++)
            localOffsets[i] = Random.Range(-offsets[i], offsets[i]);
        return new Vector3(localOffsets[0], localOffsets[1], localOffsets[2]);
    }
}
