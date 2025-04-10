using System.Net;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgmSource; //배경음 재생용 AudioSource
    public AudioSource sfxSource; //효과음 재생용 AudioSource

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
    /// 앱에 내장된 BGM 재생
    /// </summary>
    public void PlayEmbeddedBGM(AudioClip clip, bool loop = true)
    {
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }
    /// <summary>
    /// 앱에 내장된 BGM 정지
    /// </summary>
    public void StopEmbeddedBGM()
    {
        bgmSource.Stop();
    }
    /// <summary>
    /// 어드레서블로 BGM 로드 및 재생
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
            Debug.LogError("address" + "BGM 로딩 실패");
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
            Debug.LogError("[SoundManager] SFX 로딩 실패");
        }

        // 필요 시 캐싱 or Release(handle);
    }
}
