using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSpawnPanel : MonoBehaviour
{
    public GameObject Panel;
    public GameObject prefabList;
    private ObjectSpawner objectSpawner;
    private ObjectStateLogger objectStateLogger;

    void Start()
    {
        objectSpawner = GameObject.Find("ObjectSpawner").GetComponent<ObjectSpawner>();
        objectStateLogger = GameObject.Find("ObjectStateLogger").GetComponent<ObjectStateLogger>();
        
        if (objectSpawner == null || objectStateLogger == null)
        {
            Debug.LogError("Resource not set");
        }

        objectStateLogger.OnObjectCleared += DeactivatePanel;
    }

    public void TogglePanel(){

        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;
                
            if (!isActive)
            {
                Panel.SetActive(true);
                DrawPrefabPreviews();
            }
            else 
            {
                DeactivatePanel();
            }
        }
    }

    private void DrawPrefabPreviews()
    {
        GameObject template = Resources.Load("Prefabs/PrefabPreview") as GameObject;
        IEnumerable<GameObject> objects = Resources.LoadAll($"Prefabs/Spawnable", typeof(GameObject)).Cast<GameObject>();

        StartCoroutine(DrawPrefabPreviewsAsync(objects, template));
    }

    private IEnumerator DrawPrefabPreviewsAsync(IEnumerable<GameObject> objects, GameObject template)
    {
        foreach (GameObject prefab in objects)
        {
            yield return StartCoroutine(SetPrefabPreview(prefab, template));
        }
    }

    private IEnumerator SetPrefabPreview(GameObject prefab, GameObject template)
    {
        if (prefab.GetType() != typeof(GameObject))
        {
            yield break;
        }

        Texture2D pic = null;
        yield return StartCoroutine(GetAssetPreview(prefab, result => pic = result));

        if (pic != null)
        {
            GameObject preview = Instantiate(template, prefabList.transform);
            TextMeshProUGUI itemNameText = preview.GetComponentInChildren<TextMeshProUGUI>();
            Image previewImage = preview.GetComponentInChildren<Image>();
            Button previewButton = preview.GetComponentInChildren<Button>(); 

            itemNameText.SetText(prefab.name);
            previewImage.sprite = Sprite.Create(pic, new Rect(0, 0, pic.width, pic.height), Vector2.zero);
        
            previewButton.onClick.AddListener(() => OnPrefabPreviewClick(prefab));
        }
    }

    private IEnumerator GetAssetPreview(GameObject prefab, Action<Texture2D> callback)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is null.");
            callback(null);
            yield break;
        }

        Texture2D previewTexture = RuntimePreviewGenerator.GenerateModelPreview(prefab.transform, width: 256, height: 256);

        callback(previewTexture);
    }

    public void DeactivatePanel()
    {
        Panel.SetActive(false);
        //if panel is being closed, destroy drawn objects.
        foreach (Transform child in prefabList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnPrefabPreviewClick(GameObject prefab)
    {
        if (objectSpawner != null)
        {
            objectSpawner.SpawnPrefab(prefab);
            DeactivatePanel();
        }
        else
        {
            Debug.LogError("ObjectSpawner not found");
        }
    }
}