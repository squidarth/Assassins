using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColliderHandler : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = this.gameObject.GetComponent<Rigidbody2D>();
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (myRigidbody == null)
        {
            myRigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        }
        Debug.Log("Collision Exited: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Setting V to 0");

            collision.rigidbody.velocity = Vector2.zero;
            collision.rigidbody.angularVelocity = 0;

            myRigidbody.velocity = Vector2.zero;
            myRigidbody.angularVelocity = 0;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Entered: " + collision.gameObject.tag);
        if (myRigidbody == null)
        {
            myRigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        }
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Setting V to 0");

            collision.rigidbody.velocity = Vector2.zero;
            collision.rigidbody.angularVelocity = 0;

            myRigidbody.velocity = Vector2.zero;
            myRigidbody.angularVelocity = 0;
        }
    }
}
