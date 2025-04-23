using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject LoadingPannel;
    public GameObject BossUIPannel;
    public GameObject SystemUIPannel;
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
    public void ActiveUIPannel(GameObject UIPannelName)
    {
        UIPannelName.SetActive(true);
    }

    public void DeactiveUIPannel(GameObject UIPannelName)
    {
        UIPannelName.SetActive(false);
    }

    public void ShowRebindingText(string controlName)
    {
        TMP_Text text = FindText(controlName);
        if (text != null)
            text.text = "_";
    }

    public void UpdateBindingText(string controlName, KeyCode newKey)
    {
        TMP_Text text = FindText(controlName);
        if (text != null)
            text.text = newKey.ToString();
    }
    private TMP_Text FindText(string controlName)
    {
        Transform btn = SystemUIPannel.transform.Find(controlName + "Button");
        if (btn == null) return null;
        return btn.Find(controlName + "Text")?.GetComponent<TMP_Text>();
    }
}
