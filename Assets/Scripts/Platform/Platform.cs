using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float jumpForce = 300f; // Zıplama gücünü değişken olarak tanımladık

    private void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D platformRB2d = collision.gameObject.GetComponent<Rigidbody2D>();

        if (platformRB2d != null && platformRB2d.velocity.y <= 0)
        {
            platformRB2d.AddForce(Vector2.up * jumpForce);
        }
    }
}
