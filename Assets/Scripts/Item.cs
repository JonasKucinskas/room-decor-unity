using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector]
    public bool moving = false;
    private Outline outline;
    ObjectStateLogger objectStateLogger;
    GameObject ItemEditPanel;

    void Start()
    {
        objectStateLogger = GameObject.Find("ObjectStateLogger").GetComponent<ObjectStateLogger>();
        ItemEditPanel = GameObject.Find("Canvas/Panels/ItemEditPanel");
        objectStateLogger.OnObjectCleared += ClearEditingState;
    }

    void Update()
    {
        if (moving)
        {
            ObjectFollowsMouse();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //only allow left click, dont allow when item has just spawned or when the game is paused.
        if (eventData.button != PointerEventData.InputButton.Left || Time.timeScale == 0)
        {
            return;
        }

        if (moving)
        {
            objectStateLogger.ClearEditingState();
            return;
        }
        
        //set header text
        TextMeshProUGUI headerText = GameObject.Find("Canvas/Panels/ItemEditPanel/HeaderPanel/ObjNameText")
        .GetComponent<TextMeshProUGUI>();
        headerText.SetText(gameObject.name);

        bool isItemPanelActive = ItemEditPanel.activeSelf;
        
        //handles are drawn, ignore click.
        if (objectStateLogger.IsEditing() && !isItemPanelActive)
        {
            return;
        }

        if (isItemPanelActive)
        {
            objectStateLogger.ClearEditingState();
        }
        else
        {   
            ItemEditPanel.SetActive(true);
            //set current item that is being edited.
            objectStateLogger.AssignObject(gameObject);

            outline = gameObject.GetComponent<Outline>();

            if (outline == null)
            {
                outline = gameObject.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineColor = Color.yellow;
                outline.OutlineWidth = 5f;
            }
            else
            {
                outline.enabled = true;
            }
        }
        
    }

    private void ClearEditingState()
    {
        ItemEditPanel.SetActive(false);
        moving = false;
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    private void ObjectFollowsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Transform parent = hit.collider.gameObject.transform.parent;
            if (parent != null && parent.gameObject == gameObject)
            {
                return;
            }

            Renderer topRenderer = gameObject.GetComponentInChildren<Renderer>();
            Renderer baseRenderer = hit.collider.gameObject.GetComponentInChildren<Renderer>();
            
            float baseHeight = baseRenderer.bounds.size.y / 2;
            float topHeight = topRenderer.bounds.size.y / 2;
            
            Vector3 newPosition = new Vector3(
                hit.point.x, 
                hit.point.y + baseHeight + topHeight, 
                hit.point.z
            );
            
            transform.position = newPosition;
        }
    }
}
