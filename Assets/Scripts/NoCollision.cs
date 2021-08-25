using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoCollision : MonoBehaviour
{
    [SerializeField]
    float timerMax = 0.5f;
    void Awake()
    {
        this.gameObject.GetComponent<Collider>().enabled = false;
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

    
}
