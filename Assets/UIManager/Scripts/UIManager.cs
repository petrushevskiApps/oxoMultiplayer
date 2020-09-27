using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

namespace PetrushevskiApps.UIManager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private int mainScreenIndex = 0;
        [SerializeField] private bool dontDestroyOnLoad = false;
        
        [SerializeField] private List<UIScreen> screens = new List<UIScreen>();
        [SerializeField] private List<UIPopup> popups = new List<UIPopup>();

        private static Stack<UIWindow> backStack = new Stack<UIWindow>();

        
        public static UIManager Instance;
        
        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                if(dontDestroyOnLoad) DontDestroyOnLoad(Instance.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            OpenScreen<UIMainScreen>();
            InitializeAllWindows();
        }

        private void InitializeAllWindows()
        {
            screens.ForEach(screen => screen.Initialize(()=>OnBack(ShowExitPopup)));
            popups.ForEach(screen => screen.Initialize(()=>OnBack()));
        }
        
        public void OpenScreen<T>()
        {
            UIScreen screen = screens.Find(x => x.GetType() == typeof(T));
            OpenWindow(screen);
        }
        
        public void OpenPopup<T>()
        {
            UIPopup popup = popups.Find(x => x.GetType() == typeof(T));
            OpenWindow(popup);
        }
        
        private void OpenWindow<T>(T window) where T : UIWindow
        {
            // Hide current active window if of same base type
            if (backStack.Count > 0 && backStack.Peek().GetType().BaseType == typeof(T))
            {
                backStack.Peek().Hide();
            }
            
            if(backStack.Contains(window))
            {
                ClearStackToScreen(window);
            }
            else
            {
                backStack.Push(window);
            }
            
            if(window != null) window.Open();
        }
        
        public void OnBack(Action onEmptyStack = null)
        {
            if (backStack.Count > 1)
            {
                backStack.Pop().Close();
                backStack.Peek().Show();    
            }
            else onEmptyStack?.Invoke();
        }
        
        private void ClearStackToScreen(UIWindow screen)
        {
            while(backStack.Count > 0)
            {
                if (!backStack.Peek().Equals(screen))
                {
                    backStack.Pop().Close();
                }
                else break;
            }
        }

        private void ShowExitPopup()
        {
            OpenPopup<UIExitPopup>();
        }
        public void CloseApplication()
        {
            Application.Quit();
            Debug.Log("Closing App");
        }
        #if UNITY_EDITOR
        [ContextMenu("Collect Windows In Scene")]
        public void CollectWindowsInScene()
        {
            screens.Clear();
            screens = Resources.FindObjectsOfTypeAll<UIScreen>().ToList();
            
            popups.Clear();
            popups = Resources.FindObjectsOfTypeAll<UIPopup>().ToList();

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        #endif
    }
}
