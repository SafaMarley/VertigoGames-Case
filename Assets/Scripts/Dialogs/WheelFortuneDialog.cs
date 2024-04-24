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

    [SerializeField] private Image wheelBaseImage;
    [SerializeField] private Image wheelCellIndicatorImage;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private CellView[] cellViews;

    [SerializeField] private Button spinButton;

    [SerializeField] private Transform earningsHolderTransform;

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
    }

    private void OnDestroy()
    {
        spinButton.onClick.RemoveAllListeners();
    }

    private void InitializeWheel(WheelEnum wheelType)
    {
        if (_currentWheelType == wheelType) return;

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
            _comboCount = 1;
            InitializeWheel(WheelEnum.Bronze);

            OnPlayerLeave();
        }
        else
        {
            AddItemToPot(content.itemId, content.itemAmount);

            _comboCount++;

            if (_comboCount % GoldWheelFrequency == 0) InitializeWheel(WheelEnum.Gold);
            else if (_comboCount % SilverWheelFrequency == 0) InitializeWheel(WheelEnum.Silver);
            else InitializeWheel(WheelEnum.Bronze);
        }

        LogInventory();
    }

    private void LogInventory()
    {
        foreach (var itemKeyValuePair in _currentPotDictionary)
        {
            Debug.LogError($"Item : {(ItemEnum)itemKeyValuePair.Key} | Amount : {itemKeyValuePair.Value}");
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

    private void OnPlayerLeave()
    {
        _currentPotDictionary.Clear();

        foreach (var itemKeyValuePair in _currentPotDictionary.Where(x => x.Value > 0))
        {
            Inventory.AddItemToInventory(itemKeyValuePair.Key, itemKeyValuePair.Value);
        }

        foreach (Transform child in earningsHolderTransform.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
