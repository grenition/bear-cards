using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public DialogManager dialogManager;

    public void StartIntroDialog()
    {
        List<string> dialogsToRun = new List<string> { "intro", "quest_start", "village_intro" };
        dialogManager.EnqueueDialogs(dialogsToRun);
    }

    public void OnDialogComplete()
    {
        Debug.Log("Диалог завершен.");
    }
}