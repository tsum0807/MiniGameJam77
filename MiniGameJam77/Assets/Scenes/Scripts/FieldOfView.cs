using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    
    [Header("Battery Settings")]
    [SerializeField] private float batteryUsagePerSec;

    [Header("Cone Settings")]
    [SerializeField] private float normalFOV;
    [SerializeField] private float normalViewDistance;
    [SerializeField] private float focusedFOV;
    [SerializeField] private float focusedViewDistance;
    [SerializeField] private int numRay;
    [SerializeField] private LayerMask IgnoreLayers;
    [SerializeField] private LayerMask MonsterLayer;
    [SerializeField] private LayerMask BehindMaskLayer;
    [SerializeField] private LayerMask ObjectLayer;
    [SerializeField] private LayerMask AfterObjectLayer;

    // [SerializeField] private float fov = 90f;


    private float _fov;
    private float _viewDistance;
    private float startingAngle = 0f;
    public bool isFocused = false; // 0 - normal, 1 - focused
    private LayerMask curIgnoreLayers;

    private MeshFilter _meshFilter;
    private PlayerController playerController;

    void Start(){
        _meshFilter = GetComponent<MeshFilter>();
        playerController = transform.parent.GetComponent<PlayerController>();
        // starts off with normal fov and view dist
        _fov = normalFOV;
        _viewDistance = normalViewDistance;
        curIgnoreLayers = IgnoreLayers;
    }

    void Update(){
        UpdateMesh();

        // Use Battery
        if(isFocused)
        {
            // How much to use per second
            playerController.ChangeBattery(Time.deltaTime * -batteryUsagePerSec);
            if(playerController.GetCurBattery() <= 0){
                // Unfocus when out of batttery
                isFocused = false;
                SetMode(0);
            }
        }
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
        float angleIncreaseAmount = _fov/numRay;
        int curVertex = 1;
        // Calculate each vertex of each triangle
        for(int i=0; i<=numRay; i++){
            Vector3 vertex = new Vector3();
            // Reassign ignore layers
            curIgnoreLayers = IgnoreLayers;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, GetVectorFromAngle(curAngle), _viewDistance, ~curIgnoreLayers);


            while (raycastHit2D.collider != null)
            {
                if(raycastHit2D.collider.tag == "Transparent")
                {
                    // Has hit box but should be see through
                    curIgnoreLayers += BehindMaskLayer;
                    // cast new ray
                    raycastHit2D = Physics2D.Raycast(raycastHit2D.point, GetVectorFromAngle(curAngle), _viewDistance - raycastHit2D.distance, ~curIgnoreLayers);
                }
                else if(raycastHit2D.collider.tag == "Monster")
                {
                    // Monster
                    if(isFocused)
                    {
                        // Do dmg to monster if in focus mode
                        raycastHit2D.collider.GetComponent<MonsterController>().TakeDamage();
                    }
                    curIgnoreLayers += MonsterLayer;
                    // cast new ray from monster
                    raycastHit2D = Physics2D.Raycast(raycastHit2D.point,
                                                     GetVectorFromAngle(curAngle),
                                                     _viewDistance - raycastHit2D.distance,
                                                     ~curIgnoreLayers);
                }
                else if(raycastHit2D.collider.tag == "Object")
                {
                    // Objects are not see through but can be seen
                    raycastHit2D = Physics2D.Raycast(raycastHit2D.point,
                                                     GetVectorFromAngle(curAngle),
                                                     _viewDistance - raycastHit2D.distance,
                                                     AfterObjectLayer);
                }
                else
                {
                    // Hit wall
                    vertex = raycastHit2D.point - (Vector2)transform.position;
                    break;
                }
            }

            if(raycastHit2D.collider == null){
                // raycast didnt hit
                vertex = GetVectorFromAngle(curAngle) * _viewDistance;
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
        newMesh.RecalculateNormals();

        _meshFilter.mesh = newMesh;
    }

    // From codemonkey utils
    private Vector3 GetVectorFromAngle(float angle){
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public void AimAtAngle(float angle){
        // Make sure view is in middle
        angle += _fov/2f;
        startingAngle = angle;
    }

    public void SwitchMode(){
        isFocused = !isFocused;
        if(!isFocused)
        {
            // Normal wide mode
            SetMode(0);
        }else if(playerController.GetCurBattery() > 0)
        {
            // Focused mode if player has battery
            SetMode(1);
        }
        numRay = (int)_fov;
    }

    // 0 - normal
    // 1 - focused
    private void SetMode(int mode)
    {
        switch (mode)
        {
            case 0:
                // Normal wide mode
                SetFOV(normalFOV);
                SetnormalViewDistance(normalViewDistance);
                break;
            case 1:
                // Focused mode if player has battery
                SetFOV(focusedFOV);
                SetnormalViewDistance(focusedViewDistance);
                break;
        }
    }

    public void SetFOV(float newFOV){
        _fov = newFOV;
    }

    public void SetnormalViewDistance(float newDist){
        _viewDistance = newDist;
    }
}
