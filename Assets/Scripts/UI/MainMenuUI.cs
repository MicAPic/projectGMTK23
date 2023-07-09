using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        public static MainMenuUI Instance;
        
        [Header("Cursor")] 
        [SerializeField] 
        private Texture2D defaultCursor;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }

            Instance = this;
            
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
        
        // Start is called before the first frame update
        // void Start()
        // {
        //     
        // }
        
        // Update is called once per frame
        // void Update()
        // {
        //     
        // }

        public void LoadScene(string sceneToLoad)
        {
            //TODO: transition
            SceneManager.LoadScene(sceneToLoad);
        }
        
        public void Quit()
        {
            #if (UNITY_EDITOR)
            EditorApplication.ExitPlaymode();
            #elif (UNITY_STANDALONE) 
            Application.Quit();
            #elif (UNITY_WEBGL)
            Application.OpenURL("https://micapic.itch.io/kunst");
            #endif
        }
    }
}
