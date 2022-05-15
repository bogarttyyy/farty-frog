using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField]
    private KeyCode keyBind;

    [SerializeField]
    private KeyCode mouseBind;

    [SerializeField]
    private Rigidbody2D torso;

    [SerializeField]
    private Rigidbody2D otherHand;

    public Vector2 handChargeLocation = Vector2.zero;

    public float force = 75f;
    public float gripTimer = 3f;
    public float recoveryTimer = 2f;
    public float chargeTime = 1f;

    private Vector3 mousePos;
    private Rigidbody2D rb;

    private float maxGripTimer;
    private float maxRecoveryTimer;
    private float chargeGauge = 0f;

    private void Start() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D result))
        {
            rb = result;        
        }

        maxGripTimer = gripTimer;
        maxRecoveryTimer = recoveryTimer;
    }

    private void FixedUpdate()
    {
        #region DebugControls
            
        // if (Input.GetKey(keyBind) && Input.GetMouseButton(0))
        // {
        //     Vector2 mousePosV2 = (Vector2)mousePos;

        //     var distance = Vector2.Distance((Vector2)transform.position, mousePosV2);

        //     if (rb != null)
        //     {
        //         if (distance < 0.25f)
        //         {
        //             rb.bodyType = RigidbodyType2D.Static;
        //         }
        //         else
        //         {
        //             rb.bodyType = RigidbodyType2D.Kinematic;
        //             rb.MovePosition(Vector2.MoveTowards(rb.position, mousePosV2, force));
        //         }
        //     }
        //     else
        //     {
        //         transform.position = mousePosV2;
        //     }
        // }
        
        // if (Input.GetKeyUp(keyBind) && Input.GetMouseButtonUp(0))
        // {
        //     if (rb != null && rb.bodyType != RigidbodyType2D.Dynamic)
        //     {
        //         rb.bodyType = RigidbodyType2D.Dynamic;                    
        //     }
        // }
        #endregion
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GrabLedge();
        ChargeGrab();
        ReleaseCharge();
    }

    private void GrabLedge()
    {
        if (Input.GetKey(keyBind))
        {
            if (gripTimer >= 0)
            {
                rb.bodyType = RigidbodyType2D.Static;
                gripTimer -= Time.deltaTime;            
            }
            else
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }

        if (!Input.GetKey(keyBind))
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            if (gripTimer < maxGripTimer && gripTimer != maxGripTimer)
            {
                gripTimer += Time.deltaTime;            
            }
        }

        if (Input.GetKeyUp(keyBind))
        {
            StartCoroutine(ReleaseGrab());
            StopCoroutine(ReleaseGrab());
        }
    }

    public IEnumerator ReleaseGrab()
    {
        yield return new WaitForSeconds(0.05f);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void ChargeGrab()
    {
        if (Input.GetMouseButton(0) && otherHand.bodyType == RigidbodyType2D.Static)
        {
            if (rb != null && rb.bodyType != RigidbodyType2D.Static)
            {
                if(rb.bodyType != RigidbodyType2D.Kinematic){
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }

                if (chargeGauge < force)
                {
                    chargeGauge += force * Time.deltaTime;                
                }else if(chargeGauge >= force)
                {
                    chargeGauge = force;
                }
                var originalPos = (Vector2)transform.localPosition;

                // var distanceFromTorso = Vector2.Distance((Vector2)torso.transform.position, transform.position);

                var lerpedPos = Vector2.Lerp(originalPos, handChargeLocation, chargeTime * Time.deltaTime);
                
                if (rb != null)
                {
                    transform.localPosition = lerpedPos;
                }
                else
                {
                    transform.position = handChargeLocation;
                }
            }
        }
    }

    private void ReleaseCharge()
    {
        if (Input.GetMouseButtonUp(0) && !Input.GetKey(keyBind))
        {
            var mouseDir = mousePos - transform.position;
            mouseDir.z = 0;
            mouseDir = mouseDir.normalized;

            var addedForce = mouseDir * force;

            if (rb.bodyType != RigidbodyType2D.Dynamic)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }

            if (otherHand.bodyType == RigidbodyType2D.Static)
            {
                otherHand.bodyType = RigidbodyType2D.Dynamic;

                rb.AddForce(addedForce, ForceMode2D.Impulse);
                
                // var torsoRb = GetTorsoRigidBody();
                if (torso != null)
                {
                    torso.AddForce(addedForce * 0.75f, ForceMode2D.Impulse);
                }
            }
        }
    }
}
