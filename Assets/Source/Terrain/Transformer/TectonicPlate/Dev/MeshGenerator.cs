using UnityEngine;

namespace ProjectRise.Terrain.Transformer.TectonicPlate.Dev
{
    internal class MeshGenerator
    {
        internal static Mesh GenerateMesh(TerrainModel terrainModel)
        {
            float meshWidth = 0.6F;
            float meshHeight =
                ((float)terrainModel.VerticalTiles / terrainModel.HorizontalTiles) * meshWidth;
            float xOffset = meshWidth / 2;
            float yOffset = meshHeight / 2;
            Mesh mesh = new Mesh();
            Vector3 bottomLeft = new Vector3(-xOffset, -yOffset, 0);
            Vector3 bottomRight = new Vector3(xOffset, -yOffset, 0);
            Vector3 topLeft = new Vector3(-xOffset, yOffset, 0);
            Vector3 topRight = new Vector3(xOffset, yOffset, 0);
            Vector3[] vertices = new[] { bottomLeft, bottomRight, topLeft, topRight };
            int[] triangles = new[] { 0, 2, 3, 0, 3, 1 };
            Vector2 bottomLeftUV = new Vector2(0, 0);
            Vector2 bottomRightUV = new Vector2(0, 1);
            Vector2 topLeftUV = new Vector2(1, 0);
            Vector2 topRightUV = new Vector2(1, 1);
            Vector2[] uvs = new[] { bottomLeftUV, bottomRightUV, topLeftUV, topRightUV };
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.SetUVs(0, uvs);
            return mesh;
        }
    }
}
