using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct DialogStruct
{
    public DialogEnum dialogEnum;
    public DialogBase dialogPrefab;
}

public enum DialogEnum
{
    WheelOfFortuneDialog
}

public class DialogHelper : HelperBase<DialogHelper>
{
    [SerializeField] List<DialogStruct> dialogs;
    private Dictionary<DialogEnum, DialogBase> dialogDictionary = new();

    public void Init()
    {
        foreach (var dialog in dialogs)
        {
            dialogDictionary.Add(dialog.dialogEnum, dialog.dialogPrefab);
        }
    }

    public void DisplayDialog(DialogEnum dialog)
    {
        Instantiate(dialogDictionary[dialog], transform).Init();
    }
}
