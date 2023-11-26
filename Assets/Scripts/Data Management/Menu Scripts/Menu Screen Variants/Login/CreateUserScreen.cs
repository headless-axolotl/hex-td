using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Lotl.Generic.Variables;
using Lotl.Data.Users;

namespace Lotl.Data.Menu
{
    public class CreateUserScreen : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private DatabaseManager databaseManager;
        [SerializeField] private UserData baseUserData;

        [Header("UI")]
        [SerializeField] private TMP_InputField usernameInput;
        [SerializeField] private TMP_InputField passwordInput;
        [SerializeField] private TMP_InputField repeatPasswordInput;

        [SerializeField] private TMP_Text feedbackText;

        [SerializeField] private Button createButton;

        [Header("Warning Messages")]
        [SerializeField] private Color warningColor;
        [SerializeField] private StringReference userAlreadyExists;

        [Header("Error Messages")]
        [SerializeField] private Color errorColor;
        [SerializeField] private StringReference emptyField;
        [SerializeField] private StringReference mismatchedPassword;
        [SerializeField] private StringReference databaseError;

        private void OnEnable()
        {
            ResetState();
        }

        private void OnDisable()
        {
            ResetState();
        }

        private void WarnFeedback(string message)
        {
            feedbackText.color = warningColor;
            feedbackText.text = message;
        }

        private void ErrorFeedback(string message)
        {
            feedbackText.color = errorColor;
            feedbackText.text = message;
        }

        private void ResetState()
        {
            usernameInput.text = 
            passwordInput.text =
            repeatPasswordInput.text = string.Empty;

            feedbackText.text = string.Empty;
        }

        public void OnUsernameChanged()
        {
            string userId = usernameInput.text;
            bool userIdIsEmpty = string.IsNullOrEmpty(userId);
            
            if (userIdIsEmpty)
            {
                createButton.interactable = false;
                return;
            }

            bool userExists = databaseManager.UserManager.TrackedUsers.ContainsKey(userId);

            createButton.interactable = !userExists;
            
            if (userExists)
            {
                WarnFeedback(userAlreadyExists);
            }
            else
            {
                feedbackText.text = string.Empty;
            }
        }

        public void CreateUser()
        {
            if (string.IsNullOrEmpty(usernameInput.text) ||
                string.IsNullOrEmpty(passwordInput.text) ||
                string.IsNullOrEmpty(repeatPasswordInput.text))
            {
                ErrorFeedback(emptyField);
                return;
            }

            if (passwordInput.text != repeatPasswordInput.text)
            {
                ErrorFeedback(mismatchedPassword);
                return;
            }

            string userId = usernameInput.text;
            string password = passwordInput.text;

            databaseManager.UserManager.CreateUser(
                userId, password,
                baseUserData, onCompleted: (result) =>
                {
                    if (result.WasSuccessful)
                    {
                        gameObject.SetActive(false);
                        return;
                    }

                    ResetState();
                    ErrorFeedback(databaseError);
                });
        }
    }
}