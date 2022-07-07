using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UIAnimatorCore;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public enum UIPrefabNames
    {
        None,
        MainMenu,
        UIStore,
        UIChestReward,
        UILevelSuccess,
        UILevelFail,
        UIPopupSettings, 
        UIGameplay
    };

    [Serializable]
    public class UIPrefabs
    {
        public string name;
        public UIPrefabNames UIPrefabNames;
        public GameObject prefab;
        public string UIResourcesName;
    }
    public UIPrefabs[] UIPrefab;

    [Space(10f)]
    public Transform UIPrefabCreateTransform;
    public Transform UIPopupPrefabCreateTransform;
    public Transform UITempTransform;

    public CanvasScaler canvasScaler;

    bool UIAnimationActive = false;
    bool UIPopupAnimationActive = false;

    [Header("Resources Folder Name")]
    public string UIResourcesFolderName = "UIPrefabs";

    private void Awake()
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

    // Start is called before the first frame update
    void Start()
    {
        CheckTabletMode();
    }

    public void UICreatePrefabs(int _indexPrefab)
    {
        if (!UIAnimationActive)
            StartCoroutine(UICreatePrefabsAnimation(_indexPrefab));

        UIAnimationActive = true;
    }

    IEnumerator UICreatePrefabsAnimation(int _index)
    {
        GameObject tempUI = null;

        UIPrefabNames uIPrefabNames = (UIPrefabNames)_index;

        for (int i = 0; i < UIPrefab.Length; i++)
        {
            if (uIPrefabNames == UIPrefab[i].UIPrefabNames)
            {
                //Resourece Load
                ResourceRequest resource = null;
                resource = Resources.LoadAsync(UIResourcesFolderName + "/" + UIPrefab[i].UIResourcesName, typeof(GameObject));
                while (!resource.isDone)
                {
                    yield return resource;
                }
                tempUI = Instantiate(resource.asset, UITempTransform) as GameObject;

                tempUI.SetActive(false);
            }
        }

        yield return StartCoroutine(UIDestroyPrefabTransformAnimation());
        Resources.UnloadUnusedAssets();

        if (tempUI != null)
        {
            foreach (Transform item in UIPrefabCreateTransform)
            {
                Destroy(item.gameObject);
            }

            tempUI.transform.parent = UIPrefabCreateTransform;
            tempUI.SetActive(true);

            //tempUI.GetComponent<UIAnimator>().PlayAnimation(AnimSetupType.Loop);

        }

        UIAnimationActive = false;

        yield return null;
    }

    public IEnumerator UIDestroyPrefabTransformAnimation()
    {
        float timer = 0f;
        //Sound
        SoundManager.Instance.CreateSoundPrefab(SoundManager.SoundID.Click);

        foreach (Transform item in UIPrefabCreateTransform)
        {
            if (item.GetComponent<UIAnimator>())
            {
                item.GetComponent<UIAnimator>().PlayAnimation(AnimSetupType.Outro);

                if (timer < item.GetComponent<UIAnimator>().GetAnimationDuration(AnimSetupType.Outro))
                    timer = item.GetComponent<UIAnimator>().GetAnimationDuration(AnimSetupType.Outro);
            }
        }

        yield return new WaitForSeconds(timer);
    }

    public void UIDestroyPrefabTransform()
    {
        //Sound
        SoundManager.Instance.CreateSoundPrefab(SoundManager.SoundID.Click);

        foreach (Transform item in UIPrefabCreateTransform)
        {
            Destroy(item.gameObject);
        }
    }

    public void UIPopupCreatePrefabs(int _indexPrefab)
    {
        if (!UIPopupAnimationActive)
            StartCoroutine(UIPopupCreatePrefabsAnimation(_indexPrefab));

        UIPopupAnimationActive = true;
    }

    IEnumerator UIPopupCreatePrefabsAnimation(int _index)
    {
        GameObject tempUI = null;

        UIPrefabNames uIPrefabNames = (UIPrefabNames)_index;

        for (int i = 0; i < UIPrefab.Length; i++)
        {
            if (uIPrefabNames == UIPrefab[i].UIPrefabNames)
            {
                //Resourece Load
                ResourceRequest resource = null;
                resource = Resources.LoadAsync(UIResourcesFolderName + "/" + UIPrefab[i].UIResourcesName, typeof(GameObject));
                while (!resource.isDone)
                {
                    yield return resource;
                }
                tempUI = Instantiate(resource.asset, UITempTransform) as GameObject;

                tempUI.SetActive(false);
            }
        }

        yield return StartCoroutine(UIPopupDestroyPrefabTransformAnimation());
        Resources.UnloadUnusedAssets();

        if (tempUI != null)
        {
            foreach (Transform item in UIPopupPrefabCreateTransform)
            {
                Destroy(item.gameObject);
            }

            tempUI.transform.parent = UIPopupPrefabCreateTransform;
            tempUI.SetActive(true);
        }

        UIPopupAnimationActive = false;

        yield return null;
    }

    public IEnumerator UIPopupDestroyPrefabTransformAnimation()
    {
        float timer = 0f;
        //Sound
        SoundManager.Instance.CreateSoundPrefab(SoundManager.SoundID.Click);

        foreach (Transform item in UIPopupPrefabCreateTransform)
        {
            if (item.GetComponent<UIAnimator>())
            {
                item.GetComponent<UIAnimator>().PlayAnimation(AnimSetupType.Outro);

                if (timer < item.GetComponent<UIAnimator>().GetAnimationDuration(AnimSetupType.Outro))
                    timer = item.GetComponent<UIAnimator>().GetAnimationDuration(AnimSetupType.Outro);
            }
        }

        yield return new WaitForSeconds(timer);

        foreach (Transform item in UIPopupPrefabCreateTransform)
        {
            Destroy(item.gameObject);
        }

        yield return null;
    }

    public void UIPopupDestroyPrefabTransform()
    {
        //Sound
        SoundManager.Instance.CreateSoundPrefab(SoundManager.SoundID.Click);

        foreach (Transform item in UIPopupPrefabCreateTransform)
        {
            Destroy(item.gameObject);
        }

        //StartCoroutine(UIPopupDestroyPrefabTransformAnimation());
    }

    public GameObject PopupConfirmCreate()
    {
        SoundManager.Instance.CreateSoundPrefab(SoundManager.SoundID.Click);

        return null;
    }

    public void CheckTabletMode()
    {
        float SceleScreenWitdhHeight = (float)AppValueController.Instance.ScreenHeight / (float)AppValueController.Instance.ScreenWidth;

        // HER UI DA 1 OLACAK ŞEKİLDE TASARIM YAPILDI
        canvasScaler.matchWidthOrHeight = 1.0f;
    }

    public void SetCanvasScaler(float scale)
    {
        float SceleScreenWitdhHeight = (float)AppValueController.Instance.ScreenHeight / (float)AppValueController.Instance.ScreenWidth;
    }
}
