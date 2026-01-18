using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HouseHealth : MonoBehaviour
{
    public Scrollbar healthBar; // ide húzd be az Enemy alatti Scrollbar-t
    public Scrollbar loading;
    private Animator anim;
    public ParticleSystem particle;
    private bool startDust = true;

    public List<GameObject> firePoints;
    public GameObject fire;
    public List<GameObject> fires;
    public TMP_Text numberText;
    private int number = 0;
    private bool isCooking = false;
    public float fillSpeed = 0.2f; // 1 egység / 5 másodperc (0.2 = 5 mp)
    public GameObject roastPig;
    public Animator endAnim;
    public bool enemyHouse = false;
    private Spawner spawner;
    public TMP_Text houseNumber;
    private bool isHouseEnd = false;
    public GameObject house;

    void Start() {
        if (enemyHouse)
        {
            spawner = this.GetComponentInParent<Spawner>();
        }
        Time.timeScale = 1f;
        anim = GetComponent<Animator>();
        fires = new List<GameObject> ();
        for (int i = 0; i < firePoints.Count; i++)
        {
            fires.Add(null);
        }
        
    }

    void Update()
    {
        if (healthBar != null )
        {
            if (healthBar.size <= 0.7f)
            {
                if (fires[0] == null)
                {
                    fires[0] = Instantiate(fire, firePoints[0].transform.position, Quaternion.identity);
                }
            }if (healthBar.size <= 0.5f)
            {
                if (fires[1] == null)
                {
                    fires[1] = Instantiate(fire, firePoints[1].transform.position, Quaternion.identity);
                }
            }
            if (healthBar.size <= 0.3f)
            {
                if (fires[2] == null)
                {
                    fires[2] = Instantiate(fire, firePoints[2].transform.position, Quaternion.identity);
                }
            }
            if (healthBar.size <= 0f && !isHouseEnd)
            {
                Debug.Log("Death");
                int LayerIgnoreRaycast = LayerMask.NameToLayer("Default");
                gameObject.layer = LayerIgnoreRaycast;
                healthBar.gameObject.SetActive(false);
                if (!enemyHouse)
                {
                    endAnim.Play("gameOverPanelUp");
                    Time.timeScale = 0f;
                }
                else
                {
                    house.layer = LayerIgnoreRaycast;
                    spawner.spawnEnd = true;
                    int n;
                    n = int.Parse(houseNumber.text);
                    n++;
                    houseNumber.text = n.ToString();
                    if(n == 2)
                    {
                        endAnim.Play("winPanelUp");
                        Time.timeScale = 0f;
                    }
                }
                isHouseEnd = true;
            }
        }
        if (isCooking)
        {
            if (startDust)
            {
                particle.Play();
                startDust = false;
            }
            if (loading.size < 1f)
            {
                loading.size += fillSpeed * Time.deltaTime;
            }
            else
            {
                number--;
                numberText.text = number.ToString();
                GameObject roast = Instantiate(roastPig, transform.position, Quaternion.identity);
                roast.GetComponent<Rigidbody2D>().AddForce(Vector2.up);
                if (number != 0)
                {
                    loading.size = 0;
                }
                else
                {
                    loading.size = 0;
                    isCooking = false;
                    particle.Stop();
                }
            }
        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (collision.transform.GetChild(8).gameObject.active != true)
            {
                return;
            }

            PlayerMovement playerMovement = collision.transform.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                int n = playerMovement.inBagNumber;
                float addNumber = 0f;
                for (int i = 0; i < n; i++)
                {
                    addNumber += 0.2f;
                    number++;
                }
                numberText.text = number.ToString();
                isCooking = true;
                startDust = true;
                playerMovement.inBagNumber = 0;
                playerMovement.moveSpeed = playerMovement.normalSpeed;
                Debug.Log("add pig" + collision.transform.childCount);
                collision.transform.GetChild(8).gameObject.SetActive(false);
                healthBar.size += addNumber;
            }
        }
    }
}

