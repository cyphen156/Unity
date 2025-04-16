using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어와의 상호작용을 처리할 전역 입력처리기
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private GameObject player;
    private PlayerController playerController;
    private GameObject UI;
    private UIManager uiManager;

    // 키입력 이벤트 정의
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
        Sprit,
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

    // 키입력 처리기 매핑
    private List<InputBinding> inputBindings = new List<InputBinding>();

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

    // 확실하게 UI와 플레이어 객체가 생성되었다는 보장을 받은 뒤 오브젝트를 가져옴
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
        for (int i = 0; i < inputBindings.Count; ++i)
        {
            var binding = inputBindings[i];
            bool triggered = false;
            if (binding.triggerType == InputTriggerType.Hold)
                triggered = Input.GetKey(binding.key);
            else if (binding.triggerType == InputTriggerType.Down)
                triggered = Input.GetKeyDown(binding.key);
            else if (binding.triggerType == InputTriggerType.Up)
                triggered = Input.GetKeyUp(binding.key);

            if (!triggered) continue;

            switch (currentReceiver)
            {
                case InputReceiver.PlayerOnly:
                    if (!IsMouseKey(binding.key))
                        ExecutePlayerAction(binding.controlType, binding.triggerType);
                    break;
                case InputReceiver.UIOnly:
                    ExecuteUIAction();
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

    public void RebindKey(string controlName, KeyCode newKey)
    {
        if (Enum.TryParse(controlName, out Controll ctrl))
        {
            foreach (var b in inputBindings)
            {
                if (b.controlType == ctrl)
                {
                    b.key = newKey;
                    break;
                }
            }
        }
    }

    private void InitBinding()
    {
        RegisterBinding(Controll.MoveUp, KeyCode.UpArrow, InputTriggerType.Hold);
        RegisterBinding(Controll.MoveDown, KeyCode.DownArrow, InputTriggerType.Hold);
        RegisterBinding(Controll.MoveLeft, KeyCode.LeftArrow, InputTriggerType.Hold);
        RegisterBinding(Controll.MoveRight, KeyCode.RightArrow, InputTriggerType.Hold);
        RegisterBinding(Controll.Scroll, KeyCode.Tab, InputTriggerType.Down);
        RegisterBinding(Controll.ArrowDash, KeyCode.T, InputTriggerType.Down);
        RegisterBinding(Controll.Interaction, KeyCode.F, InputTriggerType.Down);
        RegisterBinding(Controll.Attack, KeyCode.X, InputTriggerType.Hold);
        RegisterBinding(Controll.Jump, KeyCode.C, InputTriggerType.Down);
        RegisterBinding(Controll.Dash, KeyCode.Z, InputTriggerType.Down);
        RegisterBinding(Controll.Skill1, KeyCode.A, InputTriggerType.Down);
        RegisterBinding(Controll.Skill2, KeyCode.S, InputTriggerType.Down);
        RegisterBinding(Controll.Sprit, KeyCode.D, InputTriggerType.Down);
        RegisterBinding(Controll.Switching, KeyCode.Space, InputTriggerType.Down);
        RegisterBinding(Controll.PressEsc, KeyCode.Escape, InputTriggerType.Down);

    }
    private void ExecutePlayerAction(Controll control, InputTriggerType trigger)
    {
        switch (control)
        {
            case Controll.MoveUp:
                playerController.MoveUp(); 
                break;
            case Controll.MoveDown:
                playerController.MoveDown(); 
                break;
            case Controll.MoveLeft:
                playerController.MoveLeft(); 
                break;
            case Controll.MoveRight:
                playerController.MoveRight(); 
                break;
            case Controll.Attack:
                playerController.Attack(); 
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
            case Controll.Sprit:
                playerController.UseSprit(); 
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
                // 메뉴 활성화하기
                break;
        }
    }

    private void ExecuteUIAction()
    {
        if (Input.anyKey)
        {
            
        }
        //uiManager.ProcessUIInput(control.ToString(), trigger.ToString());
    }
}
