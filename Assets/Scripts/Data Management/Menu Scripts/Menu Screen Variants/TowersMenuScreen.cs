using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Lotl.UI;
using Lotl.Data.Towerset;
using Lotl.Utility;

namespace Lotl.Data.Menu
{
    public class TowersMenuScreen : MenuScreen
    {
        #region Properties

        [Header("UI")]
        [SerializeField] private DataView createdTowersetsView;
        [SerializeField] private DataView availableTowersView;
        [SerializeField] private DataView towersInCurrentTowersetView;

        [SerializeField] private Button createButton;
        [SerializeField] private Button updateButton;
        [SerializeField] private Button deleteButton;

        [SerializeField] private TMP_InputField nameInputField;

        #endregion
    }
}
