using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target; // 캐릭터의 Transform
    public Vector3 offset = new Vector3(0, 5, -10); // 캐릭터 대비 카메라의 위치
    public float zoomSpeed = 4f; // 줌 속도
    public float minZoom = 5f; // 최소 줌 거리
    public float maxZoom = 15f; // 최대 줌 거리
    public float minYaw = -40f; // 최소 pitch 각도, 이 값은 사용하지 않으므로 삭제 또는 주석 처리
    public float maxYaw = 80f; // 최대 pitch 각도, 이 값도 마찬가지로 수정될 예정
    public float yawSpeed = 100f; // 카메라 회전 속도
    public float pitchSpeed = 2f; // 카메라 pitch 조정 속도
    public float minPitch = -30f; // 카메라가 내려다볼 수 있는 최소 각도 (아래 방향)
    public float maxPitch = 60f; // 카메라가 올려다볼 수 있는 최대 각도 (위 방향)

    private float currentZoom = 10f; // 현재 줌 거리
    private float currentYaw = 0f; // 현재 Yaw 값 (회전)
    private float currentPitch = 0f; // 현재 Pitch 값

    void Start()
    {
        // 마우스 커서 고정
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 줌 인/아웃
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        // 마우스 이동으로 카메라 회전 (마우스 클릭 없이)
        currentYaw += Input.GetAxis("Mouse X") * yawSpeed * Time.deltaTime;
        
        // 마우스 위/아래 움직임으로 Pitch 조정
        currentPitch -= Input.GetAxis("Mouse Y") * pitchSpeed * Time.deltaTime; // Y축 반전 제거
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
    }

    void LateUpdate()
    {
        // 카메라 위치 업데이트
        Vector3 newPos = target.position - offset * currentZoom;
        transform.position = newPos;

        // 카메라를 대상을 향해 회전시키고, 추가로 Yaw와 Pitch 회전 적용
        transform.LookAt(target.position + Vector3.up * 2f); // 카메라가 항상 대상을 바라보도록 기준 설정
        transform.RotateAround(target.position, Vector3.up, currentYaw);
        transform.RotateAround(target.position, transform.right, currentPitch);
    }
}