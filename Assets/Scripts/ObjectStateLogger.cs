using RuntimeHandle;
using UnityEngine;

public class ObjectStateLogger : MonoBehaviour
{
    private bool isObjectBeingEdited;
    private GameObject objectBeingEdited;
    public delegate void ObjectClearedHandler();
    public event ObjectClearedHandler OnObjectCleared;

    void Start()
    {
        ClearEditingState();
    }

    void Update()
    {
        if (IsEditing() && Input.GetKeyDown(KeyCode.Return))
        {
            ClearEditingState();
        }
    }

    public void ClearEditingState() 
    {
        isObjectBeingEdited = false;
        objectBeingEdited = null;
        OnObjectCleared?.Invoke();
    }

    public void AssignObject(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("Can't assign to null.");
            return;
        }
        objectBeingEdited = obj;
        isObjectBeingEdited = true;
    }
    
    public bool IsEditing()
    { 
        return isObjectBeingEdited;
    }

    public GameObject GetObject()
    {
        return objectBeingEdited;
    }
}