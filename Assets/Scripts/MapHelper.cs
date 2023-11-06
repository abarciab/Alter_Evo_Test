using UnityEngine;
#nullable enable

public static class MapHelper
{
    public static Vector2 GetMapPositionOfWorldObject(Vector3 worldSpace, Vector3[] bounds)
    {
        //get min maxbounds 
        float minX = bounds[0].x;
        float maxX = bounds[0].x;
        float minZ = bounds[0].z;
        float maxZ = bounds[0].z;
        
        for (int i = 1; i < bounds.Length; i++)
        {
            var bound = bounds[i];
            minX = Mathf.Min(bound.x, minX);
            maxX = Mathf.Max(bound.x, maxX);
            minZ = Mathf.Min(bound.z, minZ);
            maxZ = Mathf.Max(bound.z, maxZ);
        }

        var magX = maxX - minX;
        var magZ = maxZ- minZ;
        var mapX = (worldSpace.x - minX) / magX;
        var mapZ = (worldSpace.z - minZ) / magZ;
        
        return new Vector2(mapX, mapZ);
    }
    
    public static Vector3[] GetBoxColliderCorners(BoxCollider b)
    {
        if (b == null) return new []{Vector3.zero};
        
        Vector3[] verts = new Vector3[4]; // Array that will contain the BOX Collider Vertices

        verts[0] = b.transform.position + new Vector3(b.size.x, 0, b.size.z) * 0.5f;
        verts[1] = b.transform.position + new Vector3(-b.size.x, 0, b.size.z) * 0.5f;
        verts[2] = b.transform.position + new Vector3(-b.size.x, 0, -b.size.z) * 0.5f;
        verts[3] = b.transform.position + new Vector3(b.size.x, 0, -b.size.z) * 0.5f;
        return verts;
    }
}