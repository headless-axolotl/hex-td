using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lotl.Units.Attackers;
using Lotl.Generic.Variables;

namespace Lotl.Animation
{
    [RequireComponent(typeof(MobileRangedAttacker))]
    public class MobileRangedAttackerAnimationManager : MonoBehaviour
    {
        private MobileRangedAttacker driver;
        [SerializeField] private Animator animator;

        [SerializeField] private StringReference idle;
        [SerializeField] private StringReference attack;
        [SerializeField] private StringReference move;

        private void Awake()
        {
            driver = GetComponent<MobileRangedAttacker>();
            driver.OnShootAction += () =>
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