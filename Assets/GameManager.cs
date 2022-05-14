using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject pointOfReference;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(3))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Middle Mouse!");
        }
    }
}
