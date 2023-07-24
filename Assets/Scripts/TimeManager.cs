using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : SingletonBehavior<TimeManager>
{
    private void Start()
    {
        CarSequencerManager.instance.NewCarAssignedEvent += OnNewCarAssigned;
       
    }

    private void OnNewCarAssigned(Transform obj)
    {
        FreezeTime();
    }

    public void FreezeTime()
    {
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.timeScale != 1) Time.timeScale = 1;
        }
    }
}
