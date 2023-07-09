using System.Collections;
using Audio;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class InGameUI : MonoBehaviour
    {
        public static InGameUI Instance;

        [Header("Cursor")] 
        [SerializeField] 
        private Texture2D defaultCursor; 
        [SerializeField] 
        private Texture2D dragCursor;

        [Header("Pause")]
        [SerializeField]
        private float pauseTransitionDuration = 1.0f;
        [SerializeField] 
        private Vector2 pauseBrushStartLocation;
        [SerializeField] 
        private Vector2 pauseBrushEndLocation;
        [SerializeField]
        private CanvasGroup pauseMenuGroup;
        
        [Header("End Transition")] 
        [SerializeField]
        private float transitionDuration = 1.0f;
        [SerializeField] 
        private RectTransform[] paintBrushes;
        [SerializeField] 
        private Vector2[] paintBrushStartLocations;
        [SerializeField] 
        private Vector2[] paintBrushEndLocations;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }

            Instance = this;

            SetCursor(false);
        }
        
        // Start is called before the first frame update
        IEnumerator Start()
        {
            if (paintBrushStartLocations.Length != paintBrushEndLocations.Length)
            {
                Debug.LogWarning("You done goofed! Arrays mismatch");
            }
            
            for (var i = 0; i < paintBrushes.Length; i++)
            {
                var paintBrush = paintBrushes[i];
                paintBrush.anchoredPosition = paintBrushStartLocations[i];
                paintBrush.DOAnchorPos(paintBrushEndLocations[i], transitionDuration);
            }

            yield return new WaitForSeconds(transitionDuration);
            GameManager.Instance.canPause = true;
        }
        
        // Update is called once per frame
        void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape) || !GameManager.Instance.canPause) return;
            
            var paintBrush = paintBrushes[0];
            if (GameManager.Instance.isPaused)
            {
                Time.timeScale = 1.0f;
                paintBrush.DOAnchorPos(pauseBrushStartLocation, pauseTransitionDuration).SetUpdate(true);
                pauseMenuGroup.DOFade(0.0f, pauseTransitionDuration).SetUpdate(true);
            }
            else
            {
                Time.timeScale = 0.0f;
                paintBrush.anchoredPosition = pauseBrushStartLocation;
                paintBrush.DOAnchorPos(pauseBrushEndLocation, pauseTransitionDuration).SetUpdate(true);
                paintBrush.GetComponent<Shadow>().enabled = true;
                pauseMenuGroup.DOFade(1.0f, pauseTransitionDuration).SetUpdate(true);
            }

            GameManager.Instance.isPaused = !GameManager.Instance.isPaused;
        }

        public void AnimateTransition(string sceneToLoad)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
            asyncOperation.allowSceneActivation = false;

            var paintBrush = paintBrushes[0];
            paintBrush.GetComponent<Shadow>().enabled = false;
            paintBrush.anchoredPosition = paintBrushEndLocations[0];
            paintBrush.DOAnchorPos(paintBrushStartLocations[0], transitionDuration).SetUpdate(true);
            
            paintBrushes[^1].DOAnchorPos(paintBrushStartLocations[^1], transitionDuration)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    Time.timeScale = 1.0f;
                    asyncOperation.allowSceneActivation = true;
                });
        }

        public void SetCursor(bool isDragging)
        {
            if (isDragging)
            {
                Cursor.SetCursor(dragCursor, Vector2.zero, CursorMode.ForceSoftware);
                return;
            }
            
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}
