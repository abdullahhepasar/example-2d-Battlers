using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    public enum LanguageList
    {
        EN
    }

    [HideInInspector]
    public string LoadingText, PlayText, LevelCompletedText, ClaimRewardsText,
        BackMenuText, LevelFailedText, NextLevel, LevelText, 
        ToBattleText, NextText, BackText, AttackText;

    void Awake()
    {
        //Check if instance already exists
        if (Instance == null)
        {
            //if not, set instance to this
            Instance = this;
        }
        //If instance already exists and it's not this:
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayerPrefs.SetString(AppValueController.Instance.LanguageSettingName, LanguageList.EN.ToString());

        CheckLanguage();
    }

    public void CheckLanguage()
    {
        if (PlayerPrefs.GetString(AppValueController.Instance.LanguageSettingName).Contains(LanguageList.EN.ToString()))
        {
            LoadingText = "LOADING";
            PlayText = "PLAY";

            //MainMenu
            ToBattleText = "TO BATTLE";
            BackMenuText = "BACK MENU";
            LevelText = "LEVEL";
            LevelFailedText = "LEVEL FAILED";
            LevelCompletedText = "LEVEL COMPLETED";
            ClaimRewardsText = "CLAIM X2";
            NextLevel = "SKIP LEVEL";

            NextText = "NEXT";
            BackText = "BACK";
            AttackText = "ATTACK";
        }
    }
}
