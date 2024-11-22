using UnityEngine;

public class Test : MonoBehaviour
{
    public DialogManager dialogManager;

    public void StartIntroDialog()
    {
        dialogManager.StartDialog("intro", OnDialogComplete);
    }

    public void OnDialogComplete()
    {
        Debug.Log("Диалог завершен.");
    }
}