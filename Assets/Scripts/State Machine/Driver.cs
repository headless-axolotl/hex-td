using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.StateMachine
{
    public class Driver : MonoBehaviour
    {
        [Header("Driver")]
        [SerializeField] private BrainBlueprint blueprint;

        private float tickDelay;
        private State currentState = null;

        private Dictionary<State, List<Transition>> transitions = new();
        private List<Transition> currentTransitions = new();
        private List<Transition> anyTransitions = new();

        private static readonly List<Transition> EmptyTransitions = new();

        private bool isPaused = false;

        protected virtual void Awake()
        {
            BuildBrain();
        }

        protected virtual void Start()
        {
            StartCoroutine(DoTicks());
        }

        IEnumerator DoTicks()
        {
            while (true)
            {
                Tick();
                yield return new WaitForSeconds(tickDelay);
            }
        }

        private void BuildBrain()
        {
            if (blueprint == null) return;

            tickDelay = 1 / blueprint.TicksPerSecond;

            foreach (var anyTransition in blueprint.AnyTransitions)
                anyTransitions.Add(anyTransition);

            foreach (var fromTransition in blueprint.FromTransitions)
            {
                if (!transitions.TryGetValue(fromTransition.From, out var _transitions))
                {
                    _transitions = new List<Transition>();
                    transitions[fromTransition.From] = _transitions;
                }
                _transitions.Add(fromTransition.Transition);
            }

            SetState(blueprint.EntryPoint);
        }

        private void Tick()
        {
            if (isPaused) return;

            var transition = GetTransition();
            if(transition != null)
                SetState(transition.StateTo);
            if(currentState != null)
                currentState.Tick(this);
        }

        public void Pause() => isPaused = true;
        
        public void Unpause() => isPaused = false;

        private void SetState(State state)
        {
            if(state == currentState) return;

            if (currentState != null)
                currentState.OnExit(this);
            currentState = state;

            transitions.TryGetValue(currentState, out currentTransitions);

            currentTransitions ??= EmptyTransitions;

            currentState.OnEnter(this);
        }

        private Transition GetTransition()
        {
            foreach (var transition in anyTransitions)
                if (transition.Condition.IsMet(this))
                    return transition;

            foreach (var transition in currentTransitions)
                if (transition.Condition.IsMet(this))
                    return transition;

            return null;
        }
    }
}
