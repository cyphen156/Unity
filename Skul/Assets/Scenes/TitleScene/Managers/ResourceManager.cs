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

    // ���� �ε� /// �� �ε�� ��� - �׷� �ε� ����

    public void LoadScene(string sceneName)
    {
        // �� ���� �ּ� �����ε�
        if (!LoadAssetsWithLabelSync(sceneName))
        {
            Debug.LogError($"[GameManager] ���ҽ� ���� �ε� ����: {sceneName}");
            return;
        }

        // �� �ε�
        var sceneHandle = Addressables.LoadSceneAsync(sceneName + "Scene", LoadSceneMode.Single);
        sceneHandle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"[GameManager] �� {sceneName} �񵿱� �ε� �Ϸ�");
            }
            else
            {
                Debug.LogError($"[GameManager] �� {sceneName} �񵿱� �ε� ����");
            }
        };
    }

    // ���� ���� �񵿱� �ε�
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

    // ���� ����
    private void ReleaseResource()
    {
        if (handle.IsValid())
        {
            Addressables.Release(handle);
            handle = default;
        }
    }

    // �׷� ���� �ε� /// ���� ������ �ּ� �ε�
    public bool LoadAssetsWithLabelSync(string label)
    {
        if (!keys.Contains(label))
        {
            keys.Add(label);
        }

        UIManager.instance.StartLoad();

        // ���� ���� �ٿ�ε�
        var downloadHandle = Addressables.DownloadDependenciesAsync(label);
        downloadHandle.WaitForCompletion();


        // ���� ���� �ε�
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

    // �׷� ����
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
        Debug.LogWarning($"[ResourceManager] GetAsset ����: {name}�� �ε����� ����");
        return null;
    }

    private void OnDestroy()
    {
        ReleaseResource();
        ReleaseResources();
    }
}
