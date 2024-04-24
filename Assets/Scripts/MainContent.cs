using UnityEngine;

public class MainContent : MonoBehaviour
{
    [SerializeField] private DialogHelper dialogHelper;
    [SerializeField] private ItemResourcesHelper itemResourcesHelper;
    [SerializeField] private WheelResourcesHelper wheelResourcesHelper;

    void Start()
    {
        InitializeHelpers();

        DialogHelper.Instance.DisplayDialog(DialogEnum.WheelOfFortuneDialog);
    }

    private void InitializeHelpers()
    {
        Instantiate(dialogHelper, transform).Init();
        Instantiate(itemResourcesHelper, transform).Init();
        Instantiate(wheelResourcesHelper, transform).Init();
    }
}
