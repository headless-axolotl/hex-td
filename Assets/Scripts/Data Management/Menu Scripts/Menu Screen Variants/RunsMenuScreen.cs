using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Lotl.UI;
using Lotl.Data.Runs;
using Lotl.Data.Towerset;

namespace Lotl.Data.Menu
{
    public class RunsMenuScreen : TabbedScreen
    {
        #region Properties
        
        [Header("UI")]
        [SerializeField] private DataView createdRunsView;
        [SerializeField] private DataView createdRunTowersetPreview;
        [SerializeField] private DataView creationSubmenuTowersetPreview;

        [SerializeField] private Button openSubmenuButton;
        [SerializeField] private Button createButton;
        [SerializeField] private Button playButton;
        [SerializeField] private Button deleteButton;

        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Dropdown runTemplateDropdown;
        [SerializeField] private TMP_Dropdown towersetDropdown;

        [SerializeField] private GameObject creationSubmenu;

        #endregion

    }
}
