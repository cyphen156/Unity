using UnityEngine;

/// <summary>
/// ���� �ý����� �Ѱ��ϴ� �Ŵ���, �� ü������ ���� ������ �̺�Ʈ ó��
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void ChangeScene(string SceneName)
    {
        ResourceManager.instance.LoadScene(SceneName);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void ResetGame()
    {
        PlayerManager.instance.GetStateMachine().PlayDeathSequence();
    }
}
