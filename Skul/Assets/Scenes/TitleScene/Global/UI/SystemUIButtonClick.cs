using System;
using TMPro;
using UnityEngine;

public class SystemUIButtonClick : MonoBehaviour
{
    public void OnReset()
    {
        InputManager.instance.ResetKeyBindings();
    }

    public void OnRebind(GameObject buttonObject)
    {
        string controlName = buttonObject.name.Replace("Button", "");

        if (Enum.TryParse(controlName, out InputManager.Controll ctrl))
        {
            InputManager.instance.StartRebind(ctrl, controlName);
        }
        else
        {
            Debug.LogError("[SystemUIButtonClick] 잘못된 버튼 이름 또는 Controll 이름 매칭 실패: " + buttonObject.name);
        }
    }
    public void OnReturn()
    {
        InputManager.instance.ExecuteSystemUIClose();
    }
}
