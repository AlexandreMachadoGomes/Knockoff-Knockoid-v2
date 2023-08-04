using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float lifePercentage = 0;

    public KeyCode UP;
    public KeyCode DOWN;
    public KeyCode LEFT;
    public KeyCode RIGHT;
    public KeyCode PUNCH;
    public KeyCode PUNCHHEAVY;
    public KeyCode DASH;


    public float punchCD = 1.5f;
    public float heavyPunchCD = 2.5f;
    public bool isNPC;

    public float dashTime = .6f;
    public float dashCD = 1f;

    private bool isDroping = false;
    private bool isDead = false;

    private float dashCooldown = 0;
    private float punchCooldown = 0;
    public float damageCooldown = 0;

    public GameObject punho;
    public GameObject punhoPesado;
    private Rigidbody2D rigidBody;


    public int moveSpeed = 10;

    private void PlayerMovement()
    {
            if (Input.GetKey(UP))
                rigidBody.AddForce(Vector3.up * moveSpeed);
            if (Input.GetKey(LEFT))
                rigidBody.AddForce(Vector3.left * moveSpeed);
            if (Input.GetKey(RIGHT))
                rigidBody.AddForce(Vector3.right * moveSpeed);
            if (Input.GetKey(DOWN))
                rigidBody.AddForce(Vector3.down * moveSpeed);
    }

    private void Punch()
    {
        

        if (punchCooldown > 0)
            punchCooldown -= Time.deltaTime;

        if (Input.GetKey(PUNCH) && punchCooldown <= 0)
        {
            Vector3 playerMoveDir = rigidBody.velocity;
            GameObject aux = Instantiate(punho, transform.position, Quaternion.identity, this.transform);
            aux.transform.right = playerMoveDir;
            aux.GetComponent<Punho>().player = this.transform;
            dashCooldown = punchCD;
            punchCooldown = punchCD;

        }
        else if (Input.GetKey(PUNCHHEAVY) && punchCooldown <= 0)
        {
            Vector3 playerMoveDir = rigidBody.velocity;
            GameObject aux = Instantiate(punhoPesado, transform.position    , Quaternion.identity, this.transform);
            aux.transform.right = playerMoveDir;
            aux.GetComponent<PunhoPesado>().player = this.transform;
            dashCooldown = heavyPunchCD;
            punchCooldown = heavyPunchCD;

        }
    }

    private void Dash()
    {
        

        if (dashCooldown > 0)
            dashCooldown -= Time.deltaTime;

        if (Input.GetKey(DASH) && dashCooldown <= 0)
        {
            Vector2 velocity = rigidBody.velocity;
            rigidBody.velocity = Vector2.zero;  
            PlayerMovement();
            rigidBody.AddForce(velocity.normalized * 10, ForceMode2D.Impulse);
            dashCooldown = dashCD;
            punchCooldown = dashCD * .65f;
        }
    }


    public void ReceiveKnockback(Vector3 knockbackPosition, int damage)
    {
        if (damageCooldown >= 0.2f + lifePercentage / 500 && dashCooldown < dashCD - dashTime)
        {
            rigidBody.AddForce(knockbackPosition * (10 + lifePercentage / 10), ForceMode2D.Impulse);
            lifePercentage += damage;
            damageCooldown = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("wall") && rigidBody.velocity.magnitude > 3 && !isDead)
        {
            float damage = rigidBody.velocity.magnitude;
            if (damage < 40)
                lifePercentage += rigidBody.velocity.magnitude;
            else
                lifePercentage += 40;
            if (lifePercentage > 400)
            {
                StartCoroutine(Die());
            }
        }
    }


    public void DropDown()
    {
        isDroping = true;
    }


    private IEnumerator Die()
    {
        GetComponent<SpriteRenderer>().color = Color.black;
        isDead = true;

        yield return new WaitForSecondsRealtime(4f);

        SceneManager.LoadScene("StageSelection");
    }








    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("StageSelection");
        }


        if (!isDead)
        {
            if (damageCooldown < 0.5f + lifePercentage / 500)
                damageCooldown += Time.deltaTime;


            if (!isNPC && !isDroping)
            {
                PlayerMovement();
                Punch();
                Dash();
            }

            if (isDroping)
            {
                if (transform.localScale.x > 0.1f)
                {
                    transform.localScale -= Vector3.one * 0.02f;
                }
                else
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                    StartCoroutine(Die());
                }
            }
        }
    }
}
