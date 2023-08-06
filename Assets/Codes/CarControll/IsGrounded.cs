using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded : MonoBehaviour
{
    [SerializeField] private GameObject myCar;
    private bool _isGrounded=false;
    public float flipForce = 10f; // Döndürme kuvveti
    private Rigidbody rb;
    private float _inputX; // Klavye Girdileri 
    private void Start()
    {
        rb = myCar.GetComponent<Rigidbody>();
    }
    
    private void OnTriggerStay(Collider other)
    {
        _isGrounded = true;
    }
    private void OnTriggerExit(Collider other)
    {
        _isGrounded = false;
    }

    private void Update()
    {
        Debug.Log(_isGrounded);
        TurnVehicle();
        _inputX = Input.GetAxis("Horizontal");
    }

    private void TurnVehicle()
    {
        if (_isGrounded==false)
        {
            
            float t = Mathf.InverseLerp(80f, 280f, transform.eulerAngles.z);
            float flipTorque = Mathf.Lerp(0f, flipForce, t);
            rb.AddTorque((transform.forward * flipTorque) *-_inputX, ForceMode.Acceleration);
            if (Input.GetKey(KeyCode.LeftControl))
            {
                rb.AddTorque((transform.right * -flipTorque/4), ForceMode.Acceleration);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                rb.AddTorque((transform.right * flipTorque/4), ForceMode.Acceleration);
            }
        }
    }
}
