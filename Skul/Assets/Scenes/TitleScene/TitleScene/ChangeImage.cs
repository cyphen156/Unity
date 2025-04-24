using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ChangeImage : MonoBehaviour
{
    public AudioClip defaultClip;
    public AudioClip HardModeClip;
    public AudioClip selectedClip;
    private int count;
    private int randomValue;
    private Vector3 cameraPosition;
    private float currentTime;
    private float restrictTime;
    public GameObject defaultBackground;
    public GameObject logo1;
    public GameObject logo2;
    public GameObject HardModeBackground;
    private bool sceneChange;
    public GameObject SelectedObject;
    SoundManager soundManager;

    public GameObject pressKey;

    private void Awake()
    {
        soundManager = SoundManager.instance;
    }
    void Start()
    {
        restrictTime = 4.0f;
        UIManager.instance.DeactiveUIPannel(UIManager.instance.BossUIPannel);
        UIManager.instance.DeactiveUIPannel(UIManager.instance.SystemUIPannel);
        UIManager.instance.DeactiveUIPannel(UIManager.instance.PlayerUIPannel);
    }

    // �� ��Ȱ���� ���� ȣ��
    private void OnEnable()
    {
        count = 0;
        currentTime = 0f;
        TitleSceneView();
        pressKey.SetActive(false);
        sceneChange = false;
    }

    private void TitleSceneView()
    {
        randomValue = Random.Range(0, 2);
        if (randomValue == 0)
        {
            selectedClip = defaultClip;
            SelectedObject = defaultBackground;
            Debug.Log("Default Audio Play");
        }
        else
        {   // set Hard Mode
            selectedClip = HardModeClip;
            SelectedObject = HardModeBackground;
            Debug.Log("HardMod Audio Play");
        }
        soundManager.PlayEmbeddedBGM(selectedClip);
    }
    private void Update()
    {
        currentTime += Time.deltaTime;

        // ���� �ε��� �� ������ �ð��� �Ѱ��� ���
        if (currentTime > restrictTime)
        {
            currentTime = 0f;
            count++;
        }
        ShowBackground(count);

        if (Input.anyKeyDown && count > 3)
        {
            soundManager.StopEmbeddedBGM();
            sceneChange = true;
            pressKey.SetActive(false);
            GameManager.instance.ChangeScene("Stage1");
        }
    }
    private void ShowBackground(int index)
    {
        if (sceneChange)
        {
            return;
        }
        logo1.SetActive(index == 0);
        logo2.SetActive(index == 1);
        SelectedObject.SetActive(index >= 2);
        pressKey.SetActive(index >= 2);
    }
}