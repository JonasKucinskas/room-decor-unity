using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public Material lineMaterial;

    void Start()
    {
        DrawGrid();
    }

    public void DrawGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        float cellSizeWidth = PlayerPrefs.GetFloat("GridSizeX");
        float cellSizeLength = PlayerPrefs.GetFloat("GridSizeZ");

        if (cellSizeWidth == 0f || cellSizeLength == 0f)
        {
            //grid off.
            return;
        }

        int widthLines = 0;
        int lengthLines = 0;

        float planeWidth = 0;
        float planeLength = 0;

        float halfWidth = 0;
        float halfLength = 0;

        MeshRenderer planeRenderer = gameObject.GetComponent<MeshRenderer>();
        if (planeRenderer != null)
        {
            // Get the size of the plane from its renderer bounds
            planeWidth = planeRenderer.bounds.size.x;
            planeLength = planeRenderer.bounds.size.z;

            widthLines = Mathf.CeilToInt(planeWidth / cellSizeWidth);
            lengthLines = Mathf.CeilToInt(planeLength / cellSizeLength);

            halfWidth = planeWidth / 2;
            halfLength = planeLength / 2;
        }

        for (int i = 0; i <= widthLines; i++)
        {
            float x = -halfWidth + i * cellSizeWidth;
            Vector3 start = new Vector3(x, 0, -halfLength);
            Vector3 end = new Vector3(x, 0, -halfLength + planeLength);
            DrawLine(start, end);
        }

        // Draw the lines along the length (z-axis)
        for (int i = 0; i <= lengthLines; i++)
        {
            float z = -halfLength + i * cellSizeLength;
            Vector3 start = new Vector3(-halfWidth, 0, z);
            Vector3 end = new Vector3(-halfWidth + planeWidth, 0, z);
            DrawLine(start, end);
        }

    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject lineObject = new GameObject("Line");
        lineObject.transform.parent = this.transform;
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        if (lineMaterial != null)
        {
            lineRenderer.material = lineMaterial;
        }
    }
}
