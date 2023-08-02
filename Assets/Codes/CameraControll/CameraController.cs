using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CameraController : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject attachedVehicle; // Kameraya Atanmış Araba
    [SerializeField] private GameObject cameraFolder; // Kamera Klasörü
    [SerializeField] private Transform[] cameraLocations; // Kamera Konumlarının Listesi
    [SerializeField] private int locationIndicator = 1; // Konum Değişme İndikatörü
    [Range(0, 1)] public float smoothTime = 0.8f; // Yumuşak Geçiş Süresi
    #endregion

    private void Start()
    {
        attachedVehicle=GameObject.FindGameObjectWithTag("Player");
        cameraFolder = attachedVehicle.transform.Find("CAMERA").gameObject;
        cameraLocations = cameraFolder.GetComponentsInChildren<Transform>();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (locationIndicator >= 3)
            {
                locationIndicator = 2;
                locationIndicator = 1;
            }
            else locationIndicator++;
        }
    }

    private void LateUpdate()
    {
        
        transform.position = cameraLocations[locationIndicator].position * (1 - smoothTime) + transform.position * smoothTime;
        transform.LookAt(cameraFolder.transform);
    }
}
