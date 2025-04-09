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
    }

    // 씬 재활성시 마다 호출
    private void OnEnable()
    {
        count = 0;
        currentTime = 0f;
        TitleSceneView();
        pressKey.SetActive(false);
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

        // 씬을 로딩한 후 설정한 시간을 넘겼을 경우
        if (currentTime > restrictTime)
        {
            currentTime = 0f;
            count++;
        }
        ShowBackground(count);

        if (Input.anyKeyDown && count > 3)
        {
            soundManager.StopEmbeddedBGM();
            GameManager.instance.ChangeScene("Stage1");
        }
    }
    private void ShowBackground(int index)
    {
        logo1.SetActive(index == 0);
        logo2.SetActive(index == 1);
        SelectedObject.SetActive(index >= 2);
        pressKey.SetActive(index >= 2);
    }
}