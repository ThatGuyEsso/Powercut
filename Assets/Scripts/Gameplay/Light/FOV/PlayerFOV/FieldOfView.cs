using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour, ILightWeakness
{
    //Light Shape settings
    [Range(0,360)]
    protected float fovAngle=90f;
    protected int rayCount=2;
    [HideInInspector]
    protected float currentAngle;
    protected float viewDistance;
    protected LayerMask ViewBlockingLayers;
    protected LayerMask enemyLayer;

    //Mesh Settings
    [Header("Mesh Settings ")]
    protected Mesh mesh;
    protected MeshRenderer meshRenderer;
    protected Vector3 origin;

    //Store variable of angle to draw light at
    protected float offset;
    protected float startingAngle;

    //Wether lamp is on
    protected bool lightIsOn;

    //External Manager
    private LightManager manager;


    [Header("Light Shape settings")]
    public LightSettings settings;
   virtual protected void Awake()
   {
        //Initialise mesh
        mesh = new Mesh();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        GetComponent<MeshFilter>().mesh = mesh;
        //Cache manager
        manager = gameObject.GetComponent<LightManager>();

        //Initialise light variables
        SetUpLight();
   }

    virtual protected void LateUpdate()
    {
        UpdateConeView();
    }

    public void UpdateConeView()
    {
        //Every frame update the cone shape
        if (lightIsOn)
        {
            DrawVisionConeShape();
            //WeakenEnemy();
        }
    }
    virtual protected void DrawVisionConeShape()
    {
        //Create then number of total vertices required
        Vector3[] vertices = new Vector3[rayCount + 1 + 1];//orgin ray + ray 0

        //Create UVs to all vertices
        Vector2[] uv = new Vector2[vertices.Length];

        //D
        int[] triangles = new int[rayCount * 3];//for every ray there should  be triangle and as the triangle has three vertices ray*3

        currentAngle = startingAngle +offset;//Adds offset to angle player straight on
        float angleIncrease = fovAngle / rayCount;//The incremenent for each angle

        vertices[0] = origin;// first vertex should alwaus be at the index

        int vertexIndex = 1;//start at index one  (second element)
        int triangleIndex = 0;
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 vertex;
            //Using utility function to convert a angle into a vector
            RaycastHit2D hitInfo = Physics2D.Raycast(origin, EssoUtility.GetVectorFromAngle(currentAngle), viewDistance, ViewBlockingLayers);
            if (hitInfo)
            {
                //If it hits something the current vertex position = point
                vertex = hitInfo.point;
            }
            else
            {
                //If not just draw full length of ray in current angle
                vertex = origin + EssoUtility.GetVectorFromAngle(currentAngle) * viewDistance;
            }
            vertices[vertexIndex] = vertex;
            if (i > 0)//Do not do it for the first element as it is the origin
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;

            }

            vertexIndex++;
            currentAngle -= angleIncrease;
        }

        //Update mesh 
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);//Sets bounds to size of map
    }

    //Used to update the current origin of the mesh as character can move
    virtual public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }
    //Updates angle to draw from as character can turn
    virtual public void SetAimDirection(Vector3 aimDir)
    {
        startingAngle = EssoUtility.GetAngleFromVector(aimDir) - fovAngle / 2;
    }




    //Initialises variables by assigning them their approriate values
    virtual protected void SetUpLight()
    {
        origin = Vector3.zero;
        fovAngle = settings.lightAngle;
        ViewBlockingLayers = settings.lightBlockingLayers;
        enemyLayer = settings.enemyLayer;
        viewDistance = settings.lightRadius;
        lightIsOn = true;
        offset = settings.lightAngle;
        rayCount = settings.rayCount;
    }

    //On true turns light on, on false disables light
    virtual public void ToggleLight(bool isOn)
    {
        lightIsOn = isOn;
        if (lightIsOn)
        {
            if(meshRenderer)
                meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
  
    }

    //Returns value of LightIsOn
    virtual public bool GetLightIsOn()
    {
        return lightIsOn;
    }

    //Returns current manager of class
    virtual public LightManager GetLightManager()
    {
        return manager;
    }

    void ILightWeakness.MakeVulnerable()
    {
        //Blank just to allow it to interface with enemies
    }


    protected void WeakenEnemy()
    {
        currentAngle = startingAngle + offset;//Adds offset to angle player straight on
        float angleIncrease = fovAngle / rayCount;//The incremenent for each angle

        for (int i = 0; i < rayCount; i++)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(origin, EssoUtility.GetVectorFromAngle(currentAngle), viewDistance, enemyLayer);

            if (hitInfo)
            {
                if(hitInfo.transform.GetComponent<ILightWeakness>() != null)
                {
                    hitInfo.transform.GetComponent<ILightWeakness>().MakeVulnerable();
                }
                Debug.DrawRay(origin, hitInfo.point);
            }
            else
            {
                Debug.DrawRay(origin, EssoUtility.GetVectorFromAngle(currentAngle) * viewDistance);
            }
            currentAngle -= angleIncrease;
        }
    }
}
