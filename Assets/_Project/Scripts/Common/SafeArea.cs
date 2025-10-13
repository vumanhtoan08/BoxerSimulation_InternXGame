using System.Collections.Generic;
using UnityEngine;
using System;
// using mygame.sdk;
using UniRx;
// using master;
namespace Crystal.DX
{
    /// <summary>
    /// Safe area implementation for notched mobile devices. Usage:
    ///  (1) Add this component to the top level of any GUI panel. 
    ///  (2) If the panel uses a full screen background image, then create an immediate child and put the component on that instead, with all other elements childed below it.
    ///      This will allow the background image to stretch to the full extents of the screen behind the notch, which looks nicer.
    ///  (3) For other cases that use a mixture of full horizontal and vertical background stripes, use the Conform X & Y controls on separate elements as needed.
    /// </summary>
    public class SafeArea : Singleton<SafeArea>
    {
        private List<SafeAreaPanel> Panel = new List<SafeAreaPanel>();
        Rect LastSafeArea = new Rect(0, 0, 0, 0);
        Vector2Int LastScreenSize = new Vector2Int(0, 0);
        ScreenOrientation LastOrientation = ScreenOrientation.AutoRotation;
        [SerializeField] bool ConformX = true;  // Conform to screen safe area on X-axis (default true, disable to ignore)
        [SerializeField] bool ConformY = true;  // Conform to screen safe area on Y-axis (default true, disable to ignore)
        [SerializeField] bool Logging = false;  // Conform to screen safe area on Y-axis (default true, disable to ignore)
        private bool isRemoveAds;
        protected override void Awake()
        {
            base.Awake();
            UIManager.OnChangeScreen += OnChangeScreen;
            RegisterListener();
        }
        protected bool hasRegisterEvent;
        private IDisposable subShopPack;
        // private IDisposable sub;
        private void RegisterListener()
        {
            if (hasRegisterEvent) return;
            hasRegisterEvent = true;
            // var ob_shopPack = master.Observer.GetObservable(ObserverName.purchase_success, 0);
            // subShopPack = ob_shopPack.Subscribe(x => { Refresh(); });
            // var ob1 = master.Observer.GetObservable(ObserverName.screen_resize, 0);
            // sub = ob1.Subscribe(x =>
            // {
            //     Refresh();
            // });
        }
        private void RemoveListener()
        {
            if (!hasRegisterEvent) return;
            hasRegisterEvent = false;
            subShopPack.Dispose();
        }
        private void OnDestroy()
        {
            RemoveListener();
        }
        private void OnChangeScreen(Vector2 vector)
        {
            Refresh();
        }
#if UNITY_EDITOR
        private void Update()
        {
            Refresh();
        }
#endif

        void Refresh()
        {
            //TODO
            // Debug.Log($"Screen Size:{Screen.width}:{Screen.height}");
            // isRemoveAds = AdsHelper.isRemoveAds(0);
            Rect safeArea = GetSafeArea();

            // if (Screen.width != LastScreenSize.x
            //     || Screen.height != LastScreenSize.y
            //     || Screen.orientation != LastOrientation)
            // {
            // Fix for having auto-rotate off and manually forcing a screen orientation.
            // See https://forum.unity.com/threads/569236/#post-4473253 and https://forum.unity.com/threads/569236/page-2#post-5166467
            LastScreenSize.x = Screen.width;
            LastScreenSize.y = Screen.height;
            LastOrientation = Screen.orientation;

            ApplySafeArea(safeArea);
            // }
        }

        Rect GetSafeArea()
        {
            Rect safeArea = Screen.safeArea;
            return safeArea;
        }

        void ApplySafeArea(Rect r)
        {
            // LastSafeArea = r;

            // Ignore x-axis?
            if (!ConformX)
            {
                r.x = 0;
                r.width = Screen.width;
            }

            // Ignore y-axis?
            if (!ConformY)
            {
                r.y = 0;
                r.height = Screen.height;
            }

            // Check for invalid screen startup state on some Samsung devices (see below)
            if (Screen.width > 0 && Screen.height > 0)
            {
                // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
                Vector2 anchorMin = r.position;
                Vector2 anchorMax = r.position + r.size;
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;
                //TODO
                // float height = SdkUtil.GetSizeBanner();
                float height = 0;
                // Fix for some Samsung devices (e.g. Note 10+, A71, S20) where Refresh gets called twice and the first time returns NaN anchor coordinates
                // See https://forum.unity.com/threads/569236/page-2#post-6199352
                if (anchorMin.x >= 0 && anchorMin.y >= 0 && anchorMax.x >= 0 && anchorMax.y >= 0)
                {
                    for (int i = 0; i < Panel.Count; i++)
                    {
                        Panel[i].rectTransform.anchorMin = anchorMin;
                        Panel[i].rectTransform.anchorMax = anchorMax;
                        if (Panel[i].isSetBanner && !isRemoveAds)
                        {
                            Panel[i].rectTransform.anchorMin = new Vector2(0, height / Screen.height);
                        }
                    }

                }
            }
            if (Logging)
            {
                Debug.LogFormat("New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}",
                name, r.x, r.y, r.width, r.height, Screen.width, Screen.height);
            }

        }
        public void AddPanel(SafeAreaPanel newPanel)
        {
            if (Panel.Contains(newPanel)) return;
            Panel.Add(newPanel);
            Refresh();
        }
        public void RemovePanel(SafeAreaPanel panel)
        {
            if (!Panel.Contains(panel)) return;
            Panel.Remove(panel);
        }
    }
}
