using System;
using System.Collections.Generic;
using AddressableSystem;
using NMJToolBox;
using UnityEngine;
using Object = UnityEngine.Object;

public class SetListNames : MonoBehaviour
{
    [SerializeField] private string nameForEasyRead;
    [SerializeField] private string catalogPath;
    [SerializeField] private Object[] ma;

    [ContextMenu("Set Addresses")]
    [Button]
    private void SetAddresses()
    {
        var g = GetComponent<TestAddress>();
        g.addressers.Add(new Addresser());
        var c = g.addressers[g.addressers.Count - 1];
        c.name = nameForEasyRead;
        c.cataloguePath = catalogPath;
        c.addresses = new List<string>();
        foreach (var t in ma)
        {
            c.addresses.Add(t.name);
        }

        ma = Array.Empty<Object>();
    }
}