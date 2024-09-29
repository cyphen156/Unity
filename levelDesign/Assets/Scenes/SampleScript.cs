using UnityEngine;

public class SampleScript : MonoBehaviour
{

    // 프레임을 기록하기 위한 변수
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
        // 프레임 카운트 증가
        frameCount++;

        // 지정된 프레임 수가 지난 경우 로그 출력
        if (frameCount >= nextFrame)
        {
            Debug.Log($"Frame Count: {frameCount}");

            // 다음 로그를 찍을 프레임 설정 (로그를 출력할 때마다 간격을 늘려감)
            interval++;      // 간격을 1씩 늘림
            nextFrame += interval;  // 다음 로그를 찍을 프레임 수를 설정
        }
    }
}
