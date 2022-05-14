using Audio;
using UnityEngine;
using _Util;
using _Util.GameState;
using UnityEngine.SceneManagement;
using System.Collections;

namespace _Application
{
    public abstract class BaseState : GameState<StateMachine>
    {
        protected AudioManager AudioManager => AudioManager.Current;
        protected StageLoader StageLoader => StageLoader.Current;
        protected AssetsFacade AssetsFacade => AssetsFacade.Current;

        public static StageId LoadStageId { get; private set; } = StageId.None;
        public static bool IsLoad => (LoadStageId != StageId.None);

        public BaseState(StateMachine stateMachine) : base(stateMachine) { }

        public override void OnStateEnter()
        {
            Debug.Log("LoadState : " + GetType().Name);

            StageLoader.OnLoadedStage += OnLoadedStage;
            StageLoader.OnUnloadedStage += OnUnloadedStage;

            InitializeAllObjects();
        }

        protected void InitializeAllObjects()
        {
            AudioManager.Voice.Stop();
            AudioManager.BGM.Stop();
            AudioManager.Ambient.Stop();
            AudioManager.SE.Stop();

            LoadStageId = StageId.None;
        }

        public override void OnStateUpdate()
        {
            if (AssetsFacade.Current)
            {
                AssetsFacade.InvokeUpdateAssets();
            }
        }

        public override void OnStateFixedUpdate()
        {
            if (AssetsFacade.Current)
            {
                AssetsFacade.InvokeFixedUpdateAssets();
            }
        }

        public override void OnStateLateUpdate()
        {
            base.OnStateLateUpdate();

            if (AssetsFacade.Current)
            {
                AssetsFacade.InvokeLateUpdateAssets();
            }
        }

        public override void OnStateExit()
        {
            StageLoader.OnLoadedStage -= OnLoadedStage;
            StageLoader.OnUnloadedStage -= OnUnloadedStage;
        }

        protected void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public static void LoadStage(StageId stageId)
        {
            LoadStageId = stageId;
        }

        public static BaseState LoadState(StageId stageId, StateMachine stateMachine)
        {
            return stageId switch
            {
                StageId.None => new TemplateState(stateMachine),
                _ => throw new System.ComponentModel.InvalidEnumArgumentException(),
            };
        }

        public virtual void OnLoadedStage()
        {
            if (AssetsFacade.Current)
            {
                AssetsFacade.InvokeSetupAssets();
            }

            StartCoroutine(CoLoadedState());
        }

        private IEnumerator CoLoadedState()
        {
            yield return new WaitUntil(() => AssetsFacade.Current);

            AssetsFacade.InvokeStartAssets();
        }

        public virtual void OnUnloadedStage()
        {
            if (AssetsFacade.Current)
            {
                AssetsFacade.InvokExitAssets();
            }
        }
    }
}