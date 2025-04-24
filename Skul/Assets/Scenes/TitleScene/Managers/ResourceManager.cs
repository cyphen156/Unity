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
                ChangeAssets();
                Debug.Log($"[GameManager] 씬 {sceneName} 비동기 로드 완료");
            }
            else
            {
                Debug.LogError($"[GameManager] 씬 {sceneName} 비동기 로딩 실패");
            }

            PlayerManager.instance.SetStart();
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

    private void ChangeAssets()
    {
        SetUpBackground();
        SetUpBossUIPannel();
    }
    private void SetUpBackground()
    {
        GameObject bg = GameObject.FindGameObjectWithTag("ScrollBackground");
        if (bg != null)
        {
            if (bg.GetComponent<ScrollBackground>() == null)
            {
                bg.AddComponent<ScrollBackground>();
                Debug.Log("[ResourceManager] ScrollBackground 스크립트 자동 부착 완료");
            }
        }
        else
        {
            if (bg.GetComponent<FixedBackground>() == null)
            {
                bg.AddComponent<FixedBackground>();
            }
            Debug.Log("[ResourceManager] ScrollBackground 태그 객체 없음");
            Debug.Log("[ResourceManager] FixedBackground 스크립트 자동 부착 완료");
        }
    }
    private void SetUpBossUIPannel()
    {
        GameObject bossUIPannel = UIManager.instance.GetBossUIPannel();

        // 기존 UI 제거
        foreach (Transform child in bossUIPannel.transform)
        {
            if (child.name == "BossUIFirst" || child.name == "BossUISecond")
            {
                Destroy(child.gameObject);
                Debug.Log($"[ResourceManager] 기존 {child.name} 파괴");
            }
        }

        // 새 UI 장착
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject loadCanvas = GameObject.FindGameObjectWithTag("LoadCanvas");
        GameObject[] candidatesFirst = GameObject.FindGameObjectsWithTag("BossUIFirst");

        GameObject changeUIFirst = null;
        foreach (var c in candidatesFirst)
        {
            if (c.scene == currentScene)
            {
                changeUIFirst = c;
                break;
            }
        }

        if (changeUIFirst == null)
        {
            Debug.LogWarning("[ResourceManager] BossUIFirst 태그를 가진 오브젝트가 없습니다.");
        }

        else
        {
            changeUIFirst.transform.SetParent(bossUIPannel.transform, false);
            Debug.Log("[ResourceManager] BossUIFirst 교체 완료");
        }

        GameObject[] candidatesSecond = GameObject.FindGameObjectsWithTag("BossUISecond");

        GameObject changeUISecond = null;
        foreach (var c in candidatesSecond)
        {
            if (c.scene == currentScene)
            {
                changeUISecond = c;
                break;
            }
        }
        if (changeUISecond == null)
        {
            Debug.LogWarning("[ResourceManager] BossUISecond 태그를 가진 오브젝트가 없습니다.");
        }
        else
        {
            changeUISecond.transform.SetParent(bossUIPannel.transform, false);
            Debug.Log("[ResourceManager] BossUISecond 교체 완료");
        }

        UIManager.instance.DeactiveUIPannel(bossUIPannel);
        Destroy(loadCanvas);
        Debug.Log("[ResourceManager] 로드용 UI Canvas 삭제");
    }
}
