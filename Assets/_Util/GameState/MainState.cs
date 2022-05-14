using UnityEngine;

namespace _Util.GameState
{
    public class MainState : MonoBehaviour
    {
        private StateMachine stateMachine;

        private void Start()
        {
            stateMachine = new StateMachine();
            stateMachine.Start();
        }

        private void Update()
        {
            stateMachine.Update();
        }

        private void LateUpdate()
        {
            stateMachine.LateUpdate();
        }

        private void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }
    }
}