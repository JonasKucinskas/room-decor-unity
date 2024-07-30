using RuntimeHandle;
using UnityEngine;

//ui handles for moving, scale etc.
public class HandlesManager : MonoBehaviour
{
    [HideInInspector]
    private RuntimeTransformHandle transformHandle;
    public ObjectStateLogger objectStateLogger;
    public GameObject itemEditPanel;
    public GameObject tranformEditPanel;
    void Start()
    {
        if (itemEditPanel == null || objectStateLogger == null 
        || tranformEditPanel == null)
        {
            Debug.LogError("Resource not set");
        }

        objectStateLogger.OnObjectCleared += clearState;
    }

    private void clearState()
    {
        if (transformHandle != null)
        {
            Destroy(transformHandle.gameObject);
        }

        tranformEditPanel.SetActive(false);
        itemEditPanel.SetActive(false);
    }

    /// <summary>
    /// Adds handle to selected object in scene
    /// </summary>
    /// <param name="handleType">Defines type of handle: 1 - scale, 2 - move, 3 - Rotate</param>
    public void AddHandle(int handleType)
    {
        GameObject obj = objectStateLogger.GetObject();

        TranformEditPanel transformPanel = tranformEditPanel.GetComponent<TranformEditPanel>();

        //objectStateLogger editing state is set when the edit panel is opened.
        HandleType type;
        switch (handleType)
        {
            case 1: // scale
                type = HandleType.SCALE;
                transformPanel.propertyName = "localScale";
                break;
            case 2: // move 
                type = HandleType.POSITION;
                transformPanel.propertyName = "position";
                break;
            case 3: // rotate
                type = HandleType.ROTATION;
                transformPanel.propertyName = "eulerAngles";
                break;
            case 4: // delete
                objectStateLogger.ClearEditingState();
                Destroy(obj);
                itemEditPanel.SetActive(false);
                return;
            default: // unknown
                return;
        }
        tranformEditPanel.SetActive(true);
        itemEditPanel.SetActive(false);

        transformHandle = RuntimeTransformHandle.Create(obj.transform, type);
        

        if (handleType == 2)
        {
            string snap = PlayerPrefs.GetString("Snap");
            if (snap == "To Grid")
            {
                float gridSizeX = PlayerPrefs.GetFloat("GridSizeX");
                float gridSizeY = PlayerPrefs.GetFloat("GridSizeY");
                float gridSizeZ = PlayerPrefs.GetFloat("GridSizeZ");

                transformHandle.positionSnap = new Vector3(gridSizeX, gridSizeY, gridSizeZ);
            }
        }

        transformHandle.space = HandleSpace.WORLD;
        transformHandle.transform.position = obj.transform.position;
        transformHandle.autoScale = true;

    }
}