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

        private Stack<UIWindow> backStack = new Stack<UIWindow>();

        
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
            OpenWindow(screens[mainScreenIndex]);
            InitializeAllWindows();
        }

        private void InitializeAllWindows()
        {
            screens.ForEach(screen => screen.Initialize(()=>OnBack(ShowExitPopup)));
            popups.ForEach(popup => popup.Initialize(()=>OnBack()));
        }

        public void OpenScreen<T>()
        {
            UIScreen screen = screens.Find(x => x.GetType() == typeof(T));
            OpenWindow(screen);
        }
        
        public T OpenPopup<T>() where T : UIPopup
        {
            UIPopup popup = popups.Find(x => x.GetType() == typeof(T));
            OpenWindow(popup);
            return (T) popup;
        }
        
        private void OpenWindow<T>(T newWindow) where T : UIWindow
        {
            // Hide current active window if of same base type
            if (backStack.Count > 0 && backStack.Peek().GetType().BaseType == typeof(T))
            {
                UIWindow currentWindow = backStack.Peek();
                if (!currentWindow.IsBackStackable)
                {
                    backStack.Pop().Hide();
                }
                else currentWindow.Hide();
            }
            
            if(backStack.Contains(newWindow))
            {
                ClearStackToScreen(newWindow);
            }
            else
            {
                backStack.Push(newWindow);
            }
            
            if(newWindow != null) newWindow.Open();
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
            screens = transform.GetComponentsInChildren<UIScreen>().ToList();

            popups.Clear();
            popups = Resources.FindObjectsOfTypeAll<UIPopup>().ToList();

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        #endif
    }
}
