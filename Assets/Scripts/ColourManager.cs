using HSVPicker;
using UnityEngine;

public class ColourManager : MonoBehaviour
{
    public ColorPicker picker;
    public ObjectStateLogger objectStateLogger;
    public GameObject itemEditPanel;
    void Start()
    {
        if (picker == null || objectStateLogger == null)
        {
            Debug.LogError("Resource not set");
        }

        objectStateLogger.OnObjectCleared += CloseColourPicker;
    }

    public void OpenColourPicker()
    {
        picker.gameObject.SetActive(true);
        itemEditPanel.SetActive(false);
        
        GameObject obj = objectStateLogger.GetObject();
        Renderer renderer = obj.GetComponentInChildren<Renderer>();

        //set current colour to picker.
        picker.CurrentColor = renderer.material.color;

        picker.onValueChanged.AddListener(color =>
		{
			renderer.material.color = color;
		});
		renderer.material.color = picker.CurrentColor;
    }

    private void CloseColourPicker()
    {
        picker.onValueChanged.RemoveAllListeners();
        picker.gameObject.SetActive(false);
    }
}
