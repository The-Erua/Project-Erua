using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // 인스턴스가 없을 때, 씬에서 찾기
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    // 씬에서도 못 찾았을 때, 새로 생성
                    GameObject obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // 중복 인스턴스가 생성된 경우 제거
        }
    }
}