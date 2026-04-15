using System.Collections;
using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Enemy;
    [SerializeField] private float speed;
    [SerializeField] private float distanceBetween;
    [SerializeField] private float attackDistance;

    [SerializeField] private bool countForObjective = true;

    private Rigidbody2D rb;
    private Vector3 startingPosition;
    private EnemyPatrol enemyPatrol;
    private bool FacingRight = true;

    private bool isDead = false;

    private void Start()
    {
        startingPosition = transform.position;
        boxCollider = GetComponent<BoxCollider2D>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        rb = GetComponent<Rigidbody2D>();

        if (Enemy == null)
        {
            Enemy = gameObject;
        }
    }

    private void Update()
    {
        if (isDead) return;
        if (Player == null) return;

        colliderDistance = Vector2.Distance(transform.position, Player.transform.position);

        if (colliderDistance <= distanceBetween && colliderDistance > attackDistance)
        {
            if (enemyPatrol != null)
                enemyPatrol.enabled = false;

            transform.position = Vector2.MoveTowards(
                transform.position,
                Player.transform.position,
                speed * Time.deltaTime
            );
        }
        else if (colliderDistance <= attackDistance)
        {
            anim.SetBool("EnemyAttack", true);

            if (enemyPatrol != null)
                enemyPatrol.enabled = false;
        }

        if (Player.transform.position.x < transform.position.x && FacingRight)
        {
            Flip();
        }
        else if (Player.transform.position.x > transform.position.x && !FacingRight)
        {
            Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Attack"))
        {
            isDead = true;

            if (enemyPatrol != null)
            {
                enemyPatrol.enabled = false;
            }

            anim.SetTrigger("die");
            StartCoroutine(DestroyCoroutine());
        }
        else if (collision.CompareTag("Player"))
        {
            anim.SetBool("Move", false);
            anim.SetBool("EnemyAttack", true);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (isDead) return;

        if (col.CompareTag("Player"))
        {
            anim.SetBool("Move", false);
            anim.SetBool("EnemyAttack", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            anim.SetBool("EnemyAttack", false);
            anim.SetBool("Move", true);
        }
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(0.7f);

        if (countForObjective && GameManager.Instance != null)
        {
            GameManager.Instance.EnemyDefeated();
        }

        Destroy(Enemy);
    }

    private void Flip()
    {
        Vector3 tmpScale = transform.localScale;
        tmpScale.x = -tmpScale.x;
        transform.localScale = tmpScale;
        FacingRight = !FacingRight;
    }
}