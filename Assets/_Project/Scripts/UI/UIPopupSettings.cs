using UnityEngine;
using UnityEngine.UI;

public class UIPopupSettings : MonoBehaviour
{
    [Header("Image Component")]
    public Image SoundButtonImage;
    public Image VibrateButtonImage;

    public Color ActiveColor;
    public Color DeActiveColor;

    private void OnEnable()
    {
        UIUpdate(); 
    }

    public void UIUpdate()
    {
        CheckSprites();
    }

    public void CheckSprites()
    {
        if (PlayerPrefs.GetString(AppValueController.Instance.SoundIndexText).Contains("1"))
        {
            SoundButtonImage.color = ActiveColor;
        }
        else
        {
            SoundButtonImage.color = DeActiveColor;
        }
    }

    public void SetOkayButton()
    {
        UIManager.Instance.UIPopupDestroyPrefabTransform();
    }

    public void SetSoundButton()
    {
        if (PlayerPrefs.GetString(AppValueController.Instance.SoundIndexText).Contains("1"))
        {
            //Kapat
            SoundButtonImage.color = DeActiveColor;
            PlayerPrefs.SetString(AppValueController.Instance.SoundIndexText, "0");
            PlayerPrefs.SetString(AppValueController.Instance.MusicIndexText, "0");

        }
        else
        {
            //Ac
            SoundButtonImage.color = ActiveColor;
            PlayerPrefs.SetString(AppValueController.Instance.SoundIndexText, "1");
            PlayerPrefs.SetString(AppValueController.Instance.MusicIndexText, "1");
        }
    }

    public void PrivacyButton()
    {
        Application.OpenURL(AppValueController.Instance.PrivacyLink);
    }

    public void UserAgreementButton()
    {
        Application.OpenURL(AppValueController.Instance.UserAgreementLink);
    }

    public void LicenseAgreementButton()
    {
        Application.OpenURL(AppValueController.Instance.LicenseAgreementLink);
    }
}