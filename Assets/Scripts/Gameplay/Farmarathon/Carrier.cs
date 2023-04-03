using System.Collections;
using System.Collections.Generic;
using MirrorBasics;
using UnityEngine;

public class Carrier : MonoBehaviour
{
    public bool isCarrier = false;
    public float Hunger;
    public float currentHunger;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentHunger -= Time.deltaTime;
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<PlayerScore>() != null && isCarrier)
        {
            if (other.GetComponent<PlayerScore>().hasItem)
            {
                currentHunger +=3;
                other.GetComponent<PlayerScore>().hasItem = false;
            }
        }
    }

}
