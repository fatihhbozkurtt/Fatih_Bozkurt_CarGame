using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;
    public enum PanelType
    {
        MainMenu, Game, Success, Fail
    }

    [Header("Canvas Groups")]
    public CanvasGroup mainMenuCanvasGroup;
    public CanvasGroup gameCanvasGroup;
    public CanvasGroup successCanvasGroup;
    public CanvasGroup failCanvasGroup;

    [Space]

    [Header("Standard Objects")]
    public Image screenFader;

    CanvasGroup[] canvasArray;

 
     void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        canvasArray = new CanvasGroup[System.Enum.GetNames(typeof(PanelType)).Length];

        canvasArray[(int)PanelType.MainMenu] = mainMenuCanvasGroup;
        canvasArray[(int)PanelType.Game] = gameCanvasGroup;
        canvasArray[(int)PanelType.Success] = successCanvasGroup;
        canvasArray[(int)PanelType.Fail] = failCanvasGroup;

        foreach (CanvasGroup canvas in canvasArray)
        {
            canvas.gameObject.SetActive(true);
            canvas.alpha = 0;
        }

        FadeInScreen(1f);
        ShowPanel(PanelType.MainMenu);

    }

    void Start()
    {
        GameManager.instance.LevelStartedEvent += (() => ShowPanel(PanelType.Game));
        GameManager.instance.LevelSuccessEvent += (() => ShowPanel(PanelType.Success));
        GameManager.instance.LevelFailedEvent += (() => ShowPanel(PanelType.Fail));
    }



    public void ShowPanel(PanelType panelId)
    {
        int panelIndex = (int)panelId;

        for (int i = 0; i < canvasArray.Length; i++)
        {
            if (i == panelIndex)
            {
                FadePanelIn(canvasArray[i]);
            }

            else
            {
                FadePanelOut(canvasArray[i]);
            }
        }
    }

    #region ButtonEvents
    public void OnTapRestart()
    {
        FadeOutScreen(1, callback: GameManager.instance.RestartStage);
    }

    public void OnTapContinue()
    {
        FadeOutScreen(1, callback: GameManager.instance.NextStage);


    }
    #endregion

    #region FadeInOut
    private void FadePanelOut(CanvasGroup panel)
    {
        panel.DOFade(0, 0.75f);
        panel.blocksRaycasts = false;
    }

    private void FadePanelIn(CanvasGroup panel)
    {
        panel.DOFade(1, 0.75f);
        panel.blocksRaycasts = true;
    }

    public void FadeOutScreen(float duration, TweenCallback callback = null)
    {
        screenFader.raycastTarget = true;
        screenFader.DOFade(1, duration)
            .From(0)
            .OnComplete(callback);
    }

    public void FadeInScreen(float duration, TweenCallback callback = null)
    {
        screenFader.raycastTarget = false;
        screenFader.DOFade(0, duration)
            .From(1)
            .OnComplete(callback);
    }
    #endregion
}
