using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;
    // minYaw와 maxYaw는 사용하지 않으므로 삭제합니다.
    public float yawSpeed = 100f;
    public float pitchSpeed = 2f;
    public float minPitch = -30f;
    public float maxPitch = 60f;

    private float currentZoom = 10f;
    private float currentYaw = 0f;
    private float currentPitch = 0f;
    private bool isForcedZoom = false; // 강제 줌인 상태 플래그
    public float forcedZoomTarget = 1f; // 강제 줌인 시 목표 줌 값
    public float forcedZoomAmount = 1f; // 강제 줌인할 때의 목표 줌 값
    private float targetZoom; // 목표 줌 값을 저장하는 변수
    private float zoomLerpSpeed = 5f; // 줌이 원래 값으로 돌아가는데 사용되는 보간 속도
    private float originalPoint = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        targetZoom = currentZoom; // 초기 목표 줌 값을 현재 줌 값으로 설정
    }

    void Update()
    {
        if (isForcedZoom)
        {
            // 강제 줌인 상태
            currentZoom = Mathf.Lerp(currentZoom, forcedZoomAmount, Time.deltaTime * zoomSpeed);
        }
        else if (!isForcedZoom && StillFar())
        {
            // 강제 줌인이 해제되고, 원래 줌 값으로 천천히 돌아감
            currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomLerpSpeed);
        }
        else
        {
            // 정상적인 줌 조절
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                targetZoom -= scroll * zoomSpeed;
                targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
            }
        }

        // 마우스 이동으로 카메라 회전
        float modifiedYawSpeed = isForcedZoom ? yawSpeed * 0.5f : yawSpeed;
        currentYaw += Input.GetAxis("Mouse X") * modifiedYawSpeed * Time.deltaTime;
        
        // 마우스 위/아래 움직임으로 Pitch 조정
        float modifiedPitchSpeed = isForcedZoom ? pitchSpeed * 0.5f : pitchSpeed;
        currentPitch -= Input.GetAxis("Mouse Y") * modifiedPitchSpeed * Time.deltaTime;
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
    }

    private bool StillFar()
    {
        var result = Mathf.Abs(currentZoom - targetZoom) > 0.1f;
        if (!result)
            targetZoom = currentZoom;
        return result;
    }

    void LateUpdate()
    {
        Vector3 newPos = target.position - offset * currentZoom;
        transform.position = newPos;

        transform.LookAt(target.position + Vector3.up * 2f);
        transform.RotateAround(target.position, Vector3.up, currentYaw);
        transform.RotateAround(target.position, transform.right, currentPitch);
    }

    public void EnableForcedZoom(bool enable)
    {
        isForcedZoom = enable;
    }
}
