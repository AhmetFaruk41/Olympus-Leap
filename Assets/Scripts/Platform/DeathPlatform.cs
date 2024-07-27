using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlatform : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public BoxCollider2D topCollider;  // Üst kısım collider'ı
    public BoxCollider2D bottomCollider; // Alt kısım collider'ı

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red; // Death platformunu kırmızı renkte göster
        }

        // Collider referanslarının atanıp atanmadığını kontrol edin
        if (topCollider == null)
        {
            Debug.LogError("Top Collider atanmadı!");
        }
        if (bottomCollider == null)
        {
            Debug.LogError("Bottom Collider atanmadı!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D tetiklendi: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Character"))
        {
            if (collision.otherCollider == topCollider)
            {
                Debug.Log("Karakter platformun üstüne dokundu!");
                // Üst kısmına dokunduğunda bir işlem yapma
            }
            else if (collision.otherCollider == bottomCollider)
            {
                Debug.Log("Karakter platformun altına dokundu!");
                // Karakteri yok et
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D tetiklendi: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Character"))
        {
            Debug.Log("Karakterin alt collider ile çarpışması kontrol ediliyor.");
            Collider2D playerCollider = collision.GetComponent<Collider2D>();

            if (playerCollider != null && bottomCollider.IsTouching(playerCollider))
            {
                Debug.Log("Karakter platformun altına dokundu (trigger)!");
                // Karakteri yok et
                Destroy(collision.gameObject);
            }
        }
    }
}
