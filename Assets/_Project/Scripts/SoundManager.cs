using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource MusicSource;

    [Serializable]
    public class MusicList
    {
        public string audioClipName;
        [Tooltip("Default Volume")]
        public float musicVolume;
        [Tooltip("Gameplay deki ses oranı")]
        public float musicLowVolume;
        [Tooltip("Menu deki ses Kısılma oranı")]
        public float musicLowVolumeInMenu;
    }

    public MusicList[] musicLists;
    public int MusicIndex = 0;

    public string MusicResourceName = "Sound";
    public string SoundResourceName = "Sound/UI";

    public enum SoundID
    {
        Click
    }

    [Serializable]
    public class SoundList
    {
        public SoundID soundID;
        public string audioClipName;
        public float volume;
        public bool loopActive;
    }

    public SoundList[] soundLists;
    [Space(10f)]
    public GameObject SoundPrefab;

    IEnumerator SetNextPlayMusic;

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
    // Start is called before the first frame update
    void Start()
    {
        CheckSoundSystem();
    }

    public void CheckSoundSystem()
    {
        if (PlayerPrefs.GetString(AppValueController.Instance.MusicIndexText).Contains("0"))
        {
            MusicSource.enabled = false;
        }
        else
        {
            MusicSource.enabled = true;
        }
    }

    public void PlayMusic(MusicList musicList)
    {
        //MUSIC ACIKSA OYNAT
        if (PlayerPrefs.GetString(AppValueController.Instance.MusicIndexText).Contains("1"))
        {
            MusicIndex = UnityEngine.Random.Range(0, musicLists.Length);

            AudioClip clip = Resources.Load(MusicResourceName + "/" + musicLists[MusicIndex].audioClipName, typeof(AudioClip)) as AudioClip;

            MusicSource.clip = clip;
            MusicSource.volume = musicList.musicVolume;
            MusicSource.Play();

            if (SetNextPlayMusic != null)
            {
                StopCoroutine(SetNextPlayMusic);
                SetNextPlayMusic = null;
            }

            SetNextPlayMusic = SetNextMusic(clip.length);
            StartCoroutine(SetNextPlayMusic);
        }
    }

    IEnumerator SetNextMusic(float time)
    {
        yield return new WaitForSeconds(time);
        NextPlayMusic();
    }

    public void NextPlayMusic()
    {
        //Sonraki Muzik
        if (PlayerPrefs.GetString(AppValueController.Instance.MusicIndexText).Contains("1"))
        {
            //Kullanılmayanı kaldır RAM den
            Resources.UnloadUnusedAssets();
            //Bir sonraki muzik
            MusicIndex++;
            if (musicLists.Length <= MusicIndex)
                MusicIndex = 0;

            AudioClip clip = Resources.Load(MusicResourceName + "/" + musicLists[MusicIndex].audioClipName, typeof(AudioClip)) as AudioClip;

            MusicSource.clip = clip;
            MusicSource.volume = musicLists[MusicIndex].musicVolume;
            MusicSource.Play();


            if (SetNextPlayMusic != null)
            {
                StopCoroutine(SetNextPlayMusic);
                SetNextPlayMusic = null;
            }

            SetNextPlayMusic = SetNextMusic(clip.length);
            StartCoroutine(SetNextPlayMusic);
        }
    }

    public void CreateSoundPrefab(SoundID soundID)
    {
        //Kullanılmayanı kaldır RAM den
        Resources.UnloadUnusedAssets();

        if (PlayerPrefs.GetString(AppValueController.Instance.SoundIndexText).Contains("1"))
        {
            for (int i = 0; i < soundLists.Length; i++)
            {
                if (soundLists[i].soundID == soundID)
                {
                    GameObject temp = Instantiate(SoundPrefab);

                    AudioClip clip = Resources.Load(SoundResourceName + "/" + soundLists[i].audioClipName, typeof(AudioClip)) as AudioClip;

                    //temp.GetComponent<AudioSource>().clip = soundLists[i].audioClip;
                    temp.GetComponent<AudioSource>().clip = clip;
                    temp.GetComponent<AudioSource>().volume = soundLists[i].volume;
                    temp.GetComponent<AudioSource>().loop = soundLists[i].loopActive;

                    temp.GetComponent<AudioSource>().Play();

                    Destroy(temp, clip.length);
                }
            }
        }
    }

    public void MusicBackgroundVolumeSetting(float volume)
    {
        MusicSource.volume = volume;
    }

    public void CheckRadioButton()
    {
        MusicSource.enabled = !MusicSource.enabled;
    }
}
