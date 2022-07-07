using System.Collections;
using UnityEngine;

public class AppStartActivityManager : MonoBehaviour
{
    public static AppStartActivityManager Instance;

    [Header("AppValueController Prefab")]
    public GameObject AppValueControllerPrefab;

    [Header("LanguageManager Prefab")]
    public GameObject LanguageManagerPrefab;

    [Header("UILoadingCanvasPrefab Prefab")]
    public GameObject UILoadingCanvasPrefab;

    [Header("SoundManager Prefab")]
    public GameObject SoundManagerPrefab;

    [Header("AutoQualityManager Prefab")]
    public GameObject AutoQualityManagerPrefab;

    [Header("UIManager Prefab")]
    public GameObject UIManagerPrefab;

    [Header("Character Manager")]
    public GameObject CharacterManagerPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //AppValueController
        GameObject AppValueControllerPrefabTemp = Instantiate(AppValueControllerPrefab);

        //LanguageManage
        GameObject LanguageManagerPrefabTemp = null;
        if (AppValueControllerPrefabTemp)
            LanguageManagerPrefabTemp = Instantiate(LanguageManagerPrefab);

        //UILoadingCanvasPrefab
        GameObject UILoadingCanvasPrefabTemp = null;
        if (LanguageManagerPrefabTemp)
            UILoadingCanvasPrefabTemp = Instantiate(UILoadingCanvasPrefab);

        //SoundManager
        GameObject SoundManagerPrefabTemp = null;
        if (UILoadingCanvasPrefabTemp)
            SoundManagerPrefabTemp = Instantiate(SoundManagerPrefab);

        //AutoQualityManager
        GameObject AutoQualityManagerPrefabTemp = null;
        if (SoundManagerPrefabTemp)
            AutoQualityManagerPrefabTemp = Instantiate(AutoQualityManagerPrefab);

        GameObject CharacterManagerPrefabTemp = null;
        if (AutoQualityManagerPrefabTemp)
            CharacterManagerPrefabTemp = Instantiate(CharacterManagerPrefab);

        //UIManager
        GameObject UIManagerPrefabTemp = null;
        if (CharacterManagerPrefabTemp)
            UIManagerPrefabTemp = Instantiate(UIManagerPrefab);

        if (UIManagerPrefabTemp)
            AppValueController.Instance.DebugLog("AppStartActivityManager Complete", Color.green);
        else
            AppValueController.Instance.DebugLog("AppStartActivityManager ERROR", Color.red);

        FirstRunApp();
    }

    public void FirstRunApp()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString(AppValueController.Instance.FirstRunApp)))
        {
            PlayerPrefs.SetString(AppValueController.Instance.FirstRunApp, "active");

            PlayerPrefs.SetInt(AppValueController.Instance.ScreenWidthText, Screen.width);
            PlayerPrefs.SetInt(AppValueController.Instance.ScreenHeightText, Screen.height);

            PlayerPrefs.SetInt(AppValueController.Instance.MONEYName, AppValueController.Instance.MONEY);

            //Settings
            PlayerPrefs.SetInt(AppValueController.Instance.QualityIndexText, 1);
            //QualitySettings.SetQualityLevel(1);

            PlayerPrefs.SetString(AppValueController.Instance.SoundIndexText, "1");
            PlayerPrefs.SetString(AppValueController.Instance.MusicIndexText, "1");
        }

        AppValueController.Instance.MONEY = PlayerPrefs.GetInt(AppValueController.Instance.MONEYName);

        AppValueController.Instance.ScreenWidth = PlayerPrefs.GetInt(AppValueController.Instance.ScreenWidthText);
        AppValueController.Instance.ScreenHeight = PlayerPrefs.GetInt(AppValueController.Instance.ScreenHeightText);

        //Start Load APP
        StartCoroutine(LoadApp());
    }

    public IEnumerator LoadApp()
    {
        UILoading.Instance.TargetSliderValue = 0.5f;

        UIStart();

        yield return null;
    }

    public void UIStart()
    {
        UIManager.Instance.UICreatePrefabs((int)UIManager.UIPrefabNames.MainMenu);

        StartCoroutine(UILoading.Instance.WaitLoading());
    }
}
