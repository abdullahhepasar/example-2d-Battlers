using UnityEngine;

public class AppValueController : MonoBehaviour
{
    public static AppValueController Instance;

    [Header("MANAGERS")]
    public string LanguageSettingName = "Language";

    [Space(5f)]
    //Header
    [Header("VALUES")]
    public string FirstRunApp = "FIRSTRUNAPP";

    public string ScreenWidthText = "ScreenWidth";
    public string ScreenHeightText = "ScreenHeight";
    [HideInInspector] public int ScreenWidth, ScreenHeight;

    [Header("Economy")]
    public string MONEYName = "MONEY";
    public int MONEY = 20000;
    public int CurrentLevelWinCoin = 0;

    [Header("Quality Settings")]
    public string QualityIndexText = "QualityIndex";

    [Header("Sound Settings")]
    public string SoundIndexText = "SoundIndex";
    public string MusicIndexText = "MusicIndex";

    [Header("Privacy Policy Links")]
    public string PrivacyConfirmText = "PrivacyConfirm";
    public string PrivacyLink = "";
    public string UserAgreementLink = "";
    public string LicenseAgreementLink = "";

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

    #region GAME ECONOMY

    public bool GameMoneyEconomy(int fee)
    {
        // Not Enough Money
        if (MONEY + fee < 0)
            return false;

        MONEY = PlayerPrefs.GetInt(MONEYName);

        MONEY += fee;

        PlayerPrefs.SetInt(MONEYName, MONEY);

        return true;
    }

    public bool GameMoneyEconomyIsEnoughMoneyForBuy(int fee)
    {
        MONEY = PlayerPrefs.GetInt(MONEYName);

        // Not Enough Money
        if (MONEY + fee < 0)
            return false;
        else
            return true;
    }

    public int GetMoney()
    {
        return MONEY;
    }

    #endregion

    #region TOOLS
    public void DebugLog(string text, Color color)
    {
        Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>");
    }
    #endregion
}
