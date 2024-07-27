using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject defaultPlatformPrefab;
    private Animator animator;
    private bool isActivated = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (isActivated) return; // Eğer animasyon tetiklendiyse tekrar çalıştırma

        Rigidbody2D platformRB2d = collider.gameObject.GetComponent<Rigidbody2D>();
        GameObject PlatformManager = GameObject.Find("PlatformManager");

        if (platformRB2d.velocity.y <= 0)
        {
            isActivated = true;
            animator.SetTrigger("PlayAnimation"); // Animasyonu tetikle
            PlatformManager.GetComponent<PlatformManager>().CreatePlatform(collider, defaultPlatformPrefab);
        }
    }

    // Bu fonksiyon animasyonun sonunda çağrılacak
    public void DestroyPlatform()
    {
        Destroy(gameObject);
    }
}
