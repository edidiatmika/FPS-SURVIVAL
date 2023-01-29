using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealthScript : MonoBehaviour
{
    private EnemyAnimator enemyAnim;
    private NavMeshAgent navAgent;
    private EnemyController enemyController;

    public float health = 100f;

    public bool isPlayer, isBoar, isCannibal;

    private bool isDead;

    private EnemyAudio enemyAudio;

    private PlayerStats playerStats;
    

    void Awake()
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

        if (isPlayer)
        {
            playerStats.DisplayHealthStats(health);
        }

        if (isBoar || isCannibal)
        {
            if (enemyController.Enemy_State == EnemyState.PATROL)
            {
                enemyController.chaseDistance = 50f;
            }
        }

        if (health <= 0f)
        {
            PlayerDied();

            isDead = true;
        }
    }

    void PlayerDied()
    {
        if (isCannibal)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().AddTorque(-transform.forward * 2f);

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
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.enemyTag);

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }

            EnemyManager.instance.StopSpawning();

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);
        }

        if (tag == Tags.playerTag)
        {
            Invoke("RestartGame", 3f);
        }
        else
        {
            Invoke("TurnOffGameObject", 3f);
        }
    }

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("FPS01");
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
