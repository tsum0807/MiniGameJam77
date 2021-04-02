using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    [SerializeField] private float fov;
    [SerializeField] private int numRay;
    [SerializeField] private float viewDistance;
    [SerializeField] private LayerMask IgnoreLayer;
    // [SerializeField] private float fov = 90f;

    private float startingAngle = 0f;

    private MeshFilter _meshFilter;
    // private M

    void Start(){
        _meshFilter = GetComponent<MeshFilter>();
    }

    void Update(){
        UpdateMesh();
    }

    // Update mesh with new fov
    private void UpdateMesh(){
        // raycount + origin and zero ray
        int actualNumRay = numRay + 2;
        Vector3[] vertices = new Vector3[actualNumRay];
        Vector2[] uv = new Vector2[actualNumRay];
        int[] triangles = new int[numRay * 3];

        // first vertex is the origin so 0,0
        vertices[0] = Vector3.zero;

        float curAngle = startingAngle;
        float angleIncreaseAmount = fov/numRay;
        int curVertex = 1;
        // Calculate each vertex of each triangle
        for(int i=0; i<=numRay; i++){
            Vector3 vertex = new Vector3();
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, GetVectorFromAngle(curAngle), viewDistance, ~IgnoreLayer);
            if(raycastHit2D.collider == null){
                // raycast didnt hit
                vertex = GetVectorFromAngle(curAngle) * viewDistance;
            }else{
                // hit
                vertex = raycastHit2D.point - (Vector2)transform.position;
            }
            vertices[curVertex] = vertex;

            if(i > 0){
                // 3 vertices in a triangle
                int curTriIndex = 3 * (i-1);
                triangles[curTriIndex] = 0;
                triangles[curTriIndex + 1] = curVertex-1;
                triangles[curTriIndex + 2] = curVertex;                
            }

            curAngle -= angleIncreaseAmount;
            curVertex++;
        }

        // Set value to our new mesh and update it to object
        Mesh newMesh = new Mesh();
        newMesh.vertices = vertices;
        newMesh.uv = uv;
        newMesh.triangles = triangles;
    
        newMesh.RecalculateBounds();

        _meshFilter.mesh = newMesh;
    }

    // From codemonkey utils
    private Vector3 GetVectorFromAngle(float angle){
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public void AimAtAngle(float angle){
        // Make sure view is in middle
        angle -= fov/2f;
        startingAngle = angle;
    }
}
