using UnityEngine;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    public void KillPlayer(Player player)
    {
        // Karakterin ölmesini sağla
        player.Die();
        // Sonraki sahneye geçiş yap
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            // Karakter yılanla temas ederse KillPlayer metodunu çağır
            KillPlayer(other.GetComponent<Player>());
        }
    }
}
