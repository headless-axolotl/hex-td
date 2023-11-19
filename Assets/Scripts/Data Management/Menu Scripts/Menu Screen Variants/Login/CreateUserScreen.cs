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

        public void Enter()
        {
            ResetState();
        }

        public void Exit()
        {
            ResetState();
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
            bool userExists = databaseManager.UserManager.TrackedUsers.ContainsKey(userId);

            createButton.interactable = !userExists;

            if (userExists)
            {
                feedbackText.color = warningColor;
                feedbackText.text = userAlreadyExists;
            }
        }

        public void CreateUser()
        {
            if (string.IsNullOrEmpty(usernameInput.text) ||
                string.IsNullOrEmpty(passwordInput.text) ||
                string.IsNullOrEmpty(repeatPasswordInput.text))
            {
                feedbackText.color = errorColor;
                feedbackText.text = emptyField;
                return;
            }

            if (passwordInput.text != repeatPasswordInput.text)
            {
                feedbackText.color = errorColor;
                feedbackText.text = mismatchedPassword;
                return;
            }

            string userId = usernameInput.text;
            string password = passwordInput.text;

            databaseManager.UserManager.CreateUser(
                userId, password,
                baseUserData, onComplete: (result) =>
                {
                    if (result.WasSuccessful)
                        return;

                    ResetState();
                    feedbackText.color = errorColor;
                    feedbackText.text = databaseError;
                });
        }
    }
}