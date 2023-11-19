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
        [SerializeField] private StringReference emptyPassword;
        [SerializeField] private StringReference mismatchedNewPassword;
        [SerializeField] private StringReference incorrectOldPassword;
        [SerializeField] private StringReference databaseError;

        private string userId = string.Empty;

        public void Enter(string userId)
        {
            this.userId = userId;
            userIdLabel.text = userId;
            ResetState();
        }

        public void Exit()
        {
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
                feedbackText.text = emptyPassword;
                return;
            }

            if (newPasswordInput.text != repeatNewPasswordInput.text)
            {
                feedbackText.text = mismatchedNewPassword;
                return;
            }

            string oldPassword = oldPasswordInput.text;
            bool isOldPasswordValid = databaseManager.UserManager.Validate(userId, oldPassword);

            if (!isOldPasswordValid)
            {
                feedbackText.text = incorrectOldPassword;
                return;
            }

            string newPassword = newPasswordInput.text;

            databaseManager.UserManager.UpdatePassword(
                userId, oldPassword,
                newPassword, onComplete: (result) =>
                {
                    if (result.WasSuccessful)
                        return;

                    ResetState();
                    feedbackText.text = databaseError;
                });
        }
    }
}