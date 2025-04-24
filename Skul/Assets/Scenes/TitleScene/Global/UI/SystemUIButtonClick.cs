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
            Debug.LogError("[SystemUIButtonClick] �߸��� ��ư �̸� �Ǵ� Controll �̸� ��Ī ����: " + buttonObject.name);
        }
    }
    public void OnReturn()
    {
        InputManager.instance.ExecuteSystemUIClose();
    }
}
