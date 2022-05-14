using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using _Util;

namespace _Application
{
    public class StageLoader : MonoBehaviour
    {
        public static StageLoader Current { get; private set; }

        private FadeManager FadeManager => FadeManager.Current;

        const string MainSceneName = "Main";

        public static StageId CurrentStage = StageId.None;

        public bool IsLoadComplete = false;

        public Action OnLoadedStage;
        public Action OnUnloadedStage;

        private string originalSceneName;

        public static StageId SceneNameToStageId(string sceneName)
        {
            return (StageId)Enum.Parse(typeof(StageId), sceneName);
        }

        private static string StageIdToSceneName(StageId stageId)
        {
            return stageId.ToString();
        }

        private void Awake()
        {
            Current = this;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void Start()
        {
            originalSceneName = SceneManager.GetActiveScene().name;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            Debug.Log("Load : " + scene.name + ", " + loadSceneMode.ToString());

            OnLoadedStage?.Invoke();
        }

        private void OnSceneUnloaded(Scene scene)
        {
            Debug.Log("Unload : " + scene.name);

            OnUnloadedStage?.Invoke();
        }

        public void LoadStage(StageId stageId)
        {
            CurrentStage = stageId;

            StartCoroutine(CoLoadStageAsync(stageId));
        }

        public YieldInstruction LoadStageAsync(StageId stageId)
        {
            CurrentStage = stageId;

            return StartCoroutine(CoLoadStageAsync(stageId));
        }

        private IEnumerator CoLoadStageAsync(StageId stageId)
        {
            IsLoadComplete = false;

            if (Stage.Current)
            {
                yield return UnloadCurrentStageAsync();
            }
            else
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    yield return UnloadCurrentStageAsync();
                }
            }

            string sceneName = StageIdToSceneName(stageId);

            if (!ContainsScene(sceneName))
            {
                AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

                async.allowSceneActivation = false;

                FadeManager.isFadeOut = true;

                yield return new WaitUntil(() => !FadeManager.isFadeOut);
                yield return new WaitForSeconds(0.5f);

                yield return new WaitUntil(() => !FadeManager.isFadeIn);

                yield return new WaitUntil(() => async.progress >= 0.9f);//0.9で止まる

                FadeManager.isFadeIn = true;

                async.allowSceneActivation = true;
            }

            if (!Stage.Current)
            {
                GameObject o = new GameObject("Stage");
                Scene scene = SceneManager.GetSceneByName(sceneName);
                SceneManager.MoveGameObjectToScene(o, scene);
                o.AddComponent<Stage>();
            }

            // シングルトン宣言待ち
            yield return null;

            IsLoadComplete = true;
        }

        public YieldInstruction UnloadCurrentStageAsync()
        {
            if (!Stage.Current)
            {
                string initilizeSceneName = StageIdToSceneName(StageId.None);
                if (SceneManager.GetSceneByName(initilizeSceneName).IsValid())
                {
                    return null;
                }
                else
                {
                    for (int i = 0; i < SceneManager.sceneCount; i++)
                    {
                        string activeSceneName = SceneManager.GetSceneAt(i).name;
                        if (activeSceneName != MainSceneName)
                        {
                            return SceneManager.UnloadSceneAsync(activeSceneName);
                        }
                    }

                    return null;
                }
            }
            else
            {
                return SceneManager.UnloadSceneAsync(Stage.Current.gameObject.scene);
            }
        }

        public void ReloadOriginalScene()
        {
            StartCoroutine(CoReloadOriginalScene());
        }

        private IEnumerator CoReloadOriginalScene()
        {
            yield return UnloadCurrentStageAsync();

            SceneManager.LoadScene(originalSceneName);
        }

        private bool ContainsScene(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == sceneName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}