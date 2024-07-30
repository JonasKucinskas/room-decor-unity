using TMPro;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public TMP_Dropdown snapDropdown;
    public GameObject optionPanel;
    public TMP_Text statusText;
    public GridGenerator gridGenerator;
    void Start()
    {
        if (snapDropdown == null || optionPanel == null || statusText == null)
        {
            Debug.LogError("Resources not set");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPrefs()
    {
        int index = snapDropdown.value;
        PlayerPrefs.SetString("Snap", snapDropdown.options[index].text);

        TMP_InputField[] components = optionPanel.GetComponentsInChildren<TMP_InputField>();
        
        //if fails to parse, float values will be 0, which is ok
        float.TryParse(components[0].text, out float gridSizeX);
        float.TryParse(components[1].text, out float gridSizeY);
        float.TryParse(components[2].text, out float gridSizeZ);

        PlayerPrefs.SetFloat("GridSizeX", gridSizeX);
        PlayerPrefs.SetFloat("GridSizeY", gridSizeY);
        PlayerPrefs.SetFloat("GridSizeZ", gridSizeZ);

        float.TryParse(components[3].text, out float camSpeed);
        float.TryParse(components[4].text, out float camSens);

        if (camSpeed == 0)
        {
            camSpeed = 0.5f;
        }
        if (camSens == 0)
        {
            camSens = 0.5f;
        }
        
        PlayerPrefs.SetFloat("camSpeed", camSpeed);
        PlayerPrefs.SetFloat("camSens", camSens);

        statusText.SetText("Settings saved!");
        
        if (gridGenerator != null)
        {
            gridGenerator.DrawGrid();
        }
    }

    public void LoadPrefs()
    {
        int index = snapDropdown.options.FindIndex(option => option.text == PlayerPrefs.GetString("Snap"));
        snapDropdown.value = index;


        TMP_InputField[] components = optionPanel.GetComponentsInChildren<TMP_InputField>();

        components[0].text = PlayerPrefs.GetFloat("GridSizeX").ToString();
        components[1].text = PlayerPrefs.GetFloat("GridSizeY").ToString();
        components[2].text = PlayerPrefs.GetFloat("GridSizeZ").ToString();
        
        float camSpeed = PlayerPrefs.GetFloat("camSpeed");
        float camSens = PlayerPrefs.GetFloat("camSens");
        components[3].text = camSpeed.ToString();
        components[4].text = camSens.ToString();
    }
}
