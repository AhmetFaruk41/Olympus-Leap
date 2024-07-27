using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f; // Platformun hareket hızı
    public float distance = 3.75f; // Platformun hareket edeceği mesafe

    private float startX;
    private Rigidbody2D rb;

    void Start()
    {
        startX = transform.position.x; // Başlangıç konumunu kaydet

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = true; // Rigidbody2D'yi kinematik olarak ayarla
        }
    }

    void FixedUpdate()
    {
        // Platformu x ekseninde ileri geri hareket ettir
        float newX = startX + Mathf.PingPong(Time.time * speed, distance) * 2 - distance;

        rb.MovePosition(new Vector2(newX, transform.position.y));
    }
}
