using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerNameInput : MonoBehaviour
{
    public InputField playerNameInputField;
    public Button saveButton; // Butonu tanımla
    private string nextSceneName = "Statistics"; // Bir sonraki sahnenin adını belirtin
    private bool isNameSubmitted = false; // İsim bir kez kaydedildi mi kontrolü

    void Start()
    {
        playerNameInputField.onEndEdit.AddListener(OnEndEdit);
        AddEventTrigger(playerNameInputField.gameObject, EventTriggerType.PointerClick, OnInputFieldClicked);
        saveButton.onClick.AddListener(OnSaveButtonClicked); // Buton tıklama olayını dinle

        // Debug log ile sahne adının doğru ayarlandığını kontrol edin
        Debug.Log("Next scene name: " + nextSceneName);
    }

    void AddEventTrigger(GameObject obj, EventTriggerType type, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    void OnInputFieldClicked(BaseEventData eventData)
    {
        // Mobil klavyenin açılması sağlanır (genellikle otomatik olarak açılır)
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    void OnSaveButtonClicked()
    {
        if (!isNameSubmitted)
        {
            SubmitName(playerNameInputField.text);
            isNameSubmitted = true;

            // Debug log ile sahne adının doğru ayarlandığını kontrol edin
            Debug.Log("Attempting to load next scene: " + nextSceneName);

            // Save işlemi tamamlandıktan sonra bir sonraki sahneye geçiyoruz
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                try
                {
                    SceneManager.LoadScene(nextSceneName);
                    Debug.Log("Scene loaded successfully: " + nextSceneName);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Error loading scene: " + e.Message);
                }
            }
            else
            {
                Debug.LogError("Next scene name is not set or is invalid.");
            }
        }
    }

    void OnEndEdit(string playerName)
    {
        // EndEdit olayında sadece ismin geçerliliğini kontrol ediyoruz ama kaydetmiyoruz
        if (!string.IsNullOrEmpty(playerName))
        {
            Debug.Log("EndEdit called with valid name.");
        }
    }

    void SubmitName(string playerName)
    {
        if (!string.IsNullOrEmpty(playerName) && !isNameSubmitted)
        {
            SavePlayerName(playerName);
        }
    }

    void SavePlayerName(string playerName)
    {
        Debug.Log("Player Name Saved: " + playerName);
        Player currentPlayer = FindObjectOfType<Player>();
        currentPlayer.Name = playerName;
        SaveLoadManager.SavePlayer(currentPlayer);
    }
}
