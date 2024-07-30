using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject Panel;
    private GameObject instantiatedObject = null;
    ObjectStateLogger objectStateLogger;

    void Start()
    {
        objectStateLogger = GameObject.Find("ObjectStateLogger").GetComponent<ObjectStateLogger>();
    }
    public void SpawnPrefab(GameObject prefabToSpawn)
    {
        if (prefabToSpawn != null && !objectStateLogger.IsEditing()) 
        {
            GameObject parent = GameObject.Find("Objects");
            
            instantiatedObject = Instantiate(prefabToSpawn, Panel.transform.position, Quaternion.identity);
            instantiatedObject.transform.parent = parent.transform;
            
            Item item = instantiatedObject.AddComponent<Item>();
            objectStateLogger.AssignObject(prefabToSpawn);
            item.moving = true;
        }
    }
}
