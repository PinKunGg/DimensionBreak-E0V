using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    CinemachineVirtualCamera CinemachineVirtualCamera;
    float shakeTimer;
    public static CameraShake instance;
    private void Awake()
    {
        if(instance != null)
        {
            print("Bruh have more than one CameraShake");
        }
        else
        {
            instance = this;
        }

        CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();  
    }

    public void ShakeCam(float intensity, float time)
    {
        //GetCinemachineComponent
        CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlin = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    private void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
        }

        if(shakeTimer <= 0f)
        {
            CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlin = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
    }

    public void DeathCam()
    {
        CinemachineVirtualCamera.m_Follow = null;
    }
}
