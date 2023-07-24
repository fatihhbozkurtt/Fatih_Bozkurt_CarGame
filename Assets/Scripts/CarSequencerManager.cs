using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSequencerManager : SingletonBehavior<CarSequencerManager>
{
    //Events
    public event System.Action<Transform> NewCarAssignedEvent;
    public event System.Action ResetCarsEvent;

    [Header("Configuration")]
    [SerializeField] List<CarController> cars;
    [SerializeField] Color[] colors;
    int activeCarIndex;
    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < cars.Count; i++)
        {
            cars[i].SetCarIndex(i);
            cars[i].GetComponentInChildren<Renderer>().material.color = colors[i];
        }

        CarController initialCar = GetActiveCar().GetComponent<CarController>();
        initialCar.CarCrushedEvent += OnActiveCarCrushed;
    }

    private void OnActiveCarCrushed()
    {
        ResetCarsEvent?.Invoke();
        TimeManager.instance.FreezeTime();
    }

    public void AssignNewActiveCar(int formerActiveCarIndex)
    {
        CarController formerCar = cars[formerActiveCarIndex];
        formerCar.CarCrushedEvent -= OnActiveCarCrushed;

        if (formerActiveCarIndex == cars.Count - 1)
        {
            GameManager.instance.EndGame(true); // level is completed
            return;
        }

        activeCarIndex = formerActiveCarIndex + 1;
        CarController activeCar = GetActiveCar().GetComponent<CarController>();
        activeCar.gameObject.SetActive(true);
        activeCar.CarCrushedEvent += OnActiveCarCrushed;

        //Event Triggers/Invokers
        activeCar.TriggerDrivingStartedEvent();
        ResetCarsEvent?.Invoke();
        NewCarAssignedEvent?.Invoke(GetActiveCar());
    }
    public Transform GetActiveCar()
    {
        return cars[activeCarIndex].transform;
    }
}
