using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Lotl.UI
{
    [RequireComponent(typeof(TMP_InputField))]
    public class ShowHidePassword : MonoBehaviour
    {
        private TMP_InputField inputField;

        private void Awake()
        {
            inputField = GetComponent<TMP_InputField>();
        }

        public void Switch()
        {
            TMP_InputField.ContentType content = inputField.contentType;
            if (content == Standard) content = Password;
            else content = Standard;
            inputField.contentType = content;
            inputField.ForceLabelUpdate();
        }

        private TMP_InputField.ContentType Standard = TMP_InputField.ContentType.Standard;
        private TMP_InputField.ContentType Password = TMP_InputField.ContentType.Password;
    }
}