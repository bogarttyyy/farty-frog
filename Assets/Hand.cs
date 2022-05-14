using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField]
    private KeyCode keyBind;

    [SerializeField]
    private Transform torso;

    public Vector2 handChargeLocation = Vector2.zero;

    public float force;

    private Vector3 mousePos;

    private Rigidbody2D rb;

    private void Start() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D result))
        {
            rb = result;        
        }
    }

    private void FixedUpdate()
    {
        #region DebugControls
            
        if (Input.GetKey(keyBind) && Input.GetMouseButton(0))
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
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    rb.MovePosition(Vector2.MoveTowards(rb.position, mousePosV2, force));
                }
            }
            else
            {
                transform.position = mousePosV2;
            }
        }
        
        if (Input.GetKeyUp(keyBind) && Input.GetMouseButtonUp(0))
        {
            if (rb != null && rb.bodyType != RigidbodyType2D.Dynamic)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;                    
            }
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        ChargeGrab();
        ReleaseCharge();
    }
    
    private void ChargeGrab()
    {
        if (Input.GetKey(keyBind) && !Input.GetMouseButton(0))
        {
            var originalPos = (Vector2)transform.localPosition;

            var distanceFromTorso = Vector2.Distance((Vector2)torso.transform.position, transform.position);

            var lerpedPos = Vector2.Lerp(originalPos, handChargeLocation, 1 * Time.deltaTime);
            
            if (rb != null)
            {
                if(rb.bodyType != RigidbodyType2D.Kinematic){
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }
                transform.localPosition = lerpedPos;
            }
            else
            {
                transform.position = handChargeLocation;
            }
        }
    }

    private void ReleaseCharge()
    {
        if (Input.GetKeyUp(keyBind) && !Input.GetMouseButton(0))
        {
            var localForce = new Vector2(0, force);

            var mouseDir = mousePos - transform.position;
            mouseDir.z = 0;
            mouseDir = mouseDir.normalized;

            var addedForce = mouseDir * force;

            // Debug.Log(addedForce);

            if (rb.bodyType != RigidbodyType2D.Dynamic)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
            
            // rb.AddForce(addedForce);
            
            var torsoRb = GetTorsoRigidBody();
            if (torsoRb != null)
            {
                torsoRb.AddForce(addedForce / 2);
            }
        }
    }

    private Rigidbody2D GetTorsoRigidBody()
    {
        if (torso.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            return rb;
        }

        return null;
    }
}
