using TMPro;
using UnityEngine;

public class EscPanelManager : MonoBehaviour
{
    public GameObject escPanel;
    public GameObject optionsPanel;
    public GameObject objectSpawnButton; 
    public GameObject GridSizeOption;
    public TMP_Dropdown snapDropdown;
    public GameObject savePanel;
    public ObjectStateLogger objectStateLogger;

    void Start()
    {
        if (escPanel == null || objectSpawnButton == null || 
            optionsPanel == null || savePanel == null || objectStateLogger == null)
        {
            Debug.LogError("Resource not set.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !savePanel.activeSelf && !optionsPanel.activeSelf)
        {
            bool isActive = escPanel.activeSelf;

            if(!isActive)
            {
                objectStateLogger.ClearEditingState();
                OpenEscPanel();
            }
            else 
            {
                CloseEscPanel();
            }
        }
    }

    public void OpenEscPanel()
    {
        escPanel.SetActive(true);
        objectSpawnButton.SetActive(false);
        Time.timeScale = 0f;
    }

    public void CloseEscPanel()
    {
        escPanel.SetActive(false);
        objectSpawnButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void OpenOptionsPanel()
    {
        optionsPanel.SetActive(true);
        GetComponent<PlayerPrefsManager>().LoadPrefs();
    }
}