using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾���� ��ȣ�ۿ��� ó���� ���� �Է�ó����
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private GameObject player;
    private PlayerController playerController;
    private GameObject UI;
    private UIManager uiManager;
    // Ű�Է� �̺�Ʈ ����
    public enum Controll
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Scroll,
        ArrowDash,
        Interaction,
        Attack,
        Jump,
        Dash,
        Skill1,
        Skill2,
        Spirit,
        Switching,
        PressEsc
    }

    public enum InputTriggerType
    {
        Down,
        Up,
        Hold
    }

    public enum InputReceiver { None, PlayerOnly, UIOnly }

    private class InputBinding
    {
        public Controll controlType;
        public KeyCode key;
        public InputTriggerType triggerType;
    }

    private InputReceiver currentReceiver = InputReceiver.PlayerOnly;

    // Ű�Է� ó���� ����
    private List<InputBinding> inputBindings = new List<InputBinding>();

    private bool isWaitingForKey = false;
    private Controll pendingControl;
    private string controlNameToUpdate;


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

    // Ȯ���ϰ� UI�� �÷��̾� ��ü�� �����Ǿ��ٴ� ������ ���� �� ������Ʈ�� ������
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        UI = GameObject.FindGameObjectWithTag("UI");
        uiManager = UI.GetComponent<UIManager>();
        InitBinding();
    }

    private void Update()
    {
        if (isWaitingForKey)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    if (key != KeyCode.Escape)
                    {
                        RebindKey(pendingControl.ToString(), key);
                        UIManager.instance.UpdateBindingText(controlNameToUpdate, key);
                    }
                    isWaitingForKey = false;
                    break;
                }
            }
            return;
        }
        for (int i = 0; i < inputBindings.Count; ++i)
        {
            var binding = inputBindings[i];
            bool triggered = false;
            if (binding.triggerType == InputTriggerType.Hold)
            {
                triggered = Input.GetKey(binding.key);
            }
            else if (binding.triggerType == InputTriggerType.Down)
            {
                triggered = Input.GetKeyDown(binding.key);
            }
            else if (binding.triggerType == InputTriggerType.Up)
            {
                triggered = Input.GetKeyUp(binding.key);
            }

            if (!triggered)
            {
                continue;
            }

            switch (currentReceiver)
            {
                case InputReceiver.PlayerOnly:
                    if (!IsMouseKey(binding.key))
                        ExecutePlayerAction(binding.controlType, binding.triggerType);
                    break;
                case InputReceiver.UIOnly:
                    ExecuteUIAction(binding.controlType, binding.triggerType);
                    break;
            }
        }
    }

    private bool IsMouseKey(KeyCode key)
    {
        return key == KeyCode.Mouse0 || key == KeyCode.Mouse1 || key == KeyCode.Mouse2;
    }

    public void SetInputReceiver(InputReceiver receiver)
    {
        currentReceiver = receiver;
    }

    private void RegisterBinding(Controll control, KeyCode key, InputTriggerType trigger)
    {
        inputBindings.Add(new InputBinding
        {
            controlType = control,
            key = key,
            triggerType = trigger
        });
    }
    public void StartRebind(Controll control, string controlName)
    {
        isWaitingForKey = true;
        pendingControl = control;
        controlNameToUpdate = controlName;
        UIManager.instance.ShowRebindingText(controlNameToUpdate);
    }
    public void RebindKey(string controlName, KeyCode newKey)
    {
        if (Enum.TryParse(controlName, out Controll ctrl))
        {
            for (int i = 0; i < inputBindings.Count; ++i)
            {
                if (inputBindings[i].controlType == ctrl)
                {
                    inputBindings[i].key = newKey;
                }
            }
        }
    }

    private void InitBinding()
    {
        inputBindings.Clear();

        RegisterBinding(Controll.MoveUp, KeyCode.UpArrow, InputTriggerType.Hold);
        RegisterBinding(Controll.MoveUp, KeyCode.UpArrow, InputTriggerType.Up);
        RegisterBinding(Controll.MoveDown, KeyCode.DownArrow, InputTriggerType.Hold);
        RegisterBinding(Controll.MoveDown, KeyCode.DownArrow, InputTriggerType.Up);
        RegisterBinding(Controll.MoveLeft, KeyCode.LeftArrow, InputTriggerType.Hold);
        RegisterBinding(Controll.MoveLeft, KeyCode.LeftArrow, InputTriggerType.Up);
        RegisterBinding(Controll.MoveRight, KeyCode.RightArrow, InputTriggerType.Hold);
        RegisterBinding(Controll.MoveRight, KeyCode.RightArrow, InputTriggerType.Up);
        RegisterBinding(Controll.Scroll, KeyCode.Tab, InputTriggerType.Down);
        RegisterBinding(Controll.ArrowDash, KeyCode.T, InputTriggerType.Down);
        RegisterBinding(Controll.Interaction, KeyCode.F, InputTriggerType.Down);
        RegisterBinding(Controll.Attack, KeyCode.X, InputTriggerType.Hold);
        RegisterBinding(Controll.Attack, KeyCode.X, InputTriggerType.Up);
        RegisterBinding(Controll.Jump, KeyCode.C, InputTriggerType.Down);
        RegisterBinding(Controll.Dash, KeyCode.Z, InputTriggerType.Down);
        RegisterBinding(Controll.Skill1, KeyCode.A, InputTriggerType.Down);
        RegisterBinding(Controll.Skill2, KeyCode.S, InputTriggerType.Down);
        RegisterBinding(Controll.Spirit, KeyCode.D, InputTriggerType.Down);
        RegisterBinding(Controll.Switching, KeyCode.Space, InputTriggerType.Down);
        RegisterBinding(Controll.PressEsc, KeyCode.Escape, InputTriggerType.Down);
        Debug.Log("[InputManager] :: Ű ���ε� �ʱ�ȭ �Ϸ�");
    }

    public KeyCode GetDefaultKey(Controll control)
    {
        switch (control)
        {
            case Controll.MoveUp: 
                return KeyCode.UpArrow;
            case Controll.MoveDown: 
                return KeyCode.DownArrow;
            case Controll.MoveLeft: 
                return KeyCode.LeftArrow;
            case Controll.MoveRight: 
                return KeyCode.RightArrow;
            case Controll.Scroll: 
                return KeyCode.Tab;
            case Controll.ArrowDash: 
                return KeyCode.T;
            case Controll.Interaction: 
                return KeyCode.F;
            case Controll.Attack: 
                return KeyCode.X;
            case Controll.Jump: 
                return KeyCode.C;
            case Controll.Dash: 
                return KeyCode.Z;
            case Controll.Skill1: 
                return KeyCode.A;
            case Controll.Skill2: 
                return KeyCode.S;
            case Controll.Spirit: 
                return KeyCode.D;
            case Controll.Switching: 
                return KeyCode.Space;
            default: return KeyCode.None;
        }
    }
    private void ExecutePlayerAction(Controll control, InputTriggerType trigger)
    {
        switch (control)
        {
            case Controll.MoveUp:
                {
                    if (trigger == InputTriggerType.Hold)
                    {
                        playerController.MoveUp(); 
                    }
                    else
                    {
                        playerController.StopMove();
                    }
                }
                break;
            case Controll.MoveDown:
                {
                    if (trigger == InputTriggerType.Hold)
                    {
                        playerController.MoveDown();
                    }
                    else
                    {
                        playerController.StopMove();
                    }
                }
                break;
            case Controll.MoveLeft:
                {
                    if (trigger == InputTriggerType.Hold)
                    {
                        playerController.MoveLeft();
                    }
                    else
                    {
                        playerController.StopMove();
                    }
                }
                break;
            case Controll.MoveRight:
                {
                    if (trigger == InputTriggerType.Hold)
                    {
                        playerController.MoveRight();
                    }
                    else
                    {
                        playerController.StopMove();
                    }
                }
                break;
            case Controll.Attack:
                {
                    if (trigger == InputTriggerType.Hold)
                    {
                        playerController.Attack();
                    }
                    else
                    {
                        playerController.KeyUp("IsAttacking");
                    }
                }
                break;
            case Controll.Jump:
                playerController.Jump(); 
                break;
            case Controll.Dash:
                playerController.Dash(); 
                break;
            case Controll.Skill1:
                playerController.UseSkill1(); 
                break;
            case Controll.Skill2:
                playerController.UseSkill2(); 
                break;
            case Controll.Spirit:
                playerController.UseSpirit(); 
                break;
            case Controll.Switching:
                playerController.Switching(); 
                break;
            case Controll.Interaction:
                playerController.Interact(); 
                break;
            case Controll.ArrowDash:
                playerController.ArrowDash();
                break;
            case Controll.Scroll:
                playerController.Scroll(); 
                break;
            case Controll.PressEsc:
                {
                    if (currentReceiver == InputReceiver.PlayerOnly)
                    {
                        ExecuteSystemUIOpen();
                    }
                    else
                    {
                        ExecuteSystemUIClose();
                    }
                }
                // �޴� Ȱ��ȭ�ϱ�
                break;
        }
    }

    private void ExecuteUIAction(Controll control, InputTriggerType trigger)
    {
        if (Input.anyKey)
        {
            switch (control)
            {
                case Controll.PressEsc:
                    {
                        if (currentReceiver == InputReceiver.PlayerOnly)
                        {
                            ExecuteSystemUIOpen();
                        }
                        else
                        {
                            ExecuteSystemUIClose();
                        }
                    }
                    // �޴� Ȱ��ȭ�ϱ�
                    break;
            }
        }
        //uiManager.ProcessUIInput(control.ToString(), trigger.ToString());
    }
    public void ExecuteSystemUIOpen()
    {
        GameManager.instance.PauseGame();
        uiManager.ActiveUIPannel(uiManager.SystemUIPannel);
        SetInputReceiver(InputReceiver.UIOnly);
    }
    public void ExecuteSystemUIClose()
    {
        GameManager.instance.ResumeGame();
        uiManager.DeactiveUIPannel(uiManager.SystemUIPannel);
        SetInputReceiver(InputReceiver.PlayerOnly);
    }
    public void ResetKeyBindings()
    {
        InitBinding();
        UIManager.instance.UpdateAllBindingTexts();
    }
}
