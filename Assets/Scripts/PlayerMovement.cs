using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private bool facingRight = true;

    private GameObject scrollBarObj;

    [Header("Combat")]
    public Transform frontCheck;
    public float checkRadius = 0.5f;
    public LayerMask enemyLayer;
    public LayerMask enemyDeathLayer;
    
    public float damagePerSecond = 0.3f;

    [Header("Animations")]
    public string idleAnim = "Idle";
    public string walkAnim = "Walk";
    public string attackAnim = "Attack";

    private Animator anim;
    private bool isAttacking = false;

    [Header("State")]
    public bool isDeath = false;

    // ha true, akkor minden leáll
    [Header("Bag")]
    public GameObject bag;
    public int inBagNumber;
    public float bagSpeed;
    public float normalSpeed;

    void Start()
    {
        normalSpeed = moveSpeed;

        Scrollbar scrollbar = GetComponentInChildren<Scrollbar>();
        if (scrollbar != null)
        {
            scrollBarObj = scrollbar.gameObject;
        }

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        anim.Play(idleAnim); // alapb�l Idle
    }

    void Update()
    {
        if (isDeath)
        {
            rb.linearVelocity = Vector2.zero;
            return; // semmi se fusson tovább
        }
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero; // t�mad�s k�zben ne mozogjon
            AttackEnemies();
            return;
        }

        if (moveInput != 0)
        {
            if (!isAttacking)
            {
                rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
            }
            if (moveInput > 0 && facingRight) Flip();
            if (moveInput < 0 && !facingRight) Flip();

            anim.Play(walkAnim);

            // Mozg�s k�zben n�zze, van-e ellenf�l el�tte
            DetectEnemies();
            DetectDeathEnemies();
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.Play(idleAnim);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        Vector3 scale2 = scrollBarObj.transform.localScale;
        scale2.x *= -1;
        scrollBarObj.transform.localScale = scale2;
    }

    void DetectEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(frontCheck.position, checkRadius, enemyLayer);

        if (hits.Length > 0)
        {
            isAttacking = true;
            anim.Play(attackAnim);
        }
    }

    void DetectDeathEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, 0.7f, enemyDeathLayer);
        if (hits.Length > 0)
        {
            bag.SetActive(true);
            moveSpeed = bagSpeed;
        }
        foreach (Collider2D hit in hits)
        {
            {
                Destroy(hit.transform.parent.gameObject);
                inBagNumber++;
            }
        }
    }

    void AttackEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(frontCheck.position, checkRadius, enemyLayer);

        if (hits.Length > 0)
        {
            foreach (Collider2D enemy in hits)
            {
                Scrollbar bar = enemy.GetComponentInChildren<Scrollbar>();
                if (bar != null)
                {
                    bar.size -= damagePerSecond * Time.deltaTime;
                    bar.size = Mathf.Clamp01(bar.size);
                }
            }
        }
        else
        {
            // ha nincs t�bb enemy el�tte, vissza�ll idle-be
            isAttacking = false;
            anim.Play(idleAnim);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (frontCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(frontCheck.position, checkRadius);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.layer.ToString());
       if(other.gameObject.layer.ToString() == "PigDeath")
        {
            bag.SetActive(true);
        }
    }
}