using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float verticalSpeed;
    bool blockMovement;
    CarController carController;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        carController = GetComponent<CarController>();

        carController.ManuelDrivingEndedEvent += OnDrivingStarted;
        carController.CarCrushedEvent += OnCarCrushes;
        carController.ManuelDrivingEndedEvent += ArrivedAtEnd;
    }

    private void OnDrivingStarted()
    {
        if (!carController.manuelDrvingCompleted)
            blockMovement = false;
    }

    private void ArrivedAtEnd()
    {
        blockMovement = true;
    }

    private void OnCarCrushes()
    {
        //blockMovement = true;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (blockMovement)
        {
            rb.velocity = Vector3.zero;
            return;

        }


        rb.velocity = transform.forward * verticalSpeed * Time.deltaTime;

    }
}
