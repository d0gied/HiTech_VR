using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int boxCount = 0;
    public Text watch;
    void Start()
    {
        watch.text = "0";
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Box"))
            boxCount += 1;
        watch.text = boxCount.ToString();
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Box"))
            boxCount -= 1;
        watch.text = boxCount.ToString();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
