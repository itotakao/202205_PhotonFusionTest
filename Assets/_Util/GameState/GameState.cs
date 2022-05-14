using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Util.GameState
{
    public class TransitionFunction
    {
        public readonly Func<GameState> function;
        public bool mute;

        public TransitionFunction(Func<GameState> function, bool mute = false)
        {
            this.function = function;
            this.mute = mute;
        }
    }

    public abstract class GameState
    {
        public List<TransitionFunction> TransitionFunctions { get; private set; }

        public GameStateMachine StateMachine { get; private set; }

        public GameObject GameObject { get; private set; }
        GameStateObject gameStateObject;

        public GameState(GameStateMachine stateMachine)
        {
            TransitionFunctions = new List<TransitionFunction>();
            StateMachine = stateMachine;
        }

        public void OnStateInitialize()
        {
            GameObject = new GameObject(StateMachine.GetType().Name + "." + GetType().Name);
            gameStateObject = GameObject.AddComponent<GameStateObject>();
        }

        public void OnStateDestroy()
        {
            StopAllCoroutines();
            UnityEngine.Object.Destroy(GameObject);
        }

        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }
        public virtual void OnStateUpdate() { }
        public virtual void OnStateLateUpdate() { }
        public virtual void OnStateFixedUpdate() { }

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return gameStateObject.StartCoroutine(routine);
        }

        public void StopCoroutine(IEnumerator routine)
        {
            gameStateObject.StopCoroutine(routine);
        }

        public void StopAllCoroutines()
        {
            gameStateObject.StopAllCoroutines();
        }
    }

    public abstract class GameState<T> : GameState
        where T : GameStateMachine
    {
        public new T StateMachine { get { return base.StateMachine as T; } }

        public GameState(T stateMachine) : base(stateMachine) { }
    }
}