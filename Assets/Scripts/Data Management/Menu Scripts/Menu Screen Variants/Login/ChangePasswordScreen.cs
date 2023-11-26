using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lotl.Generic.Variables;

namespace Lotl.Data.Menu
{
    public class ChangePasswordScreen : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private DatabaseManager databaseManager;

        [Header("UI")]
        [SerializeField] private TMP_Text userIdLabel;
        [SerializeField] private TMP_InputField oldPasswordInput;
        [SerializeField] private TMP_InputField newPasswordInput;
        [SerializeField] private TMP_InputField repeatNewPasswordInput;

        [SerializeField] private TMP_Text feedbackText;

        [Header("Error Messages")]
        [SerializeField] private Color errorColor;
        [SerializeField] private StringReference emptyPassword;
        [SerializeField] private StringReference mismatchedNewPassword;
        [SerializeField] private StringReference incorrectOldPassword;
        [SerializeField] private StringReference databaseError;

        private string currentUserId = string.Empty;

        private void ErrorFeedback(string message)
        {
            feedbackText.color = errorColor;
            feedbackText.text = message;
        }

        public void Activate(string userId)
        {
            userIdLabel.text =
            currentUserId = userId;
        }

        private void OnEnable()
        {
            ResetState();
        }

        private void OnDisable()
        {
            userIdLabel.text =
            currentUserId = string.Empty;
            
            ResetState();
        }

        private void ResetState()
        {
            oldPasswordInput.text =
            newPasswordInput.text =
            repeatNewPasswordInput.text = string.Empty;

            feedbackText.text = string.Empty;
        }

        public void ChangePassword()
        {
            if (string.IsNullOrEmpty(oldPasswordInput.text) ||
                string.IsNullOrEmpty(newPasswordInput.text) ||
                string.IsNullOrEmpty(repeatNewPasswordInput.text))
            {
                ErrorFeedback(emptyPassword);
                return;
            }

            if (newPasswordInput.text != repeatNewPasswordInput.text)
            {
                ErrorFeedback(mismatchedNewPassword);
                return;
            }

            string oldPassword = oldPasswordInput.text;
            bool isOldPasswordValid = databaseManager.UserManager.Validate(currentUserId, oldPassword);

            if (!isOldPasswordValid)
            {
                ErrorFeedback(incorrectOldPassword);
                return;
            }

            string newPassword = newPasswordInput.text;

            databaseManager.UserManager.UpdatePassword(
                currentUserId, oldPassword, newPassword,
            onCompleted: (result) =>
            {
                ResetState();

                if (result.WasSuccessful)
                {
                    gameObject.SetActive(false);
                    return;
                }

                ErrorFeedback(databaseError);
            });
        }
    }
}