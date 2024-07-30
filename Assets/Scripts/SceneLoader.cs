using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public GameObject levelSelectPanel;
    public GameObject renameFilePanel;
    public GameObject buttonPrefab;
    private List<GameObject> playButtons;
    private string levelName;
    private string oldFilename;
    void Start()
    {
        if (levelSelectPanel == null || renameFilePanel == null || buttonPrefab == null)
        {
            Debug.Log("Resource not set");
        }
    }

    public void LoadScene(string sceneName)
    {
        //this is called from other scenes
        SceneManager.LoadScene(sceneName);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        
        SceneManager.SetActiveScene(scene);
        Debug.Log(scene.name);
        if (scene.name == "SampleScene" && !String.IsNullOrEmpty(levelName))
        {
            LevelManager.Load(levelName);
            levelName = "";
            oldFilename = "";
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadLevelSelectPanel()
    {
        playButtons = new List<GameObject>();
        
        var info = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] files = info.GetFiles("*.json");

        GameObject deleteButton = GameObject.Find("Canvas/LevelSelectPanel/HeaderPanel/DeleteButton");
        deleteButton.GetComponent<Button>().onClick.RemoveAllListeners();
        deleteButton.GetComponentInChildren<TextMeshProUGUI>().text = "Delete";

        GameObject renameButton = GameObject.Find("Canvas/LevelSelectPanel/HeaderPanel/RenameButton");
        renameButton.GetComponent<Button>().onClick.RemoveAllListeners();
        renameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Rename";

        foreach (var file in files)
        {
            string levelname = Path.GetFileNameWithoutExtension(file.Name);
            GameObject button = Instantiate(buttonPrefab, levelSelectPanel.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = levelname;
            
            Button buttonComponent = button.GetComponentInChildren<Button>();
            buttonComponent.onClick.AddListener(() => SetLevelName(button));
            buttonComponent.onClick.AddListener(() => LoadScene("SampleScene"));

            playButtons.Add(button);
        }
    }

    public void DeleteLevel(GameObject deleteButton)
    {
        deleteButton.GetComponentInChildren<TextMeshProUGUI>().text = "Cancel";
        deleteButton.GetComponent<Button>().onClick.RemoveAllListeners();
        deleteButton.GetComponent<Button>().onClick.AddListener(() => ReloadLevelCanvas());

        foreach (GameObject button in playButtons)
        {   
            //clicking on any level button will delete that level
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            button.GetComponent<Button>().onClick.AddListener(() => Delete(button));
            button.GetComponent<Image>().color = new Color32(255, 120, 120, 255);
        }
    }

    // The actual function that deletes the level
    public void Delete(GameObject button)
    {
        string levelName = button.GetComponentInChildren<TextMeshProUGUI>().text;
        string path = Application.persistentDataPath + "/" + levelName + ".json";
        File.Delete(path);

        playButtons.Remove(button);
        Destroy(button);

        ReloadLevelCanvas();
    }

    private void SetLevelName(GameObject levelButton)
    {
        levelName = levelButton.GetComponentInChildren<TextMeshProUGUI>().text;
    }

    private void ReloadLevelCanvas()
    {
        foreach (GameObject button in playButtons)
        {
            Destroy(button);
        }

        playButtons.Clear();

        LoadLevelSelectPanel();
    }

    public void BackToMain()
    {
        foreach (GameObject button in playButtons)
        {
            Destroy(button);
        }
        playButtons.Clear();
    }

    //Initiates renaming
    public void RenameLevelInit(GameObject renameButton)
    {
        renameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Cancel";
        renameButton.GetComponent<Button>().onClick.RemoveAllListeners();
        renameButton.GetComponent<Button>().onClick.AddListener(() => ReloadLevelCanvas());

        foreach (GameObject button in playButtons)
        {
            Button buttonComponent = button.GetComponent<Button>();
            string buttonText = buttonComponent.GetComponentInChildren<TextMeshProUGUI>().text;
            buttonComponent.onClick.RemoveAllListeners();
            buttonComponent.onClick.AddListener(() => OpenRenamePanel(buttonText));
            button.GetComponent<Image>().color = new Color32(120, 120, 255, 255);
        }
    }

    private void OpenRenamePanel(string buttontext)
    {
        oldFilename = buttontext;
        renameFilePanel.SetActive(true);
    }

    public void RenameFile()
    {
        string filename = renameFilePanel.GetComponentInChildren<TMP_InputField>().text;
        string filenameEdited = Regex.Replace(filename, "[^a-zA-Z0-9 ]", "");;
        
        if (String.IsNullOrEmpty(filenameEdited))
        {
            Debug.Log("Save file name not entered.");
            return;
        }
        
        //rename
        string oldPath = $"{Application.persistentDataPath}/{oldFilename}.json";
        string newPath = $"{Application.persistentDataPath}/{filenameEdited}.json";
        File.Move(oldPath, newPath);

        GameObject button = playButtons.Find(x => x.GetComponentInChildren<TextMeshProUGUI>().text == oldFilename);
        button.GetComponentInChildren<TextMeshProUGUI>().text = filenameEdited;
        
        renameFilePanel.SetActive(false);
        ReloadLevelCanvas();
    }
}