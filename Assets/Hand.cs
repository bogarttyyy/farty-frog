using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField]
    private KeyCode keyBind;

    public float force;

    private Rigidbody2D rb;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKey(keyBind))
        {
            Vector2 mousePosV2 = (Vector2)mousePos;
            rb.MovePosition(Vector2.MoveTowards(rb.position, mousePosV2, force));

            var distance = Vector2.Distance((Vector2)transform.position, mousePosV2);

            if (distance < 0.25f)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
            else
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
            
            // rb.AddForce(mousePosV2, ForceMode2D.Impulse);
            // transform.position = Vector3.Lerp(transform.position, mousePos, Time.deltaTime);

            // transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }

        if (Input.GetKeyUp(keyBind))
        {
            if (rb.bodyType != RigidbodyType2D.Dynamic)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;                    
            }
        }
                
    }
}
