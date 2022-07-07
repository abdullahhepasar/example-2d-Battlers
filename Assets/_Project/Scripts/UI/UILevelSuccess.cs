using UnityEngine;
using UnityEngine.UI;

public class UILevelSuccess : MonoBehaviour
{
    [Header("Text Language")]
    public Text LevelCompletedText;
    public Text BackMenuText;

    [Header("Text Componet")]
    public Text CoinCountText;

    private void OnEnable()
    {
        UIUpdate();
    }

    private void Start()
    {
        UIManager.Instance.UIPopupDestroyPrefabTransform();
        AppValueController.Instance.GameMoneyEconomy(AppValueController.Instance.CurrentLevelWinCoin);
    }

    public void UIUpdate()
    {
        SetLanguage();

        CoinCountText.text = "+" + AppValueController.Instance.CurrentLevelWinCoin.ToString();
    }

    private void SetLanguage()
    {
        LevelCompletedText.text = LanguageManager.Instance.LevelCompletedText;
        BackMenuText.text = LanguageManager.Instance.BackMenuText;
    }

    #region SET BUTTONS

    public void UIChange(int index)
    {
        UIManager.Instance.UICreatePrefabs(index);
    }

    public void SetNoThanksButton()
    {
        UIManager.Instance.UICreatePrefabs((int)UIManager.UIPrefabNames.MainMenu);
    }

    #endregion
}
