using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.StateMachine
{
    public class Driver : MonoBehaviour
    {
        [SerializeField] private BrainBlueprint blueprint;
        [SerializeField] private State currentState;

        private Dictionary<State, List<Transition>> transitions = new();
        private List<Transition> currentTransitions = new();
        private List<Transition> anyTransitions = new();

        private static readonly List<Transition> EmptyTransitions = new();

        protected virtual void Awake()
        {
            BuildBrain();
        }

        private void FixedUpdate()
        {
            Tick();
        }

        private void BuildBrain()
        {
            if (blueprint == null) return;

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
            var transition = GetTransition();
            if(transition != null)
                SetState(transition.StateTo);
            if(currentState != null)
                currentState.Tick(this);
        }

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
