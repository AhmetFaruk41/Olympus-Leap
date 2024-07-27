using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject defaultPlatformPrefab;

    [SerializeField]
    private GameObject boostPlatformPrefab;

    [SerializeField]
    private GameObject brokenPlatformPrefab;

    [SerializeField]
    private GameObject movingPlatformPrefab;

    [SerializeField]
    private GameObject disappearingPlatformPrefab;

    [SerializeField]
    private GameObject snakePlatformPrefab; // Yeni platform prefabı

    [SerializeField]
    private float movingPlatformStartHeight = 50f;

    [SerializeField]
    private float minDistanceBetweenPlatforms = 1.5f;

    [SerializeField]
    private ProbabilitySettings lowHeightProbabilities;

    [SerializeField]
    private ProbabilitySettings midHeightProbabilities;

    [SerializeField]
    private ProbabilitySettings highHeightProbabilities;

    private List<Vector2> recentPlatformPositions = new List<Vector2>();
    private float gameStartTime;

    private void Start()
    {
        gameStartTime = Time.time;
    }

    private void Update()
    {
        UpdateDifficulty();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CreatePlatforms(collision);
    }

    private void CreatePlatforms(Collision2D collision)
    {
        GameObject prefabToUse = GetPlatformPrefabByPlayerHeight();

        if (collision.gameObject.CompareTag("DefaultPlatform") ||
            collision.gameObject.CompareTag("MovingPlatform"))
        {
            if (Random.Range(0, 100) <= 5)
            {
                ReplacePlatform(collision, prefabToUse);
            }
            else
            {
                TransformPlatform(collision);
            }
        }
        else
        {
            ReplacePlatform(collision, prefabToUse);
        }
    }

    private GameObject GetPlatformPrefabByPlayerHeight()
    {
        float playerHeight = player.transform.position.y;
        int randomValue = Random.Range(0, 100);
        ProbabilitySettings probabilities = lowHeightProbabilities;

        if (playerHeight > movingPlatformStartHeight)
        {
            probabilities = midHeightProbabilities;
        }
        else
        {
            probabilities = highHeightProbabilities;
        }

        int cumulativeProbability = 0;

        if ((cumulativeProbability += probabilities.disappearingPlatformProbability) > randomValue)
            return disappearingPlatformPrefab;
        if ((cumulativeProbability += probabilities.movingPlatformProbability) > randomValue)
            return movingPlatformPrefab;
        if ((cumulativeProbability += probabilities.brokenPlatformProbability) > randomValue)
            return brokenPlatformPrefab;
        if ((cumulativeProbability += probabilities.boostPlatformProbability) > randomValue)
            return boostPlatformPrefab;
        if ((cumulativeProbability += probabilities.snakePlatformProbability) > randomValue) // Yeni platform olasılığı
            return snakePlatformPrefab;

        return defaultPlatformPrefab;
    }

    public void CreatePlatform(Collision2D collision, GameObject kindOfPlatform)
    {
        Vector2 newPosition;
        do
        {
            newPosition = new Vector2(
                Random.Range(-3.75f, 3.75f),
                player.transform.position.y + 7f + Random.Range(-0.50f, 0.50f));
        } while (IsPositionTooCloseToOthers(newPosition));

        recentPlatformPositions.Add(newPosition);
        if (recentPlatformPositions.Count > 10)
        {
            recentPlatformPositions.RemoveAt(0);
        }

        Instantiate(kindOfPlatform, newPosition, Quaternion.identity);
    }

    private void TransformPlatform(Collision2D collision)
    {
        Vector2 newPosition;
        do
        {
            newPosition = new Vector2(
                Random.Range(-3.75f, 3.75f),
                player.transform.position.y + 7f + Random.Range(-0.50f, 0.50f));
        } while (IsPositionTooCloseToOthers(newPosition));

        recentPlatformPositions.Add(newPosition);
        if (recentPlatformPositions.Count > 10)
        {
            recentPlatformPositions.RemoveAt(0);
        }

        collision.gameObject.transform.position = newPosition;
    }

    private bool IsPositionTooCloseToOthers(Vector2 position)
    {
        foreach (Vector2 recentPosition in recentPlatformPositions)
        {
            if (Vector2.Distance(position, recentPosition) < minDistanceBetweenPlatforms)
            {
                return true;
            }
        }
        return false;
    }

    private void ReplacePlatform(Collision2D collision, GameObject prefabToUse)
    {
        Destroy(collision.gameObject);
        CreatePlatform(collision, prefabToUse);
    }

    private void UpdateDifficulty()
    {
        float elapsedTime = Time.time - gameStartTime;
        float playerHeight = player.transform.position.y;

        if (playerHeight > movingPlatformStartHeight)
        {
            midHeightProbabilities.boostPlatformProbability += (int)(elapsedTime / 60);
            midHeightProbabilities.brokenPlatformProbability += (int)(elapsedTime / 60);
        }
    }
}

[System.Serializable]
public class ProbabilitySettings
{
    public int disappearingPlatformProbability;
    public int movingPlatformProbability;
    public int brokenPlatformProbability;
    public int boostPlatformProbability;
    public int snakePlatformProbability;
    public int defaultPlatformProbability; // Yeni default platform olasılığı
}
