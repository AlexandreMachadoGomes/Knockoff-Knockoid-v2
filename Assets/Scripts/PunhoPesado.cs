using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunhoPesado : MonoBehaviour
{
    private float punchTimer = 0;
    private Rigidbody2D rigidBody;
    private Collider2D collider;

    public float punchSpeed = 1;
    public int punchDamage = 14;
    private Vector3 punchAux = Vector3.zero;

    public Transform player;

    private void MovePunch()
    {


        if (punchTimer < 0.3f)
            punchAux -= transform.right * punchSpeed * 0.007f;
        else if (punchTimer < 0.5f)
        {
            collider.enabled = true;
            transform.localScale *= 1.06f;
            punchAux += transform.right * punchSpeed * 0.030f;
        }
        else if (punchTimer >= 0.4f)
            Destroy(this.gameObject);


        rigidBody.MovePosition(player.position + transform.right * 0.2f + punchAux);



        punchTimer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != player)
            collision.transform.GetComponent<Player>().ReceiveKnockback(transform.right * 1.6f, punchDamage);

    }



    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePunch();
    }
}
