using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lotl.UI
{
    [RequireComponent(typeof(TMP_InputField))]
    public class ShowHidePassword : MonoBehaviour
    {
        private TMP_InputField inputField;
        [SerializeField] private Button showButton;
        [SerializeField] private Button hideButton;

        private void Awake()
        {
            inputField = GetComponent<TMP_InputField>();
        }

        public void Switch()
        {
            TMP_InputField.ContentType contentType = inputField.contentType;
            if (contentType == Standard) Hide();
            else Show();
        }

        public void Show()
        {
            inputField.contentType = Standard;
            inputField.ForceLabelUpdate();
            
            if (showButton != null) showButton.gameObject.SetActive(false);
            if (hideButton != null) hideButton.gameObject.SetActive(true);
        }

        public void Hide()
        {
            inputField.contentType = Password;
            inputField.ForceLabelUpdate();
            
            if (showButton != null) showButton.gameObject.SetActive(true);
            if (hideButton != null) hideButton.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            Hide();
        }

        private const TMP_InputField.ContentType Standard = TMP_InputField.ContentType.Standard;
        private const TMP_InputField.ContentType Password = TMP_InputField.ContentType.Password;
    }
}