using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UI;

public class AddressableDownloadManager : MonoBehaviour
{
    public Button download;
    public string label = "Geats";
    private AsyncOperationHandle handle;
    public GameObject obj;
    private void Start()
    {
        StartCoroutine(CheckForUpdates(x => Debug.Log($"Check for updates completed: {x}")));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(DXXX());
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Destroy(obj);
            Debug.Log(handle.Status);
            Addressables.Release(handle);
        }
    }
    // Initialize Addressables
    IEnumerator DXXX()
    {
        handle = Addressables.LoadAssetAsync<GameObject>(label);
        yield return handle;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            obj = handle.Result as GameObject;
            obj = Instantiate(obj);
            Debug.Log("Prefab loaded successfully!");
        }
        else
        {
            Debug.LogError("Failed to load asset.");
        }
    }
    public IEnumerator InitializeAsync()
    {
        var initHandle = Addressables.InitializeAsync();
        yield return initHandle;
        if (initHandle.Status != AsyncOperationStatus.Succeeded)
            Debug.LogError("Addressables initialization failed.");
    }

    // Check for content updates (catalogs)
    public IEnumerator CheckForUpdates(Action<bool> onDone)
    {
        var checkHandle = Addressables.CheckForCatalogUpdates(true);
        yield return checkHandle;

        List<string> catalogsToUpdate = checkHandle.Result;

        if (catalogsToUpdate != null && catalogsToUpdate.Count > 0)
        {
            Debug.Log($"Catalogs to update: {catalogsToUpdate.Count}");
            var updateHandle = Addressables.UpdateCatalogs(catalogsToUpdate);
            yield return updateHandle;

            if (updateHandle.Status == AsyncOperationStatus.Succeeded)
                onDone?.Invoke(true);
            else
                Debug.LogError("Catalog update failed.");
        }
        else
        {
            onDone?.Invoke(false);
        }
    }

    // Download dependencies for a specific address label or key
    public void DownloadAssetsWithLabel(string label, Action<float> onProgress = null, Action onComplete = null)
    {
        Addressables.DownloadDependenciesAsync(label).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Dependencies downloaded!");

                // Step 2: Load the asset
                Addressables.LoadAssetAsync<GameObject>(label).Completed += loadHandle =>
                {
                    if (loadHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        GameObject prefab = loadHandle.Result;

                        // Step 3: Instantiate
                        GameObject instance = Instantiate(prefab);
                        Debug.Log("Prefab instantiated successfully!");
                    }
                    else
                    {
                        Debug.LogError("Failed to load asset.");
                    }
                };
            }
            else
            {
                Debug.LogError("Failed to download dependencies.");
            }
        };
    }

    // Get download size for a label (optional but useful)
    public IEnumerator GetDownloadSize(string label, Action<long> onSizeReceived)
    {
        var sizeHandle = Addressables.GetDownloadSizeAsync(label);
        yield return sizeHandle;

        if (sizeHandle.Status == AsyncOperationStatus.Succeeded)
            onSizeReceived?.Invoke(sizeHandle.Result);
        else
            Debug.LogError("Failed to get download size.");
    }
}
