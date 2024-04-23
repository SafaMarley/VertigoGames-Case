using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] private DialogHelper dialogHelper;

    void Start()
    {
        InitializeHelpers();

        DialogHelper.Instance.DisplayDialog(DialogEnum.WheelOfFortuneDialog);
    }

    private void InitializeHelpers()
    {
        Instantiate(dialogHelper, transform).Init();
    }
}
