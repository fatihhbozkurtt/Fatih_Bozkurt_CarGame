using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameStarter : MonoBehaviour, IPointerDownHandler
{
    public bool autoStart;

    bool readyToReceiveScreenTap = false;

    private void Start()
    {
        if (autoStart)
        {
            readyToReceiveScreenTap = false;
            StartCoroutine(AutoStartRoutine());
        }

        else
        {
            readyToReceiveScreenTap = true;
        }
    }

    IEnumerator AutoStartRoutine()
    {
        yield return null;
        yield return null;

        GameManager.instance.StartGame();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (readyToReceiveScreenTap)
        {
            readyToReceiveScreenTap = false;
            GameManager.instance.StartGame();
        }
    }
}
