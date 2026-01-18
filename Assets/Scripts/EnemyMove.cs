using UnityEngine;
using UnityEngine.UI;

public class EnemyMove : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    [Header("Detection")]
    public Transform frontCheck;
    public float checkRadius = 0.3f;
    public LayerMask targetLayer;

    private Animator anim;
    private Rigidbody2D rb;
    private Vector3 currentTarget;
    private bool facingRight = true;
    private bool isAttacking = false;

    [Header("Combat")]
    public float damagePerSecond = 0.2f; // ennyivel csökken a Scrollbar.size másodpercenként
    private Scrollbar targetScrollbar;

    [Header("State")]
    public bool isDeath = false; // ha true, akkor minden leáll


    [Header("Animation names")]
    public string attackAnimName;
    public string walkAnimName;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentTarget = pointB.position;

        anim.Play(walkAnimName); // kezdéskor sétáljon
    }

    void Update()
    {
        if (isDeath)
        {
            rb.linearVelocity = Vector2.zero;
            return; // semmi se fusson tovább
        }
        DetectTarget();

        if (!isAttacking)
        {
            Patrol();
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // álljon meg támadás közben
            DamageTarget();

        }
    }

    void Patrol()
    {
        // mozogjon a célpont felé
        transform.position = Vector2.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        // ha elérte a célpontot, váltson irányt
        if (Vector2.Distance(transform.position, currentTarget) < 0.1f)
        {
            if (currentTarget == pointB.position)
                currentTarget = pointA.position;
            else
                currentTarget = pointB.position;

            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void DetectTarget()
    {
        Collider2D hit = Physics2D.OverlapCircle(frontCheck.position, checkRadius, targetLayer);

        if (hit != null)
        {
            //Debug.Log("isAttack " + isAttacking);
            if (!isAttacking) // csak akkor indítsa el újra az Attack animot, ha épp nem támad
            {
                isAttacking = true;
                anim.Play(attackAnimName);
                targetScrollbar = hit.GetComponentInChildren<Scrollbar>();

            }
        }
        else
        {
            if (isAttacking)
            {
                isAttacking = false;
                anim.Play(walkAnimName);
                targetScrollbar = null;

            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (frontCheck != null)
            Gizmos.DrawWireSphere(frontCheck.position, checkRadius);
    }


    void DamageTarget()
    {
        if (targetScrollbar != null)
        {
            targetScrollbar.size -= damagePerSecond * Time.deltaTime;
            targetScrollbar.size = Mathf.Clamp01(targetScrollbar.size); // ne menjen 0 alá
        }
    }
}
