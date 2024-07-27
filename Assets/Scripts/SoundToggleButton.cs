using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    private Button button;
    private Image buttonImage;

    private void Start()
    {
        button = GetComponent<Button>();
        buttonImage = button.GetComponent<Image>();
        button.onClick.AddListener(ToggleSound);
        UpdateButtonImage();
    }

    private void ToggleSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ToggleMute();
            UpdateButtonImage();
        }
    }

    private void UpdateButtonImage()
    {
        if (AudioManager.instance != null)
        {
            buttonImage.sprite = AudioManager.instance.isMuted ? soundOffSprite : soundOnSprite;
        }
    }
}