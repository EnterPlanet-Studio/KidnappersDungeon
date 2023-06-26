using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject pause;
    public GameObject win;
    public GameObject def;
    Rigidbody2D rb;
    Animator anim;
    public int speed = 1;
    private bool grounded;
    public LayerMask whatIsGround;
    public int jumpForce = 1;
    public float rayDistance = 0.03f;
    public bool isGrounded = false;
    public bool death = false;
    public bool adeath = false;
    public int fly = 1;

    public bool key = false;
    public GameObject dispkey;
    public GameObject saveIcon;

    public AudioSource jumpSound;
    public AudioSource deathSound;
    public AudioSource doorOpenSound;
    public AudioSource doorStuckSound;
    public AudioSource checkpointSound;
    public AudioSource giveSound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;

        if (SaveManager.instance.hasLoaded)
        {
            gameObject.transform.position = SaveManager.instance.activeSave.respawnPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!death)
        {
            if(Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(new Vector2(0, jumpForce / 1.5f), ForceMode2D.Impulse);
                jumpSound.Play();
            }

            if(Input.GetButton("Horizontal"))
            {
                anim.SetBool("isWalk", true);
                rb.velocity = new Vector2(speed*Input.GetAxis("Horizontal"), rb.velocity.y);

                if(Input.GetAxis("Horizontal") > 0)
                {
                    anim.SetBool("isRight", true);
                }
                if(Input.GetAxis("Horizontal") < 0)
                {
                    anim.SetBool("isRight", false);
                }
            }
            else
            {
                anim.SetBool("isWalk", false);
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                pause.SetActive(true);
                Time.timeScale = 0;
            }
        }

        if (death && !adeath)
        {
            adeath = true;
            anim.SetBool("death", true);
        }
    }
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, rayDistance, whatIsGround);

        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Trap"))
        {
            death = true;
            deathSound.Play();
            def.SetActive(true);
        }
        if(col.gameObject.CompareTag("Finish"))
        {
            Destroy(col.gameObject);
            win.SetActive(true);
        }
        if(col.gameObject.CompareTag("key"))
        {
            Destroy(col.gameObject);
            giveSound.Play();
            key = true;
            dispkey.SetActive(true);
        }
        if(col.gameObject.CompareTag("Respawn"))
        {
            checkpointSound.Play();
            SaveManager.instance.activeSave.respawnPosition = col.gameObject.transform.position;
            saveIcon.SetActive(true);
            SaveManager.instance.Save();
            Invoke("hideSaveIcon", 4);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("ladder") && Input.GetButton("Vertical"))
        {
            rb.AddForce(new Vector2(0, Input.GetAxis("Vertical") * fly));
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("door") && key)
        {
            col.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            col.gameObject.GetComponent<Animator>().SetBool("opened", true);
            doorOpenSound.Play();
            key = false;
            dispkey.SetActive(false);
        }
        else if(col.gameObject.CompareTag("door") && !key)
        {
            doorStuckSound.Play();
        }
    }

    public void Unpause()
    {
        pause.SetActive(false);
        Time.timeScale = 1f;
    }

    void hideSaveIcon()
    {
        saveIcon.SetActive(false);
    }
}
