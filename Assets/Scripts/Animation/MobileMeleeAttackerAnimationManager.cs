using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lotl.Units.Attackers;
using Lotl.Generic.Variables;

namespace Lotl.Animation
{

    [RequireComponent(typeof(MobileMeleeAttacker))]
    public class MobileMeleeAttackerAnimationManager : MonoBehaviour
    {
        private MobileMeleeAttacker driver;
        [SerializeField] private Animator animator;

        [SerializeField] private StringReference idle;
        [SerializeField] private StringReference attack;
        [SerializeField] private StringReference move;

        private void Awake()
        {
            driver = GetComponent<MobileMeleeAttacker>();
            driver.OnAttackAction += () =>
            {
                animator.Play(attack);
            };
            driver.OnStartMoving += () =>
            {
                animator.Play(move);
            };
            driver.OnStartMoving += () =>
            {
                animator.Play(idle);
            };
        }
    }
}