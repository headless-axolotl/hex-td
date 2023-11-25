using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using Lotl.Data.Users;
using Lotl.Generic.Variables;
using Lotl.SceneManagement;

namespace Lotl.Data.Menu
{
    public class LoginScreen : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private DatabaseManager databaseManager;
        [SerializeField] private UserCookie userCookie;
        [SerializeField] private StringReference menuScene;
        [SerializeField] private SceneTransitionInitializer transitionInitializer;

        [Header("Additional Screens")]
        [SerializeField] private ChangePasswordScreen changePasswordScreen;

        [Header("UI")]
        [SerializeField] private TMP_InputField usernameInput;
        [SerializeField] private TMP_InputField passwordInput;

        [SerializeField] private TMP_Text feedbackText;

        [Header("Warning Messages")]
        [SerializeField] private Color warningColor;
        [SerializeField] private StringReference noCreatedUsers;

        [Header("Error Messages")]
        [SerializeField] private Color errorColor;
        [SerializeField] private StringReference emptyField;
        [SerializeField] private StringReference unknownUser;
        [SerializeField] private StringReference wrongPassword;
        
        private void Awake()
        {
            if(databaseManager == null)
            {
                Debug.LogError("Missing database manager!");
            }
        }

        /// <summary>
        /// Called by an event when the DatabaseManager is initialized.
        /// </summary>
        public void Activate()
        {
            userCookie.Set(string.Empty, string.Empty);

            ResetState();
        }

        public void ResetState()
        {
            usernameInput.text =
            passwordInput.text = string.Empty;

            int totalUsers = databaseManager.UserManager.TrackedUsers.Count;
            if (totalUsers == 0)
            {
                feedbackText.color = warningColor;
                feedbackText.text = noCreatedUsers;
            }
            else
            {
                feedbackText.text = string.Empty;
            }
        }

        public void LogIn()
        {
            if (string.IsNullOrEmpty(usernameInput.text) ||
                string.IsNullOrEmpty(passwordInput.text))
            {
                feedbackText.color = errorColor;
                feedbackText.text = emptyField;
                return;
            }

            string userId = usernameInput.text;
            bool userExists = databaseManager.UserManager.TrackedUsers.ContainsKey(userId);

            if (!userExists)
            {
                feedbackText.color = errorColor;
                feedbackText.text = unknownUser;

                usernameInput.text = string.Empty;

                return;
            }

            string password = passwordInput.text;
            bool isCorrectPassword = databaseManager.UserManager.Validate(userId, password);

            if(!isCorrectPassword)
            {
                feedbackText.color = errorColor;
                feedbackText.text = wrongPassword;

                passwordInput.text = string.Empty;

                return;
            }

            userCookie.Set(userId, password);

            transitionInitializer.StartSceneTransition(menuScene);
        }

        public void OpenChangePasswordScreen()
        {
            if (string.IsNullOrEmpty(usernameInput.text))
            {
                feedbackText.color = errorColor;
                feedbackText.text = unknownUser;
                return;
            }

            string userId = usernameInput.text;
            bool userExists = databaseManager.UserManager.TrackedUsers.ContainsKey(userId);

            if (!userExists)
            {
                feedbackText.color = errorColor;
                feedbackText.text = unknownUser;

                usernameInput.text = string.Empty;

                return;
            }

            changePasswordScreen.Enter(userId);
            changePasswordScreen.gameObject.SetActive(true);

            ResetState();
        }
    }
}