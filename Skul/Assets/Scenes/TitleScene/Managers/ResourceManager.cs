using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    public string key = string.Empty;
    public List<string> keys = new List<string>();

    private AsyncOperationHandle<GameObject> handle;
    private AsyncOperationHandle<IList<GameObject>> labelHandle;

    private Dictionary<string, GameObject> loadedAssets = new Dictionary<string, GameObject>();
    
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

    // 개별 로딩 /// 씬 로드시 사용 - 그룹 로딩 연결

    public void LoadScene(string sceneName)
    {
        // 씬 관련 애셋 프리로드
        if (!LoadAssetsWithLabelSync(sceneName))
        {
            Debug.LogError($"[GameManager] 리소스 동기 로딩 실패: {sceneName}");
            return;
        }

        // 씬 로드
        var sceneHandle = Addressables.LoadSceneAsync(sceneName + "Scene", LoadSceneMode.Single);
        sceneHandle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"[GameManager] 씬 {sceneName} 비동기 로드 완료");
            }
            else
            {
                Debug.LogError($"[GameManager] 씬 {sceneName} 비동기 로딩 실패");
            }
        };
    }

    // 개별 에셋 비동기 로드
    public void LoadAsset(string key)
    {
        handle = Addressables.LoadAssetAsync<GameObject>(key);
        handle.Completed += Handle_Completed;
    }

    private void Handle_Completed(AsyncOperationHandle<GameObject> operation)
    {
        if (operation.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(operation.Result, transform);
        }
        else
        {
            Debug.LogError($"Asset for {key} failed to load.");
        }
    }

    // 개별 해제
    private void ReleaseResource()
    {
        if (handle.IsValid())
        {
            Addressables.Release(handle);
            handle = default;
        }
    }

    // 그룹 동기 로딩 /// 씬과 연관된 애셋 로드
    public bool LoadAssetsWithLabelSync(string label)
    {
        if (!keys.Contains(label))
        {
            keys.Add(label);
        }

        UIManager.instance.StartLoad();

        // 번들 동기 다운로드
        var downloadHandle = Addressables.DownloadDependenciesAsync(label);
        downloadHandle.WaitForCompletion();


        // 번들 동기 로드
        labelHandle = Addressables.LoadAssetsAsync<GameObject>(
            label,
            obj =>
            {
                if (obj != null && !loadedAssets.ContainsKey(obj.name))
                    loadedAssets[obj.name] = obj;
            });

        labelHandle.WaitForCompletion();

        UIManager.instance.EndLoad();
        return labelHandle.Status == AsyncOperationStatus.Succeeded;
    }
    private void LoadHandle_Completed(AsyncOperationHandle<IList<GameObject>> operation)
    {
        if (operation.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogWarning("Some assets did not load.");
        }

        UIManager.instance.EndLoad();
    }

    // 그룹 해제
    private void ReleaseResources()
    {
        if (labelHandle.IsValid())
        {
            Addressables.Release(labelHandle);
            labelHandle = default;
        }
    }

    public GameObject GetAsset(string name)
    {
        if (loadedAssets.TryGetValue(name, out var obj))
        {
            return obj;
        }
        Debug.LogWarning($"[ResourceManager] GetAsset 실패: {name}가 로딩되지 않음");
        return null;
    }

    private void OnDestroy()
    {
        ReleaseResource();
        ReleaseResources();
    }
}
