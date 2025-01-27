using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BountyBoardUI : MonoBehaviour
{
    // ------------------------------
    //
    // This updates and manages the UI when the player opens/closes the bounty board
    //
    // ------------------------------

    // Public variables
    [Header("UI Object References")]
    public GameObject BountyButtonPrefab;
    public GameObject ConditionBoxPrefab;
    public GameObject RewardBoxPrefab;

    [Space(10)]
    public TMP_Text BountyNameText;
    public TMP_Text BountyDescriptionText;
    public GameObject BountyButtonContainer;
    public GameObject ConditionsContainer;
    public GameObject RewardsContainer;
    public GameObject BountyConfirmButton;
    public GameObject BountyCompleteButton;
    public GameObject ToggleObject;

    // Private variables
    private Animator _selectionAnim;
    private BountyObject _selectedBounty;
    private Dictionary<BountyObject, GameObject[]> _conditionButtons = new Dictionary<BountyObject, GameObject[]>();
    private Dictionary<BountyObject, GameObject[]> _rewardButtons = new Dictionary<BountyObject, GameObject[]>();
    private BountyBoardConfirmUI _confirmUI;

    [HideInInspector] public BountyBoard _bountyBoard;
    [HideInInspector] public GameObject _selectionUI;
    [HideInInspector] public GameObject _menuUI;

    // Start is called before the first frame update
    void Start()
    {
        _selectionUI = transform.GetChild(0).gameObject;
        _selectionAnim = GetComponent<Animator>();
        _menuUI = transform.GetChild(1).gameObject;
        _confirmUI = BountyConfirmButton.GetComponent<BountyBoardConfirmUI>();

        _selectionUI.SetActive(false);
        _menuUI.SetActive(false);

        // Creates selectable buttons for all available bounties
        foreach (BountyObject bounty in _bountyBoard.Bounties)
        {
            GenerateBountyButton(bounty);
        }
    }

    public void Open()
    {
        _selectedBounty = null;
        BountyNameText.text = "";
        BountyDescriptionText.text = "";

        ToggleObject.SetActive(false);

        // Display all completed bounties to the user
        foreach (BountyObject bounty in _bountyBoard.Bounties)
        {
            bounty._button.SetColour(bounty.IsCompleted);
        }
    }

    public void GenerateBountyButton(BountyObject newBounty)
    {
        BountyBoardUIButton button = Instantiate(BountyButtonPrefab, BountyButtonContainer.transform).GetComponent<BountyBoardUIButton>();
        button.SetUp(newBounty, this);
        newBounty._button = button;

        GenerateConditionsList(newBounty);

        GenerateRewardsList(newBounty);
    }

    private void GenerateConditionsList(BountyObject bounty)
    {
        // Show the player a list of all the conditions they need to complete
        List<GameObject> collectInfo = new List<GameObject>(bounty.BountyInfo.CollectConditions.Count);
        foreach (ConditionCollect condition in bounty.BountyInfo.CollectConditions)
        {
            GameObject conditionText = Instantiate(ConditionBoxPrefab, ConditionsContainer.transform);
            collectInfo.Add(conditionText);

            conditionText.GetComponentInChildren<TMP_Text>().text = condition.ObjectDescription;
        }

        List<GameObject> exploreInfo = new List<GameObject>(bounty.BountyInfo.ExploreConditions.Count);
        foreach (ConditionExplore condition in bounty.BountyInfo.ExploreConditions)
        {
            GameObject conditionText = Instantiate(ConditionBoxPrefab, ConditionsContainer.transform);
            exploreInfo.Add(conditionText);

            conditionText.GetComponentInChildren<TMP_Text>().text = condition.LocationDescription;
        }

        List<GameObject> newButtons = new List<GameObject>();
        newButtons.AddRange(collectInfo);
        newButtons.AddRange(exploreInfo);
        _conditionButtons[bounty] = newButtons.ToArray();
    }

    private void GenerateRewardsList(BountyObject bounty)
    {
        // Show the player a list of all the rewards they can win
        List<GameObject> statInfo = new List<GameObject>(bounty.BountyInfo.StatRewards.Count);
        foreach (RewardStat statReward in bounty.BountyInfo.StatRewards)
        {
            GameObject rewardText = Instantiate(RewardBoxPrefab, RewardsContainer.transform);

            statInfo.Add(rewardText);
            rewardText.GetComponentInChildren<TMP_Text>().text = "+" + statReward.Count + " " + statReward.StatName;
        }

        List<GameObject> itemInfo = new List<GameObject>(bounty.BountyInfo.ItemRewards.Count);
        foreach (RewardItem itemReward in bounty.BountyInfo.ItemRewards)
        {
            GameObject rewardText = Instantiate(RewardBoxPrefab, RewardsContainer.transform);

            itemInfo.Add(rewardText);
            rewardText.GetComponentInChildren<TMP_Text>().text = "+" + itemReward.Count + " " + itemReward.ItemName;
        }

        List<GameObject> newButtons = new List<GameObject>();
        newButtons.AddRange(statInfo);
        newButtons.AddRange(itemInfo);
        _rewardButtons[bounty] = newButtons.ToArray();
    }

    public void SelectNewBounty(BountyObject newBounty)
    {
        _selectedBounty = newBounty;

        BountyNameText.text = _selectedBounty.BountyInfo.Name;
        BountyDescriptionText.text = _selectedBounty.BountyInfo.Description;

        ToggleObject.SetActive(true);

        // Decide whether the player should be able to confirm or cancel this selected bounty
        if (!_selectedBounty.IsCompleted)
        {
            if (!BountyConfirmButton.activeSelf)
            {
                BountyConfirmButton.SetActive(true);
            }
            BountyCompleteButton.SetActive(false);
        }
        else
        {
            BountyCompleteButton.SetActive(true);
        }

        if (!_selectedBounty.IsInProgress)
        {
            _confirmUI.SwitchToConfirm();
        }
        else
        {
            _confirmUI.SwitchToCancel();
        }

        foreach (KeyValuePair<BountyObject, GameObject[]> condition in _conditionButtons)
        {
            foreach (GameObject value in condition.Value)
            {
                if (condition.Key == newBounty)
                {
                    value.SetActive(true);
                }
                else
                {
                    value.SetActive(false);
                }
            }
        }

        foreach (KeyValuePair<BountyObject, GameObject[]> condition in _rewardButtons)
        {
            foreach (GameObject value in condition.Value)
            {
                if (condition.Key == newBounty)
                {
                    value.SetActive(true);
                }
                else
                {
                    value.SetActive(false);
                }
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(ConditionsContainer.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(RewardsContainer.GetComponent<RectTransform>());
    }

    public void Exit()
    {
        _bountyBoard.PlayerLeave();
    }

    public void ConfirmNewBounty()
    {
        // The player either assigns themselves a new bounty, or cancels one they already have
        if (!_selectedBounty.IsInProgress)
        {
            _bountyBoard.AssignBounty(_selectedBounty);
        }
        else
        {
            _bountyBoard.ResetBounty(_selectedBounty);
        }

        _bountyBoard.PlayerLeave();
    }

    public void CompleteBounty()
    {
        _selectedBounty.DistributeRewards();

        Destroy(_selectedBounty._button.gameObject);
        _bountyBoard.RemoveBounty(_selectedBounty);
        _bountyBoard.PlayerLeave();
    }

    void Update()
    {
        // Updates the visuals for when the player gets close to the bounty board
        _selectionUI.transform.position = Camera.main.WorldToScreenPoint(_bountyBoard.transform.position + _bountyBoard.SelectionUIOffset);
        _selectionAnim.SetBool("FadeState", _bountyBoard._isPlayerNearby);
    }
}
