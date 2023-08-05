using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallride : MonoBehaviour
{
    [SerializeField] float gravityScale = 0.5f;
    [SerializeField] private GameObject myCar;
    [SerializeField] private bool isGrounded=false;
    
    
    private Rigidbody rb;

    private void Start()
    {
        rb = myCar.GetComponent<Rigidbody>();
        Debug.Log(Physics.gravity);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Wheels"))
        {
            //isGrounded = true;
            Debug.Log("dokundu");
            Physics.gravity = new Vector3(0, -6, -1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wheels"))
        {
            //isGrounded = false;
            Debug.Log("cikti");
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }

    }

    /*private void FixedUpdate()
    {
        if (isGrounded==false)
        {
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
    }*/
}
