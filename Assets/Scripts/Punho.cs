using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punho : MonoBehaviour
{
    private float punchTimer = 0;
    private Rigidbody2D rigidBody;

    public float punchSpeed = 1;
    public int punchDamage = 10;
    private Vector3 punchAux = Vector3.zero;

    public Transform player;

    private void MovePunch()
    {


        if (punchTimer < 0.15f)
            punchAux += transform.right * punchSpeed * 0.03f;
        else
            punchAux -= transform.right * punchSpeed * 0.03f;

        rigidBody.MovePosition(player.position + transform.right * 0.2f + punchAux);

        if (punchTimer >= .3f)
            Destroy(this.gameObject);

        
        punchTimer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != player)
            collision.transform.GetComponent<Player>().ReceiveKnockback(transform.right, punchDamage);
        
    }



    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePunch();
    }
}
