using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ForceMeshCollider : MonoBehaviour
{
    void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshCollider mc = GetComponent<MeshCollider>();

        if (mc == null)
            mc = gameObject.AddComponent<MeshCollider>();

        mc.sharedMesh = null;
        mc.sharedMesh = mf.sharedMesh;

        mc.convex = false; 
    }
}
