using UnityEngine;

public class SampleScript : MonoBehaviour
{

    // �������� ����ϱ� ���� ����
    int frameCount = 0;
    int interval = 1;
    int nextFrame = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Hello Wolrd!");
    }

    // Update is called once per frame
    void Update()
    {
        // ������ ī��Ʈ ����
        frameCount++;

        // ������ ������ ���� ���� ��� �α� ���
        if (frameCount >= nextFrame)
        {
            Debug.Log($"Frame Count: {frameCount}");

            // ���� �α׸� ���� ������ ���� (�α׸� ����� ������ ������ �÷���)
            interval++;      // ������ 1�� �ø�
            nextFrame += interval;  // ���� �α׸� ���� ������ ���� ����
        }
    }
}
