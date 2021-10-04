using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace AddressableSystem
{
    public enum HandleType
    {
        CATALOGUE,
        ADDRESSABLE
    }

    public static class AddressableLoader
    {
        public static readonly Dictionary<HandleType, List<AsyncOperationHandle>> handles =
            new Dictionary<HandleType, List<AsyncOperationHandle>>()
            {
                [HandleType.CATALOGUE] = new List<AsyncOperationHandle>(),
                [HandleType.ADDRESSABLE] = new List<AsyncOperationHandle>(),
            };

        public static string lastContentPath = "";

        /// <summary>
        /// Getting unity object from addressable database using object address.
        /// NOTE! Only one object with the address can be called.
        /// Unity objects: gameObject/prefab, model, material, sprite, etc.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="cataloguePath"></param>
        /// <typeparam name="TUnityObject"></typeparam>
        /// <returns></returns>
        public static async Task<TUnityObject> LoadObjectUsingAddress<TUnityObject>(
            string address, string cataloguePath)
            where TUnityObject : Object
        {
            await LoadNewContentCatalog(cataloguePath);
            AsyncOperationHandle<TUnityObject> handle = Addressables.LoadAssetAsync<TUnityObject>(address);
            await handle;
            handles[HandleType.ADDRESSABLE].Add(handle);
            TUnityObject loadedObject = handle.Result;
            if (loadedObject) return loadedObject;
            throw new NullReferenceException("The loaded object is null. check addressable!");
        }

        /// <summary>
        /// Getting unity objects from addressable database using objects Label.
        /// NOTE! Only every object with the label will be called.
        /// Unity objects: gameObject/prefab, model, material, sprite, etc.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="cataloguePath"></param>
        /// <typeparam name="TUnityObject"></typeparam>
        /// <returns></returns>
        public static async Task<List<TUnityObject>> LoadObjectsUsingLabel<TUnityObject>(
            string label, string cataloguePath)
            where TUnityObject : Object
        {
            await LoadNewContentCatalog(cataloguePath);

            AsyncOperationHandle<IList<TUnityObject>> handle = Addressables.LoadAssetsAsync<TUnityObject>(label, null);
            await handle;
            handles[HandleType.ADDRESSABLE].Add(handle);
            var loadedObjects = (List<TUnityObject>) handle.Result;
            if (loadedObjects.Count > 0) return loadedObjects;
            throw new NullReferenceException("The loaded objects are empty. check addressable!");
        }

        /// <summary>
        /// Loading new Catalogue.
        /// </summary>
        /// <remarks>The old catalogue would be released and no longer usable until loaded anew</remarks>
        /// <param name="cataloguePath">Remote Path to the catalogue location!</param>
        public static async Task LoadNewContentCatalog(string cataloguePath)
        {
            if (lastContentPath.Equals(cataloguePath))
            {
                Debug.Log($"{cataloguePath}: Loaded");
                return;
            }

            lastContentPath = cataloguePath;

            AsyncOperationHandle<IResourceLocator> handler = Addressables.LoadContentCatalogAsync(cataloguePath, true);
            await handler;

            if (handler.Result is null)
            {
                Debug.LogError($"Loaded Catalogue, {cataloguePath}, can't be reached");
                return;
            }

            handles[HandleType.CATALOGUE].Add(handler);
            Debug.Log($"{handler.Result}");
        }

        public static void ReleaseAddressable(AsyncOperationHandle handle)
        {
            Debug.Log($"{handle.IsValid()}");
            Debug.Log($"{handle.Status}");
            Addressables.Release(handle);
            if (handle.IsValid())
                Debug.Log($"{handle.IsValid()}");
        }
    }
}