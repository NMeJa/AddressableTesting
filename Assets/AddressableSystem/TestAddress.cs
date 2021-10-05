using System;
using System.Collections.Generic;
using System.Diagnostics;
using NMJToolBox;
using RuntimeInspectorNamespace;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace AddressableSystem
{
    [Serializable]
    public class Addresser
    {
        public string name;
        public string cataloguePath;
        public List<string> addresses;
    }

    public class TestAddress : MonoBehaviour
    {
        [Range(0, 6)] public int mainIndex;
        [Range(0, 19)] public int secondaryIndex;
        public static TestAddress instance;
        public string lastContentPath;

        public List<Addresser> addressers;
        private void Awake() => instance = this;

        [ContextMenu("Load Catalogue")]
        [Button] [RuntimeInspectorButton("Load Content", false, ButtonVisibility.InitializedObjects)]
        private async void LoadContent()
        {
            var st = new Stopwatch();
            st.Start();
            Debug.Log($"Catalogue S Time: {st.Elapsed}");
            await AddressableLoader.LoadNewContentCatalog(addressers[mainIndex].cataloguePath);
            lastContentPath =
                AddressableLoader.lastContentPath.Substring(AddressableLoader.lastContentPath.Length - 50);
            st.Stop();
            Debug.Log($"Catalogue F Time: {st.Elapsed}");
        }

        public GameObject chosen;

        [ContextMenu("Load Address")]
        [Button] [RuntimeInspectorButton("Load GameObject", false, ButtonVisibility.InitializedObjects)]
        private async void LoadAddressableByAddress()
        {
            var st = new Stopwatch();
            st.Start();
            Debug.Log($"Address S Time: {st.Elapsed}");

            chosen =
                await AddressableLoader
                    .LoadObjectUsingAddress<GameObject>(addressers[mainIndex].addresses[secondaryIndex],
                                                        addressers[mainIndex]
                                                            .cataloguePath);
            st.Stop();
            Debug.Log($"Address F Time: {st.Elapsed}");
        }

        public Sprite chosenSprite;

        [ContextMenu("Load Sprite")]
        [Button] [RuntimeInspectorButton("Load Sprite", false, ButtonVisibility.InitializedObjects)]
        private async void LoadAddressableByAddressSprite()
        {
            var st = new Stopwatch();
            st.Start();
            chosenSprite =
                await AddressableLoader.LoadObjectUsingAddress<Sprite>(addressers[mainIndex].addresses[secondaryIndex],
                                                                       addressers[mainIndex].cataloguePath);
            st.Stop();
            Debug.Log($"Address Time: {st.Elapsed}");
        }

        [ContextMenu("Create GameObject")]
        [Button] [RuntimeInspectorButton("Create GameObject", false, ButtonVisibility.InitializedObjects)]
        private void CreateGameObject()
        {
            Instantiate(chosen);
        }

        [ContextMenu("Create Sprite")]
        [Button] [RuntimeInspectorButton("Create Sprite", false, ButtonVisibility.InitializedObjects)]
        private void CreateSprite()
        {
            Instantiate(chosenSprite);
        }

        [ContextMenu("Check")]
        [Button] [RuntimeInspectorButton("Check Handlers", false, ButtonVisibility.InitializedObjects)]
        private void CheckHandlers()
        {
            Debug.Log($"Catalogues");
            var catalogues = AddressableLoader.handles[HandleType.CATALOGUE];
            for (int index = 0; index < catalogues.Count; index++)
            {
                var element = catalogues[index];
                if (!element.IsValid())
                {
                    Debug.Log($"This index: {index} Wasn't valid!");
                    continue;
                }

                Debug.Log($"Name: {element.Result} | Status: {element.Status} | Valid: {element.IsValid()}");
            }

            Debug.Log($"Addressables");
            var addressables = AddressableLoader.handles[HandleType.ADDRESSABLE];
            for (int index = 0; index < addressables.Count; index++)
            {
                var element = addressables[index];
                if (!element.IsValid())
                {
                    Debug.Log($"This index: {index} Wasn't valid!");
                    continue;
                }

                Debug.Log($"Name: {element.Result} | Status: {element.Status} | Valid: {element.IsValid()}");
            }
        }
    }
}