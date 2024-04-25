using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class WheelFortuneDialog : DialogBase
{
    const float RoundAngle = 360.0f;
    const int SlotCount = 8;
    const int ReviveCost = 25;

    [SerializeField] private Image wheelBaseImage;
    [SerializeField] private Image wheelCellIndicatorImage;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private CellView[] cellViews;

    [SerializeField] private Button spinButton;
    [SerializeField] private Button withdrawButton;
    [SerializeField] private Button giveUpButton;
    [SerializeField] private Button reviveButton;

    [SerializeField] private Transform inventoryHolderTransform;
    [SerializeField] private Transform earningsHolderTransform;
    [SerializeField] private Transform revivePopup;

    [SerializeField] private ItemView itemViewPrefab;

    [Header("Changeable")]
    [SerializeField] Ease rotationEase;
    [SerializeField] int minLapCount;
    [SerializeField] int maxLapCount;
    [SerializeField] float rotationDuration;

    private WheelContentStruct[] _wheelContents;

    private int _comboCount;
    private int _currentIndex;

    private int SilverWheelFrequency = 5;
    private int GoldWheelFrequency = 30;

    private WheelEnum _currentWheelType = WheelEnum.None;

    private static Dictionary<int, int> _currentPotDictionary = new();

    public override void Init()
    {
        base.Init();

        _comboCount = 1;

        InitializeWheel(WheelEnum.Bronze);

        spinButton.onClick.AddListener(OnSpinButtonClicked);
        withdrawButton.onClick.AddListener(OnPlayerWithdraw);
        giveUpButton.onClick.AddListener(OnPlayerGiveUp);
        reviveButton.onClick.AddListener(OnPlayerRevived);
    }

    private void OnDestroy()
    {
        spinButton.onClick.RemoveAllListeners();
        withdrawButton.onClick.RemoveAllListeners();
        giveUpButton.onClick.RemoveAllListeners();
        reviveButton.onClick.RemoveAllListeners();
    }

    private void InitializeWheel(WheelEnum wheelType)
    {
        if (_currentWheelType == wheelType) return;

        switch (wheelType)
        {
            case WheelEnum.Silver:
            case WheelEnum.Gold:
                withdrawButton.interactable = true;
                break;

            case WheelEnum.Bronze:
                withdrawButton.interactable = false;
                break;
        }

        _currentWheelType = wheelType;
        WheelResources wheelResource = WheelResourcesHelper.Instance.GetItemResource(_currentWheelType);

        wheelBaseImage.sprite = wheelResource.wheelSprite;
        wheelCellIndicatorImage.sprite = wheelResource.wheelCellIndicatorSprite;

        titleText.text = wheelResource.wheelTitle;
        descriptionText.text = wheelResource.wheelDescription;

        _wheelContents = wheelResource.wheelContents;

        for (int i = 0; i < _wheelContents.Length; i++)
        {
            var wheelContent = _wheelContents[i];

            cellViews[i].Init(wheelContent.itemId, wheelContent.itemAmount);
        }
    }

    private void OnSpinButtonClicked()
    {
        spinButton.interactable = false;
        int indexToGo = Random.Range(0, SlotCount);
        _currentIndex = (_currentIndex + indexToGo) % SlotCount;

        wheelBaseImage.transform.DOLocalRotate(Vector3.forward * ((RoundAngle * Random.Range(minLapCount, maxLapCount)) + (indexToGo * RoundAngle / SlotCount)), rotationDuration, RotateMode.LocalAxisAdd)
            .SetEase(rotationEase)
            .OnComplete(OnSpinEnd);
    }

    private void OnSpinEnd()
    {
        spinButton.interactable = true;

        var content = _wheelContents[_currentIndex];

        if ((ItemEnum) content.itemId == ItemEnum.Grenade)
        {
            revivePopup.gameObject.SetActive(true);
            reviveButton.interactable = Inventory.GetItemAmount((int) ItemEnum.Cash) > ReviveCost;
        }
        else
        {
            AddItemToPot(content.itemId, content.itemAmount);

            _comboCount++;

            if (_comboCount % GoldWheelFrequency == 0) InitializeWheel(WheelEnum.Gold);
            else if (_comboCount % SilverWheelFrequency == 0) InitializeWheel(WheelEnum.Silver);
            else InitializeWheel(WheelEnum.Bronze);
        }

        //LogInventory();
    }

    private void OnPlayerWithdraw()
    {
        ResetPot(true);
        DisplayInventory();

        _comboCount = 1;
        InitializeWheel(WheelEnum.Bronze);
    }

    private void OnPlayerGiveUp()
    {
        revivePopup.gameObject.SetActive(false);
        ResetPot(false);

        _comboCount = 1;
        InitializeWheel(WheelEnum.Bronze);
    }

    private void OnPlayerRevived()
    {
        revivePopup.gameObject.SetActive(false);
        Inventory.UpdateInventory((int)ItemEnum.Cash, -25);
        DisplayInventory();
    }

    private void LogInventory()
    {
        foreach (var itemKeyValuePair in _currentPotDictionary)
            Debug.LogError($"Item : {(ItemEnum)itemKeyValuePair.Key} | Amount : {itemKeyValuePair.Value}");
    }

    private void DisplayInventory()
    {
        foreach (Transform child in inventoryHolderTransform.transform)
            Destroy(child.gameObject);

        foreach (var itemKeyValuePair in Inventory.GetCurrentInventory())
        {
            var itemView = Instantiate(itemViewPrefab, inventoryHolderTransform);
            itemView.Init(itemKeyValuePair.Key, itemKeyValuePair.Value);
        }
    }

    private void AddItemToPot(int itemId, int itemAmount)
    {
        if (_currentPotDictionary.ContainsKey(itemId))
            _currentPotDictionary[itemId] += itemAmount;
        else
            _currentPotDictionary.Add(itemId, itemAmount);

        var itemView = Instantiate(itemViewPrefab, earningsHolderTransform);
        itemView.Init(itemId, itemAmount);
    }

    private void ResetPot(bool transferPotToInventory)
    {
        foreach (Transform child in earningsHolderTransform.transform)
            Destroy(child.gameObject);

        if (transferPotToInventory)
            foreach (var itemKeyValuePair in _currentPotDictionary.Where(x => x.Value > 0))
                Inventory.AddItemToInventory(itemKeyValuePair.Key, itemKeyValuePair.Value);

        _currentPotDictionary.Clear();
    }
}
