using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowMotionScale = 0.5f;
    public CameraMovement cameraMovement; // CameraMovement에 대한 참조 추가
    public GameObject distortion;
    void StartSlowMotion()
    {
        Time.timeScale = slowMotionScale;
        distortion.SetActive(true);
        cameraMovement.EnableForcedZoom(true); // 카메라 강제 줌인 활성화
    }

    void EndSlowMotion()
    {
        Time.timeScale = 1.0f;
        distortion.SetActive(false);
        cameraMovement.EnableForcedZoom(false); // 카메라 강제 줌인 비활성화
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartSlowMotion();
        }
        
        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            EndSlowMotion();
        }
    }
}