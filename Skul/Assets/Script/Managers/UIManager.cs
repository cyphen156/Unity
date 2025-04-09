using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject LoadingPannel;
    
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
}
