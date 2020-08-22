using UnityEngine;

namespace RPG.Characters
{
    public class Boss : MonoBehaviour
    {
        [SerializeField] GameObject exitFence;
        [SerializeField] GameObject fog;
        [SerializeField] GameObject snowParticles;
        [SerializeField] GameObject snowman;
        [SerializeField] GameObject stoneman;
        [SerializeField] MusicPlaylist musicPlaylist;
        [SerializeField] Transform[] spawnPoints;
        [SerializeField] float firstWaveHealthPercentBorder = .7f;
        [SerializeField] float secondWaveHealthPercentBorder = .3f;

        HealthSystem healthSystem;

        bool spawnedSnowmans = false;
        bool spawnedStonemans = false;
        bool bossDefeated = false;

        // Start is called before the first frame update
        void Start()
        {
            healthSystem = GetComponent<HealthSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            bool triggerFirstWave = healthSystem.healthAsPercentage >= firstWaveHealthPercentBorder - 0.15f
                && healthSystem.healthAsPercentage <= firstWaveHealthPercentBorder;
            bool triggerSecondWave = healthSystem.healthAsPercentage >= secondWaveHealthPercentBorder - 0.15f
                && healthSystem.healthAsPercentage <= secondWaveHealthPercentBorder;
            if (triggerFirstWave && !spawnedSnowmans)
            {
                spawnedSnowmans = true;
                SpawnEnemies(snowman);
            }
            else if (triggerSecondWave && !spawnedStonemans)
            {
                spawnedStonemans = true;
                SpawnEnemies(stoneman);
            }

            if(healthSystem.healthAsPercentage <= Mathf.Epsilon && !bossDefeated)
            {
                StartCoroutine(musicPlaylist.FadeOutAfterBoss(3));
                fog.SetActive(false);
                exitFence.SetActive(false);
                Destroy(snowParticles);
                bossDefeated = true;
            }
        }

        void SpawnEnemies(GameObject enemy)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                var enemyGO = Instantiate(enemy, spawnPoints[i].position, snowman.transform.rotation);
                enemyGO.GetComponent<EnemyAI>().MultiplyChaseRadius(3);
            }
        }

        private void OnDestroy()
        {
           
        }
    }
}