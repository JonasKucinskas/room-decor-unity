using System;
using System.Reflection;
using TMPro;
using UnityEngine;

public class TranformEditPanel : MonoBehaviour
{
    public TMP_InputField inputField1;
    public TMP_InputField inputField2;
    public TMP_InputField inputField3;

    private TMP_InputField[] fields;
    public ObjectStateLogger objectStateLogger;
    private Transform editedGameObject;
    private bool manualEntering;

    [HideInInspector]
    public string propertyName;
    private PropertyInfo property;
    void Awake()
    {
        if (inputField1 == null || inputField2 == null || 
            inputField3 == null || objectStateLogger == null)
        {
            Debug.LogError("Resources not set");
            return;
        }
        
        fields = new TMP_InputField[] { inputField1, inputField2, inputField3 };
        objectStateLogger.OnObjectCleared += CloseTransformEditPanel;
    }

    void Update()
    {
        //set current object values to input fields.
        if (objectStateLogger.IsEditing() && !manualEntering)
        {
            var propertyValue = property.GetValue(editedGameObject);

            fields[0].text = propertyValue.GetType().GetField("x").GetValue(propertyValue).ToString();
            fields[1].text = propertyValue.GetType().GetField("y").GetValue(propertyValue).ToString();
            fields[2].text = propertyValue.GetType().GetField("z").GetValue(propertyValue).ToString();
        }
    }

    public void SetValue(int inputFieldIndex)
    {
        //this supports any properties that have x,y,z fields.
        TMP_InputField field = fields[inputFieldIndex - 1];
        string text = field.text;

        if (String.IsNullOrEmpty(text))
        {
            return;
        }

        float floatValue;
        bool succeded = float.TryParse(text, out floatValue);

        if (!succeded)
        {
            Debug.LogError("failed to parse value");
            return;
        }

        

        //get transform property
        var property = editedGameObject.GetType().GetProperty(propertyName);

        //get property's value, change it according to needs, and set a new one.
        var currentValue = property.GetValue(editedGameObject);
        var newValue = currentValue;

        //values are always vector3
        var valueType = typeof(Vector3);

        switch(inputFieldIndex)
        {
            case 1:
                valueType.GetField("x").SetValue(newValue, floatValue);
                break;
            case 2:
                valueType.GetField("y").SetValue(newValue, floatValue);
                break;
            case 3:
                valueType.GetField("z").SetValue(newValue, floatValue);
                break;
            default:
                return;
        }


        property.SetValue(editedGameObject.transform, newValue);
        
        //manual entering finished.
        manualEntering = false;
    }

    private void OnEnable()
    {
        //editedGameObject only changes when this gameobject is enabled.
        editedGameObject = objectStateLogger.GetObject().transform;
        property = editedGameObject.GetType().GetProperty(propertyName);
    }

    public void OnInputFieldSelect()
    {
        manualEntering = true;
    }

    public void CloseTransformEditPanel()
    {
        gameObject.SetActive(false);
    }
}