using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAssigner : SingletonBehavior<RotationAssigner>
{
    Transform activeCar;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float rotationSpeed;
    Vector3 rot;
    float yRot;
    bool block;

    private void Start()
    {
        activeCar = CarSequencerManager.instance.GetActiveCar();
        activeCar.GetComponent<CarController>().CarCrushedEvent += OnCarCollides;
        CarSequencerManager.instance.NewCarAssignedEvent += OnNewCarAssigned;
    }

    private void OnNewCarAssigned(Transform newCar)
    {
        activeCar = newCar; 
    }

    private void OnCarCollides()
    {
        block = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLevelActive) return;

        if (Input.GetMouseButton(0))
        {
           if(block) block = false; 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, targetLayer))
            {

                if (hit.collider.GetComponent<Sides>().sideType == SideType.Left)
                {
                    yRot -= rotationSpeed * Time.deltaTime;
                }
                else
                {
                    yRot += rotationSpeed* Time.deltaTime;
                }

            }
        }

        rot = new Vector3(0, yRot, 0);
        yRot = 0;

    }

    private void LateUpdate()
    {
        if (block) return;

        activeCar.transform.Rotate(rot);
        rot = Vector3.zero;
    }
}
