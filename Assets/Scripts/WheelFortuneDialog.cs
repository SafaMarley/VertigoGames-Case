using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WheelFortuneDialog : DialogBase
{
    const float RoundAngle = 360.0f;

    [SerializeField] private Image wheelBaseImage;
    [SerializeField] private Image wheelCellIndicatorImage;

    [SerializeField] private CellView[] cellViews;

    [Header("Changeable")]
    [SerializeField] Ease rotationEase;
    [SerializeField] int minLapCount;
    [SerializeField] int maxLapCount;
    [SerializeField] float rotationDuration;

    public override void Init()
    {
        base.Init();

        int resultIndex = Random.Range(0, 8);
        wheelBaseImage.transform.DOLocalRotate(Vector3.forward * ((RoundAngle * Random.Range(minLapCount, maxLapCount)) + (resultIndex * RoundAngle / 8f)), rotationDuration, RotateMode.LocalAxisAdd).SetEase(rotationEase);
    }
}
