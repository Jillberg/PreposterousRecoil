using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    //public static CinemachineShake Instance { get; private set;}

    [SerializeField] private GameObject RotatePoint;

    public float shakeIntensity;
    public float shakeTime;

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;
    private float lerpIntensity;
    private float lerpTime;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    void Awake()
    {
        RotatePoint.GetComponent<Aiming>().OnShoot += ShakeCamera_OnShoot;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (cinemachineVirtualCamera != null)
        {
            Debug.Log("CinemachineVirtualCamera component not found on " + gameObject.name);
        }

        if (cinemachineBasicMultiChannelPerlin != null)
        {
            Debug.Log("found it" );
        }
    }

    private void ShakeCamera_OnShoot(object sender, Aiming.OnShootEventArgs e)
    {
        if (cinemachineVirtualCamera != null)
        {
           CameraShake(shakeIntensity, shakeTime);
 
        }
    }

    public void CameraShake(float intensity,float time)
    {

       // CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin= cinemachineVirtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain=intensity;
        shakeTimer = time;
        lerpIntensity=intensity;
        lerpTime=time;


    }

    void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
               // CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(lerpIntensity, 0f, 1 - (shakeTimer / lerpTime));

            }
        }
        
    }
}
