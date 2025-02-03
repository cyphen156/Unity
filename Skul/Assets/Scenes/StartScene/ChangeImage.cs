using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class ChangeImage : MonoBehaviour
{
    public AudioSource defaultAudio;
    public AudioSource HardModeAudio;
    public AudioSource selectedAudio;

    private int randomValue;
    private Vector3 cameraPosition;
    private float deltaTime;
    private float clickTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // default Position Value (0, 0, 0)
        clickTime = 10;
    }

    // �� ��Ȱ���� ���� ȣ��
    private void OnEnable()
    {
        deltaTime = 0f;
        randomValue = Random.Range(0, 2);
        if (randomValue == 0)
        {
            selectedAudio = defaultAudio;
            cameraPosition = new Vector3(0, 0, 0);
            Debug.Log("Default Audio Play");
        }
        else
        {   // set Hard Mode
            selectedAudio = HardModeAudio;
            cameraPosition = new Vector3(20, 0, 0);
            Debug.Log("HardMod Audio Play");
        }
        transform.position = cameraPosition;
        selectedAudio.Play();
    }
    // Update is called once per frame
    private void Update()
    {
        deltaTime += Time.deltaTime;

        // ���� �ε��� �� ������ �ð��� �Ѱ��� ���
        if (Time.deltaTime > clickTime)
        {
            // �ƹ� Ű �Է½� ���� ��ȯ��
            if (Input.anyKeyDown)
            {
                selectedAudio.Stop();
                SceneManager.LoadScene(1);
            }
        }
    }
    void FixedUpdate()
    {
        
    }

}
