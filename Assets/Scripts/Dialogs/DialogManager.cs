using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

[System.Serializable]
public class DialogLine
{
    public string character;
    public string text;
    public string image;
}

[System.Serializable]
public class Dialog
{
    public string id;
    public List<DialogLine> lines;
}

[System.Serializable]
public class DialogData
{
    public List<Dialog> dialogs;
}

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel; 
    public TextMeshProUGUI nameText; 
    public TextMeshProUGUI dialogText;
    public Image characterImage; 
    public Button nextButton; 
    private DialogData dialogData;
    private Queue<DialogLine> currentLines;
    private System.Action onDialogComplete;
    private bool isDialogActive = false; // Флаг активности диалога
    private Queue<string> dialogQueue; // Очередь ID диалогов

    void Start()
    {
        dialogPanel.SetActive(false);
        currentLines = new Queue<DialogLine>();
        dialogQueue = new Queue<string>(); 

        // Загрузка JSON-файла
        string path = Path.Combine(Application.streamingAssetsPath, "dialog.json");
        string json = File.ReadAllText(path);
        dialogData = JsonUtility.FromJson<DialogData>(json);

        // Проверка наличия всех изображений
        foreach (var dialog in dialogData.dialogs)
        {
            foreach (var line in dialog.lines)
            {
                Sprite sprite = Resources.Load<Sprite>("Images/" + line.image);
                if (sprite == null)
                {
                    Debug.LogError("Изображение " + line.image + " не найдено!");
                }
            }
        }
    }

    public void EnqueueDialogs(List<string> dialogIds)
    {
        foreach (var dialogId in dialogIds)
        {
            dialogQueue.Enqueue(dialogId);
        }

        if (!isDialogActive && dialogQueue.Count > 0)
        {
            StartNextDialog();
        }
    }

    private void StartNextDialog()
    {
        if (dialogQueue.Count == 0)
        {
            Debug.Log("Все диалоги завершены.");
            return;
        }

        string nextDialogId = dialogQueue.Dequeue();
        StartDialog(nextDialogId, StartNextDialog); 
    }

    public void StartDialog(string dialogId, System.Action onComplete = null)
    {
        if (isDialogActive)
        {
            Debug.LogWarning("Диалог уже активен!");
            return;
        }

        Dialog dialog = dialogData.dialogs.Find(d => d.id == dialogId);
        if (dialog == null)
        {
            Debug.LogError("Диалог с ID " + dialogId + " не найден.");
            return;
        }

        // Сбрасываем интерфейс
        nameText.text = string.Empty;
        dialogText.text = string.Empty;
        if (characterImage != null)
        {
            characterImage.sprite = null;
            characterImage.gameObject.SetActive(false);
        }

        // Показываем панель диалога
        dialogPanel.SetActive(true);
        onDialogComplete = onComplete;
        isDialogActive = true; // Устанавливаем флаг активности

        // Очищаем очередь строк и заполняем новыми строками диалога
        currentLines.Clear();
        foreach (var line in dialog.lines)
        {
            currentLines.Enqueue(line);
        }

        // Переходим к первой строке
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (currentLines.Count == 0)
        {
            EndDialog(); // Завершаем диалог, если строки закончились
            return;
        }

        DialogLine line = currentLines.Dequeue();
        
        nameText.text = line.character; 
        dialogText.text = line.text; 

        Sprite sprite = Resources.Load<Sprite>("Images/" + line.image);
        if (sprite != null)
        {
            characterImage.sprite = sprite;         
            characterImage.gameObject.SetActive(true); 
        }
        else
        {
            characterImage.sprite = Resources.Load<Sprite>("Images/default"); 
            characterImage.gameObject.SetActive(true); 
            Debug.LogWarning("Изображение " + line.image + " не найдено. Используется изображение по умолчанию.");
        }
    }

    void EndDialog()
    {
        dialogPanel.SetActive(false);

        nameText.text = string.Empty;
        dialogText.text = string.Empty;

        if (characterImage != null)
        {
            characterImage.sprite = null;
            characterImage.gameObject.SetActive(false); 
        }

        // Сбрасываем флаг активности диалога
        isDialogActive = false;

        onDialogComplete?.Invoke();

        Debug.Log("Диалог завершен.");
    }
}