using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [SerializeField] private Transform enemy;
    [SerializeField] private float speed;

    private Vector3 initScale;
    private bool movingLeft;

    [SerializeField] private Animator anim;
    [SerializeField] private float idleDuration;
    private float idleTimer;

    private void Start()
    {
        if (enemy != null)
        {
            initScale = enemy.localScale;
        }
    }

    private void OnDisable()
    {
        if (anim != null)
        {
            anim.SetBool("Move", false);
        }
    }

    private void Update()
    {
        if (enemy == null || leftEdge == null || rightEdge == null || anim == null)
            return;

        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
            {
                MoveInDirection(-1);
            }
            else
            {
                DirectionChange();
            }
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                DirectionChange();
            }
        }
    }

    private void MoveInDirection(int direction)
    {
        if (enemy == null || anim == null)
            return;

        idleTimer = 0;
        anim.SetBool("Move", true);

        enemy.localScale = new Vector3(
            Mathf.Abs(initScale.x) * direction,
            initScale.y,
            initScale.z
        );

        enemy.position = new Vector3(
            enemy.position.x + Time.deltaTime * direction * speed,
            enemy.position.y,
            enemy.position.z
        );
    }

    private void DirectionChange()
    {
        if (anim == null)
            return;

        anim.SetBool("Move", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
        }
    }
}
