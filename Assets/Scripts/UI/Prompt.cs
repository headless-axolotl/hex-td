using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Lotl.UI
{
    public class Prompt : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text promptQuestion;
        [SerializeField] private TMP_Text promptDescription;

        private Action confirmAction = null;
        private Action cancelAction = null;

        public void Activate(string question, string description, Action confirm, Action cancel)
        {
            promptQuestion.text = question;
            promptDescription.text = description;

            confirmAction = confirm;
            cancelAction = cancel;

            gameObject.SetActive(true);
        }

        public void Confirm()
        {
            confirmAction?.Invoke();
            gameObject.SetActive(false);
        }

        public void Cancel()
        {
            cancelAction?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
