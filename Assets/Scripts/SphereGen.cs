using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereGen : MonoBehaviour
{
    [SerializeField]
    float timerMax = 0.25f;
    [SerializeField]
    float[] offsets = new float[3];
    float timer;
    int spheresCreated = 0;
    public Text sphereCounter;
    public GameObject spehrePrefab;
    public Transform sphereContainer;
    void Awake()
    {
        timer = timerMax;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
            timer -= Time.deltaTime;
        else
        {
            GameObject newSphere = Instantiate(spehrePrefab, NewPos(), spehrePrefab.transform.rotation, sphereContainer);
            newSphere.GetComponent<SpherePhysics>().spherePrefab = this.spehrePrefab;
            spheresCreated++;
            sphereCounter.text = spheresCreated.ToString();
            if(spheresCreated >= 250) this.enabled = false;
            timer = timerMax;
        }
    }
    Vector3 NewPos()
    {
        float[] localOffsets = new float[3];
        for(int i=0;i<3;i++)
            localOffsets[i] = Random.Range(-offsets[i],offsets[i]);
        return new Vector3(localOffsets[0], localOffsets[1], localOffsets[2]);
    }
}
