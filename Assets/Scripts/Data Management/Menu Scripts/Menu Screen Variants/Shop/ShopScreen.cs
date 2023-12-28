using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;

using Lotl.UI;
using Lotl.Data.Users;
using Lotl.Generic.Variables;
using Lotl.Data.Towerset;

namespace Lotl.Data.Menu
{
    public class ShopScreen : TabbedScreen
    {
        [Header("Data")]
        [SerializeField] private DatabaseManager databaseManager;
        [SerializeField] private UserCookie userCookie;

        [Header("UI")]
        [SerializeField] private DataView lockedTowersView;
        [SerializeField] private DataView unlockedTowersView;
        [SerializeField] private Button purchaseButton;
        [SerializeField] private TMP_Text currentUserMetaCurrencyAmountText;
        [SerializeField] private TMP_Text feedbackText;

        [Header("Prompts")]
        [SerializeField] private Prompt prompt;
        [SerializeField] private StringReference purchasePromptQuestion;
        [SerializeField] private StringReference purchasePromptDescription;
        [SerializeField] private StringReference selectedTowerStringFormat;

        [Header("Warning Messages")]
        [SerializeField] private Color warningColor;
        [SerializeField] private StringReference noSelectedTower;
        [SerializeField] private StringReference notEnoughCurrency;

        [Header("Error Messages")]
        [SerializeField] private Color errorColor;
        [SerializeField] private StringReference databaseError;

        private int currentUserMetaCurrencyAmount;
        private HashSet<TowerToken> unlockedTowers = new();

        private void Awake()
        {
            if (databaseManager == null)
            {
                Debug.LogError("Missing database manager!");
            }
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

        public void Activate()
        {
            feedbackText.text = string.Empty;

            databaseManager.UserManager.GetUserData(userCookie.UserId, onCompleted: (result, userData) =>
            {
                if(!result.WasSuccessful)
                {
                    ErrorFeedback(databaseError);
                    Debug.LogWarning(result.Message);
                    return;
                }

                unlockedTowers.Clear();
                unlockedTowers.AddRange(userData.UnlockedTowers.TowerTokens);
                
                currentUserMetaCurrencyAmount = userData.MetaCurrencyAmount;

                UpdateUnlockedTowersView(unlockedTowers);
                UpdateCurrentUserMetaCurrencyAmountText(currentUserMetaCurrencyAmount);
                UpdateLockedTowersView(databaseManager.TowerTokenLibrary.TowerTokens);
            });
        }

        #region Data Event View Methods
        
        private void OnEnable()
        {
            Activate();

            lockedTowersView.OnSelect += OnLockedTowerSelected;
            lockedTowersView.OnDeselect += OnLockedTowerDeselected;
        }

        private void OnDisable()
        {
            lockedTowersView.OnSelect -= OnLockedTowerSelected;
            lockedTowersView.OnDeselect -= OnLockedTowerDeselected;
        }

        private void OnLockedTowerSelected()
        {
            purchaseButton.interactable = true;
        }

        private void OnLockedTowerDeselected()
        {
            purchaseButton.interactable = false;
        }

        #endregion

        #region Utility

        private void UpdateUnlockedTowersView(IEnumerable<TowerToken> unlockedTowers)
        {
            IEnumerable<object> unlockedTowersViewData = unlockedTowers
                    .OrderBy(towerToken => towerToken.IndexInLibrary)
                    .Cast<object>();

            unlockedTowersView.SetData(
                unlockedTowersViewData,
                Conversions.ConvertTowerToken);
        }

        private void UpdateLockedTowersView(IEnumerable<TowerToken> allTowerTokens)
        {
            IEnumerable<object> lockedTowersViewData = allTowerTokens
                .Where(towerToken => !unlockedTowers.Contains(towerToken))
                .OrderBy(towerToken => towerToken.IndexInLibrary)
                .Cast<object>();

            lockedTowersView.SetData(
                lockedTowersViewData,
                Conversions.ConvertTowerTokenWtihShopCost);
        }

        private void UpdateCurrentUserMetaCurrencyAmountText(int amount)
        {
            currentUserMetaCurrencyAmountText.text = $"{amount}€";
        }

        #endregion

        public void PromptPurchaseSelectedLockedTower()
        {
            if(lockedTowersView.SelectedEntry == null)
            {
                WarnFeedback(noSelectedTower);
                return;
            }


            int selectedIndex = lockedTowersView.SelectedEntry.Index;
            TowerToken selectedTowerToken = lockedTowersView.Data[selectedIndex] as TowerToken;

            if(selectedTowerToken == null)
            {
                Debug.LogWarning("Locked towers view does not hold Tower Tokens!");
                return;
            }

            if (selectedTowerToken.ShopCost > currentUserMetaCurrencyAmount)
            {
                WarnFeedback(notEnoughCurrency);
                return;
            }

            string promptDescription =
                purchasePromptDescription + "\n" +
                string.Format(selectedTowerStringFormat, selectedTowerToken.TowerName);

            prompt.Activate(
                purchasePromptQuestion,
                promptDescription,
                ConfirmPurchaseSelectedLockedTower,
                null);
        }

        public void ConfirmPurchaseSelectedLockedTower()
        {
            int selectedIndex = lockedTowersView.SelectedEntry.Index;
            TowerToken selectedTowerToken = lockedTowersView.Data[selectedIndex] as TowerToken;
            unlockedTowers.Add(selectedTowerToken);

            int newCurrentUserMetaCurrencyAmount = currentUserMetaCurrencyAmount - selectedTowerToken.ShopCost;
            if (newCurrentUserMetaCurrencyAmount < 0) newCurrentUserMetaCurrencyAmount = 0;

            TowersetInfo unlockedTowersTowersetInfo = new(unlockedTowers);
            
            UserData updatedUserData = new(
                newCurrentUserMetaCurrencyAmount,
                unlockedTowersTowersetInfo);

            databaseManager.UserManager.UpdateUserData(
                userCookie.UserId,
                userCookie.Password,
                updatedUserData,
            onCompleted: (result) =>
            {
                if(!result.WasSuccessful)
                {
                    Debug.LogWarning(result.Message);
                    ErrorFeedback(databaseError);
                    unlockedTowers.Remove(selectedTowerToken);
                    return;
                }

                unlockedTowers.Add(selectedTowerToken);
                currentUserMetaCurrencyAmount = newCurrentUserMetaCurrencyAmount;
                
                UpdateUnlockedTowersView(unlockedTowers);
                UpdateCurrentUserMetaCurrencyAmountText(currentUserMetaCurrencyAmount);
                lockedTowersView.RemoveEntry(selectedIndex);
            });
        }
    }
}
