using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public Scrollbar healthBar; // ide húzd be az Enemy alatti Scrollbar-t
    private EnemyMove patrol;
    private PlayerMovement playerMovement;
    private Animator anim;
    public string deathAnimName;
    public GameObject particle;
    private GameObject bag;
    private WolfDeath dead;
    void Start()
    {
        patrol = GetComponent<EnemyMove>();
        playerMovement = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
        dead = GetComponent<WolfDeath>();
    }

    void Update()
    {
        if (patrol !=null && patrol.isDeath) return; // ha már halott, akkor ne fusson újra
        if (playerMovement != null && playerMovement.isDeath) return;


        if (healthBar != null && healthBar.size <= 0f)
        {
            Debug.Log("Death");
            if (patrol != null)
            {
                patrol.isDeath = true; // megállít mindent
                int LayerIgnoreRaycast = LayerMask.NameToLayer("PigDeath");
                gameObject.layer = LayerIgnoreRaycast;
            }
            else if(playerMovement != null)
            {
                playerMovement.isDeath = true;
                int LayerIgnoreRaycast = LayerMask.NameToLayer("Default");
                gameObject.layer = LayerIgnoreRaycast;
            }

            anim.Play(deathAnimName);
            if (particle != null)
            {
                Instantiate(particle, this.transform.position, Quaternion.identity);

            }
           
            
            healthBar.gameObject.SetActive(false);
            //GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Dead", 1.0f);
        }
    }

    public void Dead()
    {
        if (dead != null)
        {
            dead.Dead();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.tag == "Roasted" && this.tag == "Player")
        {
            healthBar.size += 0.2f;
            Destroy(collision.gameObject);
        }
    }
}
