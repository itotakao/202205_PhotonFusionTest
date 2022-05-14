using System.Collections;
using UnityEngine;
using _Util.GameState;

namespace _Application
{
    public class TemplateState : BaseState
    {
        public TemplateState(StateMachine stateMachine) : base(stateMachine)
        {
            TransitionFunctions.Add(new TransitionFunction(Transition));
        }

        private BaseState Transition()
        {
            return (IsLoad ? LoadState(LoadStageId, StateMachine) : null);
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();

            StageLoader.LoadStage(StageId.None);

            StartCoroutine(CoStateEnter());
        }

        private IEnumerator CoStateEnter()
        {
            yield return new WaitUntil(() => StageLoader.IsLoadComplete);

            yield return new WaitUntil(() => false);

            LoadStage(StageId.None);
        }

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();


        }

        public override void OnStateFixedUpdate()
        {
            base.OnStateFixedUpdate();


        }

        public override void OnStateLateUpdate()
        {
            base.OnStateLateUpdate();


        }

        public override void OnStateExit()
        {
            base.OnStateExit();


        }
    }
}