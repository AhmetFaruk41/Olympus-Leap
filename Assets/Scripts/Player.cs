using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D playerRB2d;
    private float playerSpeed = 15f;
    private float horizontalAxes;
    private float maxJumpScore = 0f;
    private string name = "Jumper";
    private bool isPaused = false;
    private bool isLive = true;

    public float MaxJumpScore
    {
        get { return maxJumpScore; }
        set { maxJumpScore = value; } // Private yerine Public setter
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public bool IsLive
    {
        get { return isLive; }
        set { isLive = value; }
    }

    [SerializeField]
    private Sprite playerUpMovement;
    [SerializeField]
    private Sprite playerLeftMovement;
    [SerializeField]
    private Text playerScore;
    [SerializeField]
    private Text playerHighScore;
    [SerializeField]
    private Button moveRightButton; // Sağ hareket butonu
    [SerializeField]
    private Button moveLeftButton; // Sol hareket butonu
    [SerializeField]
    private Button pauseButton; // Pause butonu
    [SerializeField]
    private Sprite playIcon; // Üçgen simge
    [SerializeField]
    private Sprite pauseIcon; // Duraklatma simge

    private bool isMovingRight = false;
    private bool isMovingLeft = false;

    private void Start()
    {
        LoadHighScore();

        playerRB2d = GetComponent<Rigidbody2D>();

        // Sağ ve sol hareket butonlarına dokunmatik olaylar ekleme
        AddEventTriggerListener(moveRightButton.gameObject, EventTriggerType.PointerDown, (e) => { isMovingRight = true; });
        AddEventTriggerListener(moveRightButton.gameObject, EventTriggerType.PointerUp, (e) => { isMovingRight = false; });

        AddEventTriggerListener(moveLeftButton.gameObject, EventTriggerType.PointerDown, (e) => { isMovingLeft = true; });
        AddEventTriggerListener(moveLeftButton.gameObject, EventTriggerType.PointerUp, (e) => { isMovingLeft = false; });

        // Pause butonu tıklama olayını ekleme
        pauseButton.onClick.AddListener(TogglePause);

        pauseButton.gameObject.SetActive(true);

        // Başlangıçta pause butonunun simgesini "||" olarak ayarlayın
        pauseButton.GetComponent<Image>().sprite = pauseIcon;
    }

    private void Update()
    {
        CalculateScore(playerRB2d);
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        if (isMovingRight)
        {
            GetComponent<SpriteRenderer>().sprite = playerLeftMovement;
            GetComponent<SpriteRenderer>().flipX = true;
            horizontalAxes = 1;
        }
        else if (isMovingLeft)
        {
            GetComponent<SpriteRenderer>().sprite = playerLeftMovement;
            GetComponent<SpriteRenderer>().flipX = false;
            horizontalAxes = -1;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = playerUpMovement;
            horizontalAxes = 0;
        }

        // Hareketi yumuşatmak için Lerp kullan
        float smoothHorizontal = Mathf.Lerp(playerRB2d.velocity.x, horizontalAxes * playerSpeed, 0.1f);
        playerRB2d.velocity = new Vector2(smoothHorizontal, playerRB2d.velocity.y);
    }

    private void CalculateScore(Rigidbody2D playerRB2d)
    {
        if (playerRB2d.velocity.y > 0 && transform.position.y > maxJumpScore)
        {
            maxJumpScore = transform.position.y;
        }

        if (playerScore != null)
        {
            playerScore.text = "Score: " + Math.Round(maxJumpScore);
        }
        else
        {
            Debug.LogError("playerScore Text component is not assigned in the Inspector.");
        }
    }

    private void LoadHighScore()
    {
        if (SaveLoadManager.isDataFileExists())
        {
            PlayerData bestPlayer = (PlayerData)SaveLoadManager.LoadBestPlayer();
            if (playerHighScore != null)
            {
                playerHighScore.text = "High Score: \n" + "<color=green>" + bestPlayer.playerName + " | " + bestPlayer.playerScore + "</color>";
            }
            else
            {
                Debug.LogError("playerHighScore Text component is not assigned in the Inspector.");
            }
        }
        else
        {
            SaveLoadManager.CreateEmptyPlayers();
        }
    }

    // Pause butonu için tıklama olayını yönetme
    private void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            pauseButton.GetComponent<Image>().sprite = playIcon; // Buton simgesini üçgen olarak değiştir
        }
        else
        {
            Time.timeScale = 1;
            pauseButton.GetComponent<Image>().sprite = pauseIcon; // Buton simgesini duraklatma olarak değiştir
        }
    }

    // Event Trigger eklemek için yardımcı metod
    private void AddEventTriggerListener(GameObject obj, EventTriggerType eventType, Action<BaseEventData> callback)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>() ?? obj.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(callback));
        trigger.triggers.Add(entry);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Snake"))
        {
            Die();
        }
    }

    public void Die()
    {
        // Ölüm davranışını burada tanımlayın
        // Örneğin, oyunu yeniden başlatma, bir ölüm ekranı gösterme vb.
        Debug.Log("Player Died!");
        isLive = false;
        // Örneğin, sahneyi yeniden başlatmak:
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
