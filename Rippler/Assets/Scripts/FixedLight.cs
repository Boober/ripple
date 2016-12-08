using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FixedLight : MonoBehaviour {

    private bool isBox; //Checking whether the bounds are a box, or whether it should be a circle.
    public bool isOn; //Checks whether the light is on. If the light is off, we don't need to create the mesh!
    private bool isCreated; //Checks whether the light mesh is already created so that it doesn't have to be generated every frame.
    private int numOfPoints = 20;

    public Material lightMaterial;
    private Mesh lightMesh; //The Mesh to cut through the shadows.
    private MeshFilter mFilter;
    private MeshRenderer mrenderer;

    private Collider2D bounds; //The bounds of this light, whether it is a circle or box collider.

    void Start () {
        bounds = GetComponent<Collider2D>();
        isBox = (bounds is BoxCollider2D) ? true : false; //Checks whether the Collider connected to the light source is a box or not.
        mFilter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));              // Add a Mesh Filter component to the light game object so it can take on a form
        mrenderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;      // Add a Mesh Renderer component to the light game object so the form can become visible
                                                                                        //gameObject.name = "2DLight";
                                                                                        //renderer.material.shader = Shader.Find ("Transparent/Diffuse");							// Find the specified type of material shader
        mrenderer.sharedMaterial = lightMaterial;                                                       // Add this texture
        lightMesh = new Mesh();                                                                 // create a new mesh for our light mesh
        mFilter.mesh = lightMesh;                                                           // Set this newly created mesh to the mesh filter
        lightMesh.name = "Light Mesh";                                                          // Give it a name
        lightMesh.MarkDynamic();
        mrenderer.sortingLayerName = "Coverage";
        mrenderer.sortingOrder = 0;
        isOn = true; //TODO: Possibly connect this with a lightswitch.
        isCreated = false;

    }
	
	// Update is called once per frame
	void Update () {
        if (!isCreated)
        {
            renderLightMesh();
            resetBounds();
        }
    }

    void renderLightMesh()
    {
        lightMesh.Clear();
        if (isOn)
        {
            if (isBox)
            {
                MakeRectangle();
            }
            else MakeCircle(numOfPoints);
        }
        /*
        if (isBox)
        {
                BoxCollider2D box = (BoxCollider2D)bounds;
                float w = box.bounds.size.x / 2;
                float h = box.bounds.size.y / 2;
                Debug.Log(w);
                Debug.Log(h);
                lightMesh.vertices = new Vector3[] {
                     new Vector3(-w, -h, 0.01f),
                     new Vector3(w, -h, 0.01f),
                     new Vector3(w, h, 0.01f),
                     new Vector3(-w, h, 0.01f)
                 };
                lightMesh.uv = new Vector2[] {
                     new Vector2 (0, 0),
                     new Vector2 (0, 1),
                     new Vector2(1, 1),
                     new Vector2 (1, 0)
                 };
                lightMesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
                lightMesh.RecalculateNormals();
                Debug.Log("Got here. So why is the light mesh not showing up?");
                GetComponent<Renderer>().sharedMaterial = lightMaterial;
        } else
        { //It must be a circle!
            float angleStep = 360.0f / (float)numOfPoints;
            List<Vector3> vertexList = new List<Vector3>();
            List<int> triangleList = new List<int>();
            Quaternion quaternion = Quaternion.Euler(0.0f, 0.0f, angleStep);
            // Make first triangle.
            vertexList.Add(new Vector3(0.0f, 0.0f, 0.0f));  // 1. Circle center.
            vertexList.Add(new Vector3(0.0f, 0.5f, 0.0f));  // 2. First vertex on circle outline (radius = 0.5f)
            vertexList.Add(quaternion * vertexList[1]);     // 3. First vertex on circle outline rotated by angle)
                                                            // Add triangle indices.
            triangleList.Add(0);
            triangleList.Add(1);
            triangleList.Add(2);
            for (int i = 0; i < numOfPoints - 1; i++)
            {
                triangleList.Add(0);                      // Index of circle center.
                triangleList.Add(vertexList.Count - 1);
                triangleList.Add(vertexList.Count);
                vertexList.Add(quaternion * vertexList[vertexList.Count - 1]);
            }
            
            lightMesh.vertices = vertexList.ToArray();
            lightMesh.triangles = triangleList.ToArray();
            lightMesh.RecalculateNormals();
        }
        */
    }

    void resetBounds()
    {
        Bounds b = lightMesh.bounds;
        b.center = Vector3.zero;
        lightMesh.bounds = b;
        isCreated = true;
    }

    private void MakeRectangle()
    {
        BoxCollider2D box = (BoxCollider2D)bounds;
        float w = box.bounds.size.x / 2;
        float h = box.bounds.size.y / 2;
        Debug.Log(w);
        Debug.Log(h);
        lightMesh.vertices = new Vector3[] {
                     new Vector3(-w, -h, 0.01f),
                     new Vector3(w, -h, 0.01f),
                     new Vector3(w, h, 0.01f),
                     new Vector3(-w, h, 0.01f)
                 };
        lightMesh.uv = new Vector2[] {
                     new Vector2 (0, 0),
                     new Vector2 (0, 1),
                     new Vector2(1, 1),
                     new Vector2 (1, 0)
                 };
        lightMesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        lightMesh.RecalculateNormals();
        Debug.Log("Got here. So why is the light mesh not showing up?");
        GetComponent<Renderer>().sharedMaterial = lightMaterial;
    }

    public void MakeCircle(int numOfPoints)
    {
        CircleCollider2D circ = (CircleCollider2D)bounds;
        float radius = circ.radius;
        float angleStep = 360.0f / (float)numOfPoints;
        List<Vector3> vertexList = new List<Vector3>();
        List<int> triangleList = new List<int>();
        Quaternion quaternion = Quaternion.Euler(0.0f, 0.0f, angleStep);
        // Make first triangle.
        vertexList.Add(new Vector3(0.0f, 0.0f, 0.0f));  // 1. Circle center.
        vertexList.Add(new Vector3(0.0f, radius, 0.0f));  // 2. First vertex on circle outline (radius = 0.5f)
        vertexList.Add(quaternion * vertexList[1]);     // 3. First vertex on circle outline rotated by angle)
                                                        // Add triangle indices.
        triangleList.Add(0);
        triangleList.Add(1);
        triangleList.Add(2);
        for (int i = 0; i < numOfPoints - 1; i++)
        {
            triangleList.Add(0);                      // Index of circle center.
            triangleList.Add(vertexList.Count - 1);
            triangleList.Add(vertexList.Count);
            vertexList.Add(quaternion * vertexList[vertexList.Count - 1]);
        }
        lightMesh.vertices = vertexList.ToArray();
        lightMesh.triangles = triangleList.ToArray();
        lightMesh.RecalculateNormals();
    }
}
