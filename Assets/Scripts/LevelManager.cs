using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public TMP_InputField saveTextField;
    public TMP_Text statusText;
    private string saveFileName = "";

    public void Start()
    {
        if (saveTextField == null || statusText == null)
        {
            Debug.LogError("Resources not set");
        }
    }

    private void CheckOverwrite(string text)
    {
        Debug.Log("Save file name: " + saveFileName);
        PlayerPrefs.SetString("saveFileName", saveFileName);


        if (string.IsNullOrEmpty(saveFileName))
        {
            Debug.LogError("Save file name not entered.");
            return;
        }

        // Check if file already exists
        if (File.Exists(Application.persistentDataPath + "/" + saveFileName + ".json"))
        {
        }
        else
        {
            Save();
        }
    }

    public void CancelOverwrite()
    {
    }

    public void Save()
    {
        GameObject objects = GameObject.Find("Objects");

        int childCount = objects.transform.childCount;
        
        List<ItemSerializable> items = new List<ItemSerializable>(); 

        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = objects.transform.GetChild(i);
            ItemSerializable item = new ItemSerializable(childTransform.gameObject);
            items.Add(item);
        }

        string filename = Regex.Replace(saveTextField.text, "[^a-zA-Z0-9 ]", "");;

        if (String.IsNullOrEmpty(filename))
        {
            statusText.SetText("No name");
            return;
        }

        string path = $"{Application.persistentDataPath}/{filename}.json";

        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };


        try
        {
            string json = JsonConvert.SerializeObject(items, Formatting.Indented, settings);
            File.WriteAllText(path, json);
            statusText.SetText("Saved!");
            Debug.Log("Saved to: " + path);

        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            statusText.SetText("Error");
        }
        
        items.Clear();
    }

    public static void Load(string filename)
    {
        string path = $"{Application.persistentDataPath}/{filename}.json";

        if (!File.Exists(path))
        {
            Debug.LogError("Save file with this name does not exist.");
            return;
        }

        string JsonData = File.ReadAllText(path);
        List<ItemSerializable> items = JsonConvert.DeserializeObject<List<ItemSerializable>>(JsonData);
        
        GameObject objectsGO = GameObject.Find("Objects");

        foreach (ItemSerializable obj in items)
        {
            var prefab = Resources.Load(obj.path) as GameObject;
            GameObject instObj = Instantiate(prefab, objectsGO.transform);

            instObj.transform.position = new Vector3(obj.position.x, obj.position.y, obj.position.z);
            instObj.transform.rotation = Quaternion.Euler(obj.rotation.x, obj.rotation.y, obj.rotation.z);
            instObj.transform.localScale = new Vector3(obj.scale.x, obj.scale.y, obj.scale.z);;

            Renderer renderer = instObj.GetComponentInChildren<Renderer>();
            renderer.material.color = new Color(obj.color.x, obj.color.y, obj.color.z);;

            
            instObj.AddComponent<Item>();
        }
    }
}
