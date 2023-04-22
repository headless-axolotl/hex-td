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

        private Dictionary<Type, List<Transition>> transitions = new();
        private List<Transition> currentTransitions = new();
        private List<Transition> anyTransitions = new();

        private static List<Transition> EmptyTransitions = new();

        private void Awake()
        {
            BuildBrain();
        }

        private void Update()
        {
            Tick();
        }

        private void BuildBrain()
        {
            foreach(var anyTransition in blueprint.AnyTransitions)
                anyTransitions.Add(anyTransition);

            foreach(var fromTransition in blueprint.FromTransitions)
            {
                Type fromType = fromTransition.From.GetType();
                if (!transitions.TryGetValue(fromType, out var _transitions))
                {
                    _transitions = new List<Transition>();
                    transitions[fromType] = _transitions;
                }
                _transitions.Add(fromTransition.Transition);
            }
        }

        private void Tick()
        {
            var transition = GetTransition();
            if(transition != null)
                SetState(transition.To);
            if(currentState != null)
                currentState.Tick(this);
        }

        private void SetState(State state)
        {
            if(state == currentState) return;

            if (currentState != null)
                currentState.OnExit(this);
            currentState = state;

            transitions.TryGetValue(currentState.GetType(), out currentTransitions);

            currentTransitions ??= EmptyTransitions;

            currentState.OnEnter(this);
        }

        private Transition GetTransition()
        {
            foreach (var transition in anyTransitions)
                if (transition.Check(this))
                    return transition;

            foreach (var transition in currentTransitions)
                if (transition.Check(this))
                    return transition;

            return null;
        }

    }
}
