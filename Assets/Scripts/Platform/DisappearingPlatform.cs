using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public float visibleTime = 2f; // Platformun görüneceği süre
    public float invisibleTime = 2f; // Platformun kaybolacağı süre

    private Renderer platformRenderer;
    private Collider2D platformCollider;

    void Start()
    {
        platformRenderer = GetComponent<Renderer>();
        platformCollider = GetComponent<Collider2D>();
        StartCoroutine(ToggleVisibility());
    }

    IEnumerator ToggleVisibility()
    {
        while (true)
        {
            // Platformu görünür yap
            platformRenderer.enabled = true;
            platformCollider.enabled = true;
            yield return new WaitForSeconds(visibleTime);

            // Platformu görünmez yap
            platformRenderer.enabled = false;
            platformCollider.enabled = false;
            yield return new WaitForSeconds(invisibleTime);
        }
    }
}
