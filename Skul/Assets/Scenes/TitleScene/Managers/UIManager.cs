using System;
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
    public GameObject PlayerUIPannel;
    public Slider bossHPBar;
    public Slider playerHPBar;

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
        { 
            text.text = "_";
        }
    }

    public void UpdateBindingText(string controlName, KeyCode newKey)
    {
        TMP_Text text = FindText(controlName);
        if (text != null)
        {
            text.text = newKey.ToString();
        }
    }
    private Transform FindDeepChild(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child;
            }

            Transform result = FindDeepChild(child, name);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
    private TMP_Text FindText(string controlName)
    {
        string buttonName = controlName + "Button";
        string textName = controlName + "ButtonText";

        Transform button = FindDeepChild(SystemUIPannel.transform, buttonName);
        if (button == null)
        {
            Debug.LogWarning($"[UIManager] {buttonName} 오브젝트를 찾을 수 없습니다.");
            return null;
        }

        Transform textObj = button.Find(textName);
        if (textObj == null)
        {
            Debug.LogWarning($"[UIManager] {textName} 오브젝트를 {buttonName} 안에서 찾을 수 없습니다.");
            return null;
        }

        return textObj.GetComponent<TMP_Text>();
    }

    public void UpdateAllBindingTexts()
    {
        foreach (InputManager.Controll control in Enum.GetValues(typeof(InputManager.Controll)))
        {
            KeyCode key = InputManager.instance.GetDefaultKey(control);
            UpdateBindingText(control.ToString(), key);
        }
    }

    public void SetHeadIcon(string targetHeadIcon, Sprite changeIcon)
    {
        Transform target = FindDeepChild(PlayerUIPannel.transform, targetHeadIcon);
        Image image = target.GetComponent<Image>();
        ChangeIcon(image, changeIcon);
    }
    public void SetSkillIcons(Sprite changeIcon1, Sprite changeIcon2)
    {
        Transform target1 = FindDeepChild(PlayerUIPannel.transform, "SkillIcon1");
        Transform target2 = FindDeepChild(PlayerUIPannel.transform, "SkillIcon2");
        Image image1 = target1.GetComponent<Image>();
        Image image2 = target2.GetComponent<Image>();
        ChangeIcon(image1, changeIcon1);
        ChangeIcon(image2, changeIcon2);
    }

    private void ChangeIcon(Image targetImage, Sprite changeIcon)
    {
        targetImage.sprite = changeIcon;
        targetImage.enabled = true;
    }
    public void UpdateHPBar(float ratio, Slider target)
    {
        target.value = ratio;
    }
}
