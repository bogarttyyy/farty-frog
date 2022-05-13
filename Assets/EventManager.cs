using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{

    public static event Action DebugSpawnEvent;
    public static event Action OnSpawnRoseEvent;
    public static event Action OnSliceDragEvent;
    public static event Action OnSliceUpEvent;
    public static event Action OnRoseHitEvent;
    public static event Action OnTrashOutOfBounds;
    public static event Action OnGameOver;
    public static event Action<float> OnAddPoints;
    // public static event Action OnTrashHitEvent;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DebugSpawnEvent?.Invoke();        
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            OnSpawnRoseEvent?.Invoke();
        }

        if (Input.GetMouseButtonDown(0))
        {
            OnSliceDragEvent?.Invoke();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnSliceUpEvent?.Invoke();
        }
    }

    public static void SpawnRoseEvent(){
        OnSpawnRoseEvent?.Invoke();
    }
}
