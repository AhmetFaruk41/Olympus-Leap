using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingPlatform : MonoBehaviour
{
    [SerializeField]
    private float scaleSpeed = 1f; // Platformun ölçeklenme hızı
    [SerializeField]
    private float maxScale = 1.5f; // Maksimum ölçek
    [SerializeField]
    private float minScale = 0.5f; // Minimum ölçek

    private bool scalingUp = true;

    private void Update()
    {
        if (scalingUp)
        {
            transform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;
            if (transform.localScale.x >= maxScale)
            {
                scalingUp = false;
            }
        }
        else
        {
            transform.localScale -= Vector3.one * scaleSpeed * Time.deltaTime;
            if (transform.localScale.x <= minScale)
            {
                scalingUp = true;
            }
        }
    }
}
