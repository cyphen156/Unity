using UnityEngine;

/// <summary>
/// 게임 시스템을 총괄하는 매니저, 씬 체인지와 같은 굵직한 이벤트 처리
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    public void ChangeScene(string SceneName)
    {
        ResourceManager.instance.LoadScene(SceneName);
    }
}
