using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour, ICrushable
{
    // Events
    public event System.Action ManuelDrivingStartedEvent;
    public event System.Action ManuelDrivingEndedEvent;
    public event System.Action CarCrushedEvent;

    [SerializeField] int carIndex;
    [SerializeField] Transform start;
    [SerializeField] Transform end;
    [HideInInspector] public bool manuelDrvingCompleted;

    private void Start()
    {
        start.parent = null;
        end.parent = null;
        SetInitParams();
        CarSequencerManager.instance.ResetCarsEvent += OnReset;
    }
    
    #region  Setters
    public void SetCarIndex(int index)
    {
        carIndex = index;
    }
    public void SetInitParams()
    {
        transform.SetLocalPositionAndRotation(start.position, start.localRotation);
    }
    private void OnReset()
    {
        SetInitParams();
    }
    #endregion
    #region Interactions
    private void OnTriggerEnter(Collider other)
    {
        if (!gameObject.activeInHierarchy) return;

        if (other.gameObject.TryGetComponent(out ICrushable _))
        {
            SetInitParams();
            CarCrushedEvent?.Invoke();
        }
        if (other.transform == end)
        {
            other.gameObject.SetActive(false);
            SetInitParams();
            ManuelDrivingEndedEvent?.Invoke();
            CarSequencerManager.instance.AssignNewActiveCar(carIndex);

            start.gameObject.SetActive(false);
            manuelDrvingCompleted = true;
        }
    }
    public void TriggerDrivingStartedEvent()
    {
        if (!manuelDrvingCompleted) ManuelDrivingStartedEvent?.Invoke();
    }
    #endregion
}
