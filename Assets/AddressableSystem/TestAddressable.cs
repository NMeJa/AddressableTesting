using System.Diagnostics;
using NMJToolBox;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Debug = UnityEngine.Debug;

public class TestAddressable : MonoBehaviour
{
    public GameObject[] references;
    public string[] contentNames;
    public string def;

    private void Start()
    {
        def = contentNames[0];
    }

    [Button]
    public void LoadAssetByAddress()
    {
        Create(references[0].name);
    }

    private void Create(string str)
    {
        var s = str.Substring(0, contentNames[0] == def
                                  ? str.Length
                                  : str.Length - 1);
        Debug.Log($"{s}");
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        Debug.Log($"Started Cat: {stopWatch.Elapsed}");
        Addressables.LoadContentCatalogAsync(contentNames[0], true).Completed += loaded =>
            {
                Debug.Log($"{loaded.Result.LocatorId}");
                stopWatch.Stop();
                Debug.Log($"Stopped Cat: {stopWatch.Elapsed}");
            };
        stopWatch.Restart();
        Debug.Log($"Started ADdreess: {stopWatch.Elapsed}");
        Addressables
            .LoadAssetAsync<GameObject>(s)
            .Completed += loaded =>
            {
                Instantiate(loaded.Result);
                stopWatch.Stop();
                Debug.Log($"Stopped Arrdess: {stopWatch.Elapsed}");
            };
    }
}