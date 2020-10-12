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
    Vector3 origin;
    float startingAngle;
    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;
    }
    // Start is called before the first frame update
    void Update()
    {
     
      
        Vector3[] vertices = new Vector3[rayCount +1+1];//orgin ray + ray 0
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount*3];//for every ray there should  be triangle and as the triagle has three vertices

        currentAngle = startingAngle;
        float angleIncrease = fovAngle / rayCount;

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D hitInfo = Physics2D.Raycast(origin, GetVectorFromAngle(currentAngle), viewDistance, ViewBlockingLayers);
            if (hitInfo)
            {
                vertex = hitInfo.point;
            }
            else
            {
                vertex = origin + GetVectorFromAngle(currentAngle) * viewDistance;
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

    }

    public Vector3 GetVectorFromAngle(float angle)
    {
        //angle -> 360
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDir)
    {
        startingAngle = GetAngleFromVector(aimDir) - fovAngle / 2;
    }

    public float GetAngleFromVector(Vector3 vector)
    {
        vector = vector.normalized;
        float n = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360f;
        return n;
    }
 
}
