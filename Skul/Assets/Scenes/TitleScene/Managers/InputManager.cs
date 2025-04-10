using UnityEngine;

/// <summary>
/// 플레이어와의 상호작용을 처리할 입력처리기
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
