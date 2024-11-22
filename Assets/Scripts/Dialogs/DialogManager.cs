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
    public GameObject dialogPanel; // Панель с диалогом
    public TextMeshProUGUI nameText; // TextMeshPro для имени
    public TextMeshProUGUI dialogText; // TextMeshPro для текста диалога
    public Image characterImage; // UI-элемент для изображения персонажа
    public Button nextButton; // Кнопка для переключения

    private DialogData dialogData;
    private Queue<DialogLine> currentLines;
    private System.Action onDialogComplete;
    private bool isDialogActive = false; // Флаг активности диалога

    void Start()
    {
        dialogPanel.SetActive(false);
        currentLines = new Queue<DialogLine>();

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
        
        // Устанавливаем имя и текст
        nameText.text = line.character; // Устанавливаем имя персонажа
        dialogText.text = line.text;    // Устанавливаем текст диалога

        // Загружаем и устанавливаем изображение персонажа
        Sprite sprite = Resources.Load<Sprite>("Images/" + line.image);
        if (sprite != null)
        {
            characterImage.sprite = sprite;          // Устанавливаем изображение
            characterImage.gameObject.SetActive(true); // Активируем изображение
        }
        else
        {
            characterImage.sprite = Resources.Load<Sprite>("Images/default"); // Заглушка
            characterImage.gameObject.SetActive(true); // Активируем изображение
            Debug.LogWarning("Изображение " + line.image + " не найдено. Используется изображение по умолчанию.");
        }
    }

    void EndDialog()
    {
        // Скрываем панель диалога
        dialogPanel.SetActive(false);

        // Очищаем текстовые элементы
        nameText.text = string.Empty;
        dialogText.text = string.Empty;

        // Отключаем изображение персонажа
        if (characterImage != null)
        {
            characterImage.sprite = null; // Убираем спрайт
            characterImage.gameObject.SetActive(false); // Отключаем объект
        }

        // Сбрасываем флаг активности диалога
        isDialogActive = false;

        // Вызываем callback, если он есть
        onDialogComplete?.Invoke();

        Debug.Log("Диалог завершен.");
    }
}