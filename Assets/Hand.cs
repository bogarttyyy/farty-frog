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
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D result))
        {
            rb = result;        
        }
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKey(keyBind))
        {
            Vector2 mousePosV2 = (Vector2)mousePos;

            var distance = Vector2.Distance((Vector2)transform.position, mousePosV2);

            if (rb != null)
            {
                if (distance < 0.25f)
                {
                    rb.bodyType = RigidbodyType2D.Static;
                }
                else
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.MovePosition(Vector2.MoveTowards(rb.position, mousePosV2, force));
                }
            }
            else
            {
                transform.position = mousePosV2;
            }


            
            // rb.AddForce(mousePosV2, ForceMode2D.Impulse);
            // transform.position = Vector3.Lerp(transform.position, mousePos, Time.deltaTime);

            // transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }

        // if (Input.GetKeyUp(keyBind))
        // {
        //     if (rb.bodyType != RigidbodyType2D.Dynamic)
        //     {
        //         rb.bodyType = RigidbodyType2D.Dynamic;                    
        //     }
        // }
                
    }
}
