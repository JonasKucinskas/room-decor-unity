using UnityEngine;

[System.Serializable]   
public class ItemSerializable
{
    [SerializeField] public string name;
    [SerializeField] public string path;
    [SerializeField] public TripleValueSerializable color;
    [SerializeField] public TripleValueSerializable position;
    [SerializeField] public TripleValueSerializable rotation;
    [SerializeField] public TripleValueSerializable scale;
    
    public ItemSerializable()
    {
    }

    public ItemSerializable(GameObject obj)
    {
        this.name = obj.name.Replace("(Clone)", "");
        this.path = $"Prefabs/Spawnable/{this.name}";

        Renderer renderer = obj.GetComponentInChildren<Renderer>();
        Transform transform = obj.transform;
        this.color = new TripleValueSerializable(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b);
        this.position = new TripleValueSerializable(transform.position.x, transform.position.y, transform.position.z);
        this.rotation = new TripleValueSerializable(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);;
        this.scale = new TripleValueSerializable(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}

public class TripleValueSerializable
{
    //Unity classes cause problems when trying to serialize them,
    //this simplifies things.
    public float x;
    public float y;
    public float z;

    public TripleValueSerializable(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}
