using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : MonoBehaviourSingleton<TimeManager>
{
    public float slowMotionScale = 0.5f;
    public CameraMovement cameraMovement; // CameraMovement에 대한 참조 추가
    public ColorFilterAdjustment adjustment;
    public GameObject distortion;
    public GameObject effect;
    public GameObject effect1;
    public GameObject effect2;
    [SerializeField] private float time;


    public void WaitForSecs(Action action, float time)
    {
        StartCoroutine(DoWaitForSecs(action, time));
    }

    IEnumerator DoWaitForSecs(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }
    void StartSlowMotion()
    {
        Time.timeScale = slowMotionScale;
        effect2.SetActive(true);
        
        StartCoroutine(CO_Slow());
    }

    private IEnumerator CO_Slow()
    {
        yield return new WaitForSeconds(time);
        distortion.SetActive(true);
        effect.SetActive(true);
        effect1.SetActive(true);
        adjustment.ActiveColorAdjustment();
        cameraMovement.EnableForcedZoom(true); // 카메라 강제 줌인 활성화
    }

    void EndSlowMotion()
    {
        Time.timeScale = 1.0f;
        distortion.SetActive(false);
        effect.SetActive(false);
        effect1.SetActive(false);
        effect2.SetActive(false);
        adjustment.DeActiveColorAdjustment();

        cameraMovement.EnableForcedZoom(false); // 카메라 강제 줌인 비활성화
    }

    private void Start()
    {
       
    }

    private RigidBodyTimeController _controller;
    private AnimationTimeController _controller1;
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            var player =  FindAnyObjectByType<Player>();
            var rigidBody = player.GetComponent<Rigidbody>();
            var animator = player.GetComponent<Animator>();
            _controller = new RigidBodyTimeController(rigidBody);
            _controller.ControlTimeScale(time);
            _controller1 = new AnimationTimeController(animator);
            _controller1.ControlTimeScale(time);
            // StartSlowMotion();
        }
        
        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            _controller.ResetTimeScale();
            _controller1.ResetTimeScale();
            // EndSlowMotion();
        }
    }
}