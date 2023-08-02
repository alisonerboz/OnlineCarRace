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
public class MyCarController : MonoBehaviour
{
    #region Variables
    //PRIVATE------------------------------------------------------------------------
    [SerializeField] private float maxSpeed = 200f; // Maksimum Hız
    [SerializeField] private float acceleration = 500f; // Hızlanma Çarpanı
    [SerializeField] private float turnSensibility = 1f; // Dönüş Hassasiyeti
    [SerializeField] private float maxTurnAngle = 45f; // Maksimum Dönüş Açısı
    [SerializeField] private float brakePower = 500f; // Fren Gücü
    [SerializeField] private List<Wheel> _wheels; // Tekerleklerin Listesi
    [SerializeField] private GameObject[] _brakeLights; // Arka Fren Işıkları Listesi
    [SerializeField] private GameObject[] _headLights; // Ön Işıkları Listesi
    private float _inputX, _inputY; // Klavye Girdileri 
    //PUBLİC--------------------------------------------------------------------------
    private Vector3 centerOfMass=new (0f,0.035f,0f);
    //BOOL
    private bool isHeadLightOpen;
    #endregion

    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
    }

    private void LateUpdate()
    {
        Move();
        Turn();
        Brake();
        BackLightsCheck();
    }

    private void Update()
    {
        WheelTurner();
        MoveDirection();
        if (Input.GetKeyDown(KeyCode.H))
        {
            _headLights[0].SetActive(isHeadLightOpen);
            _headLights[1].SetActive(isHeadLightOpen);
            isHeadLightOpen = !isHeadLightOpen;
        }
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
            wheel.collider.motorTorque = _inputY * maxSpeed * acceleration * Time.deltaTime;
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
                wheel.collider.brakeTorque = brakePower;
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

    private void BackLightsCheck()
    {
        foreach (var wheel in _wheels)
        {
            if (wheel.collider.motorTorque<0||Input.GetKey(KeyCode.Space))
            {
                foreach (var brakeLight in _brakeLights)
                {
                    brakeLight.SetActive(true);
                }
            }
            else
            {
                foreach (var brakeLight in _brakeLights)
                {
                    brakeLight.SetActive(false);
                }
            }
        }
    }
}
