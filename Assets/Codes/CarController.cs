using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public enum Axel // Çekiş
{
    Front, // Ön
    Rear // Arka
}
[Serializable] public struct Wheel
{
    public GameObject model;
    public WheelCollider collider;
    public Axel axel;
}
public class CarController : MonoBehaviour
{
    #region Variables
    //PRIVATE------------------------------------------------------------------------
    [SerializeField] private float maxSpeed = 200f; // Maksimum Hız
    [SerializeField] private float turnSensibility = 1f; // Dönüş Hassasiyeti
    [SerializeField] private float maxTurnAngle = 45f; // Maksimum Dönüş Açısı
    [SerializeField] private List<Wheel> _wheels; // Tekerleklerin Listesi
    private float _inputX, _inputY; // Klavye Girdileri 
    //PUBLİC--------------------------------------------------------------------------
    public Vector3 centerOfMass;
    #endregion

    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
    }

    private void LateUpdate()
    {
        Move();
        Turn();
    }

    private void Update()
    {
        WheelTurner();
        MoveDirection();
        Brake();
    }

    private void MoveDirection()
    {
        _inputX = Input.GetAxis("Horizontal");
        _inputY = Input.GetAxis("Vertical");
    }
    private void Move()
    {
        foreach (var wheel in _wheels)
        {
            wheel.collider.motorTorque = _inputY * maxSpeed * 500 * Time.deltaTime;
        }
    }

    private void Turn()
    {
        foreach (var wheel in _wheels)
        {
            if (wheel.axel==Axel.Front)
            {
                var _steerAngle = _inputX * turnSensibility * maxTurnAngle;
                wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle,_steerAngle,0.1f);
            }
        }
    }

    private void WheelTurner()
    {
        foreach (var wheel in _wheels)
        {
            Quaternion _rot;
            Vector3 _pos;
            wheel.collider.GetWorldPose(out _pos, out _rot);
            wheel.model.transform.position = _pos;
            wheel.model.transform.rotation = _rot;
        }
    }

    private void Brake()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var wheel in _wheels)
            {
                wheel.collider.brakeTorque = 10000;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            foreach (var wheel in _wheels)
            {
                wheel.collider.brakeTorque = 0;
            }
        }
    }
}
