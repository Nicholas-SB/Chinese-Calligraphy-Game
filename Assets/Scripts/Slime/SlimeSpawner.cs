using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] slimePrefabs;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnOffsetX = 12f;
    [SerializeField] private float spawnRangeY = 3f;
    [SerializeField] private string[] availableHanzis;
    public int maxSlimes = 20;
    public int spawnedCount = 0;

    private float timer = 0f;

    void Update()
    {
        if (spawnedCount >= maxSlimes) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnSlime();
            spawnedCount++;
        }
    }

    private void SpawnSlime()
    {
        float spawnX = Random.value > 0.5f ? spawnOffsetX : -spawnOffsetX;
        float spawnY = Random.Range(-spawnRangeY, spawnRangeY);

        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);

        // Pick random prefab from available ones
        GameObject randomPrefab = slimePrefabs[Random.Range(0, slimePrefabs.Length)];
        GameObject slime = Instantiate(randomPrefab, spawnPos, Quaternion.identity);

        SlimeBehavior behavior = slime.GetComponent<SlimeBehavior>();
        behavior.player = player;

        string randomHanzi = availableHanzis[Random.Range(0, availableHanzis.Length)];
        behavior.SetHanzis(new string[] { randomHanzi });

        Debug.Log($"Spawned slime with Hanzi: {randomHanzi}");
    }
}