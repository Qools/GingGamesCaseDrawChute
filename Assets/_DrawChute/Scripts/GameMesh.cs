using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMesh : MonoBehaviour
{
    private MeshFilter gameMeshFilter;
    private MeshCollider gameMeshCollider;


    // Start is called before the first frame update
    void Start()
    {
        gameMeshFilter = GetComponent<MeshFilter>();
        gameMeshCollider = GetComponent<MeshCollider>();
    }

    private void OnEnable()
    {
        BusSystem.OnDrawEnd += ChangeMesh;
    }

    private void OnDisable()
    {
        BusSystem.OnDrawEnd -= ChangeMesh;
    }

    private void ChangeMesh(Mesh newMesh)
    {
        ResetMeshCollider();
        ResetMeshFilter();


        gameMeshFilter.mesh = newMesh;
        gameMeshCollider.sharedMesh = newMesh;
    }

    private void ResetMeshCollider()
    {
        gameMeshCollider.sharedMesh = null;
    }

    private void ResetMeshFilter()
    {
        gameMeshFilter.mesh = null;
    }
}
