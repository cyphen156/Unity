using UnityEngine;

/// <summary>
/// �÷��̾���� ��ȣ�ۿ��� ó���� �Է�ó����
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
