using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashZone : FieldOfView,IHurtable
{
    PolygonCollider2D polyCollider;
    override protected void Awake()
    {
        //Initialise mesh
        mesh = new Mesh();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        GetComponent<MeshFilter>().mesh = mesh;
        polyCollider = GetComponent<PolygonCollider2D>();
        SetUpLight();
        lightIsOn = false;
    }

    void IHurtable.Damage(float damage, Vector3 knockBackDir, float knockBack)
    {

    }

    override protected void Update()
    {

        base.Update();
    }


    public void Flash(Vector3 position, Vector3 dir)
    {
        ToggleLight(true);
        SetAimDirection(dir);
        SetOrigin(position);
        lightIsOn = true;
        polyCollider.enabled = true;

        Invoke("StopFlash", 0.25f);
    }

    protected override void DrawVisionConeShape()
    {
        //Create then number of total vertices required
        Vector3[] vertices = new Vector3[rayCount + 1 + 1];//orgin ray + ray 0

        //Create UVs to all vertices
        Vector2[] uv = new Vector2[vertices.Length];

        //D
        int[] triangles = new int[rayCount * 3];//for every ray there should  be triangle and as the triangle has three vertices ray*3

        currentAngle = startingAngle + offset;//Adds offset to angle player straight on
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
        polyCollider.CreatePrimitive(rayCount);

        Vector2[] pointsIn2D = new Vector2[vertices.Length];

        for(int i =0; i < pointsIn2D.Length; i++)
        {
            pointsIn2D[i] = /*(Vector2)origin + */(Vector2)vertices[i];
        }
        pointsIn2D[0]  = origin;
        pointsIn2D[pointsIn2D.Length-1] = origin;
        polyCollider.SetPath(0, pointsIn2D);

    }


    private void StopFlash()
    {
        ToggleLight(false);
        polyCollider.enabled = false;
        Debug.LogError("WAit");
        lightIsOn = false;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 dir = transform.position -collision.transform.position;
            collision.gameObject.GetComponent<IHurtable>().Damage(200f, dir.normalized, 2000f);
        }
    }
}
