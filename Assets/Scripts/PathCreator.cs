using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PathCreator : MonoBehaviour
{
    float timeStamp;
    CarController carController;
    public List<Vector3> path = new();
    [SerializeField] bool canCreatePath;

    [Header("TEST")]
    [SerializeField] bool autoDrivingMode;
    [SerializeField] float lookSpeed;
    [SerializeField] float moveSpeed;
    bool pathIsCompleted;
    Vector3 lookTargetPos;
    Vector3 moveTargetPos;
    int step;
    const int LookStepOffset = 10;


    private void Awake()
    {
        carController = GetComponent<CarController>();

        carController.ManuelDrivingStartedEvent += OnDrivingStarted;
        carController.ManuelDrivingEndedEvent += OnDrivingEnded;
        carController.CarCrushedEvent += OnCarCrushed;
    }
    private void OnEnable()
    {
        if (!carController.manuelDrvingCompleted) canCreatePath = true;
    }

    private void OnDrivingStarted()
    {
        canCreatePath = true;
    }
    private void OnDrivingEnded()
    {
        canCreatePath = false;
        autoDrivingMode = true;

    }

    private void OnCarCrushed()
    {
        if (carController.manuelDrvingCompleted)
        {
            OnPathIsEnded();
        }
        else
            path.Clear();
    }



    private void FixedUpdate()
    {
        if (!GameManager.instance.isLevelActive) return;
        if (!canCreatePath) return;

        timeStamp += Time.deltaTime;

        if (timeStamp >= 0.1f)
        {
            path.Add(transform.position);
            timeStamp = 0;
        }
    }

    private void Update()
    {
        if (!autoDrivingMode) return;

        if (!pathIsCompleted)
        {
            if (step == 0)
            {
                moveTargetPos = path[step];
                lookTargetPos = path[step + LookStepOffset];
                step++;
            }
            else
            {
                Vector3 targetPos = new Vector3(moveTargetPos.x, transform.position.y, moveTargetPos.z);
                if (Vector3.Distance(transform.position, targetPos) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);

                    Vector3 direction = lookTargetPos - transform.position;
                    direction.y = 0.0f;

                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * lookSpeed);
                }
                else
                {
                    if (step < path.Count - 1)
                    {
                        moveTargetPos = path[step];
                        if (step + LookStepOffset < path.Count)
                        {
                            lookTargetPos = path[step + LookStepOffset];
                        }
                        else
                        {
                            lookTargetPos = path[(step + path.Count - step) - 1];
                        }
                        step++;
                    }
                    else
                    {
                        pathIsCompleted = true;
                        OnPathIsEnded();
                    }
                }
            }
        }
    }

    void OnPathIsEnded()
    {
        carController.SetInitParams();
        pathIsCompleted = false;
        step = 0;
    }
}
