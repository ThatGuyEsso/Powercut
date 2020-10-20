using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Range(0,360)]
    public float fovAngle=90f;
    public int rayCount=2;
    [HideInInspector]
    public float currentAngle;
    public float viewDistance;

    public LayerMask ViewBlockingLayers;
    private Mesh mesh;
    private MeshRenderer meshRenderer;
    Vector3 origin;
    private float offset;
    float startingAngle;

    private bool lightIsOn;
    public LightSettings settings;
    public LightManager manager;
    private void Awake()
    {
        mesh = new Mesh();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        GetComponent<MeshFilter>().mesh = mesh;
        manager = gameObject.GetComponent<LightManager>();
        origin = Vector3.zero;
        SetUpLight();
    }
    // Start is called before the first frame update
    void Update()
    {


        DrawVisionConeShape();

    }

    public void DrawVisionConeShape()
    {
        Vector3[] vertices = new Vector3[rayCount + 1 + 1];//orgin ray + ray 0
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];//for every ray there should  be triangle and as the triagle has three vertices

        currentAngle = startingAngle +offset;
        float angleIncrease = fovAngle / rayCount;

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D hitInfo = Physics2D.Raycast(origin, EssoUtility.GetVectorFromAngle(currentAngle), viewDistance, ViewBlockingLayers);
            if (hitInfo)
            {
                vertex = hitInfo.point;
            }
            else
            {
                vertex = origin + EssoUtility.GetVectorFromAngle(currentAngle) * viewDistance;
            }
            vertices[vertexIndex] = vertex;
            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;

            }

            vertexIndex++;
            currentAngle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDir)
    {
        startingAngle = EssoUtility.GetAngleFromVector(aimDir) - fovAngle / 2;
    }




    private void SetUpLight()
    {
        fovAngle = settings.lightAngle;
        ViewBlockingLayers = settings.lightBlockingLayers;
        viewDistance = settings.lightRadius;
        lightIsOn = true;
        offset = settings.lightAngle;
        rayCount = settings.rayCount;
    }


    public void ToggleLight(bool isOn)
    {
        lightIsOn = isOn;
        if (lightIsOn)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }


    public bool GetLightIsOn()
    {
        return lightIsOn;
    }

}
