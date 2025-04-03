using System.Net;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgmSource; //����� ����� AudioSource
    public AudioSource sfxSource; //ȿ���� ����� AudioSource

    public static SoundManager instance;

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

    /// <summary>
    /// �ۿ� ����� BGM ���
    /// </summary>
    public void PlayEmbeddedBGM(AudioClip clip, bool loop = true)
    {
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }
    /// <summary>
    /// �ۿ� ����� BGM ����
    /// </summary>
    public void StopEmbeddedBGM()
    {
        bgmSource.Stop();
    }
    /// <summary>
    /// ��巹����� BGM �ε� �� ���
    /// </summary>
    public void PlayAddressableBGM(string address, bool loop = true)
    {
        AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(address);
        handle.Completed += OnBGMClipLoaded;
    }

    private void OnBGMClipLoaded(AsyncOperationHandle<AudioClip> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            bgmSource.clip = handle.Result;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            Debug.LogError("address" + "BGM �ε� ����");
        }

        // Addressables.Release(handle);
    }

    public void PlaySFX(string address)
    {
        AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(address);
        handle.Completed += OnSFXClipLoaded;
    }

    private void OnSFXClipLoaded(AsyncOperationHandle<AudioClip> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            sfxSource.PlayOneShot(handle.Result);
        }
        else
        {
            Debug.LogError("[SoundManager] SFX �ε� ����");
        }

        // �ʿ� �� ĳ�� or Release(handle);
    }
}
