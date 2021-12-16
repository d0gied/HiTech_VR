using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolowPhysics : MonoBehaviour
{
    Rigidbody rb;
    public Transform target;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(target.position);
    }
}
