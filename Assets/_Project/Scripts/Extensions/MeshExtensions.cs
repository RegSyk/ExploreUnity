using UnityEngine;

namespace Explore
{
    public static class MeshExtensions
    {
        public static Vector3 CalculateMeshCenter(this Mesh mesh)
        {
            Vector3 center = Vector3.zero;

            for (int i = 0; i < mesh.vertexCount; i++)
            {
                center += mesh.vertices[i];
            }

            return center / mesh.vertexCount;
        }
    }
}
