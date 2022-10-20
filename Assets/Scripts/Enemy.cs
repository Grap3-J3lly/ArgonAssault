using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject enemyExplosionVFX;
    [SerializeField] GameObject enemyHitVFX;
    [SerializeField] Transform parent;
    [SerializeField] int pointAmount = 10;
    [SerializeField] int enemyHealth = 100;
    [SerializeField] int shotDamage = 50;

    ScoreBoard scoreBoard;

    void Start() {
        scoreBoard = FindObjectOfType<ScoreBoard>();
    }

    private void OnParticleCollision(GameObject other) {
        
        ProcessHit();
        KillEnemy();
    }

    void ProcessHit()
    {
        scoreBoard.IncreaseScore(pointAmount);
        scoreBoard.DisplayScore();
    }

    void KillEnemy()
    {
        enemyHealth -= shotDamage;
        SpawnExplosion(enemyHitVFX);

        if(enemyHealth != 0) {return;}
        SpawnExplosion(enemyExplosionVFX);
        Destroy(gameObject);
    }

    void SpawnExplosion(GameObject explosionType)
    {
        GameObject vfx = Instantiate(explosionType, transform.position, Quaternion.identity);
        vfx.transform.parent = parent;
    }

}
