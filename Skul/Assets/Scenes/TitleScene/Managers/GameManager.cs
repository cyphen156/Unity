using UnityEngine;

/// <summary>
/// 게임 시스템을 총괄하는 매니저, 씬 체인지와 같은 굵직한 이벤트 처리
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
}
