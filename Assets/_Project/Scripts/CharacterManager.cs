using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    public enum CharacterID
    {
        None,
        Char1,
        Char2,
        Char3,
        Char4,
        Char5,
        Char6,
        Char7,
        Enemy1
    }

    [Header("Character Data Scriptable List")]
    public List<CharacterDataScriptable> CharacterDataScriptable = new List<CharacterDataScriptable>();

    [HideInInspector]
    public CharacterDataScriptable SelectedCharacter;
    private int CharacterIndex = 0;

    [Header("Selected Enemy")]
    public CharacterDataScriptable SelectedEnemy;

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

        //Get All Character Data
        StartCoroutine(LoadCharacterData());
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    IEnumerator LoadCharacterData()
    {
        AppValueController.Instance.DebugLog("LoadCharacterData", Color.green);

        //Load Default Vehicle Datas
        CharacterDataScriptable[] tempCharacterDataScriptable = Resources.LoadAll<CharacterDataScriptable>("ScriptableData/CharacterDatas") as CharacterDataScriptable[];

        CharacterDataScriptable = new List<CharacterDataScriptable>();

        for (int i = 0; i < tempCharacterDataScriptable.Length; i++)
        {
            CharacterDataScriptable.Add(tempCharacterDataScriptable[i]);
        }

        //Get First Char
        if (SelectedCharacter == null)
            SelectedCharacter = CharacterDataScriptable[0];

        yield return null;
    }

    public CharacterDataScriptable GetCharacterData()
    {
        return SelectedCharacter;
    }

    public void SetCharacterData(int _index)
    {
        AppValueController.Instance.DebugLog("SetCharacterData", Color.green);

        CharacterIndex += _index;

        if (CharacterIndex < 0)
        {
            CharacterIndex = CharacterDataScriptable.Count - 1;
        }
        if (CharacterIndex >= CharacterDataScriptable.Count)
        {
            CharacterIndex = 0;
        }

        SelectedCharacter = CharacterDataScriptable[CharacterIndex];
    }

    public CharacterDataScriptable GetEnemyData()
    {
        return SelectedEnemy;
    }
}
