using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class HealthScript : MonoBehaviour
{
    private EnemyAnimator enemyAnim;
    private NavMeshAgent navAgent;
    private EnemyController enemyController;

    public float health = 100f;

    public bool isPlayer, isBoar, isCannibal;

    private bool isDead;

    public const string Enemy = "Enemy";
    public const string Player = "Player";

    private EnemyAudio enemyAudio;
    private PlayerStats playerStats;

    private void Awake()
    {
        if (isBoar || isCannibal)
        {
            enemyAnim = GetComponent<EnemyAnimator>();
            enemyController = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();
            enemyAudio = GetComponentInChildren<EnemyAudio>();
        }

        if (isPlayer) 
        { 
            playerStats = GetComponent<PlayerStats>();
        }
    }

    public void ApplyDamage(float damage)
    {
        if (isDead)
            return;

        health -= damage;

        if (isBoar || isCannibal)
        {
            if (enemyController.enemyState == EnemyState.PATROL)
            {
                enemyController.chaseDistance = 50f;
            }
        }

        if (health <= 0)
        {
            isDead = true;
            Died();
        }

        if (isPlayer)
        {
            playerStats.DisplayHealthStats(health);
        }
    }

    private void Died()
    {
        if (isCannibal)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            //GetComponent<Rigidbody>().AddTorque(-transform.forward * 5f);

            enemyController.enabled = false;
            navAgent.enabled = false;
            enemyAnim.enabled = false;
            StartCoroutine(DeadSound());

            EnemyManager.instance.EnemyDied(true);
        }

        if (isBoar)
        {
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemyController.enabled = false;
            enemyAnim.Dead();
            StartCoroutine(DeadSound());
            EnemyManager.instance.EnemyDied(false);
        }

        if (isPlayer)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Enemy);

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>()
                .GetCurrentSelectedWeapon()
                .gameObject
                .SetActive(false);

            EnemyManager.instance.StopSpawning();
        }

        if (tag == Player)
        {
            Invoke(nameof(RestartGame), 3f);
        }
        else
        {
            Invoke(nameof(TurnOffGameObject), 3f);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }

    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f);
        enemyAudio.PlayDeadSound();
    }
}
