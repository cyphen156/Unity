using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject LoadingPannel;
    public GameObject BossUIPannel;
    public Slider bossHPBar;

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

    public void StartLoad()
    {
        LoadingPannel.SetActive(true);
    }

    public void EndLoad()
    {
        LoadingPannel.SetActive(false);
    }

    public GameObject GetBossUIPannel()
    {
        return BossUIPannel;
    }
    public void ActiveBossUI()
    {
        bossHPBar.transform.SetAsLastSibling();
        BossUIPannel.SetActive(true);
    }

    public void DeactiveBossUI()
    {
        BossUIPannel.SetActive(false);
    }
}
