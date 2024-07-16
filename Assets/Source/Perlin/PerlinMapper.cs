using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectRise.Perlin
{
    public class PerlinMapper : MonoBehaviour
    {
        public float lacunarity = 1;
        public float sampleSize = 0.01F;
        public float worldWidth = 10;
        public float worldHeight = 10;
        public float xOffset = 0;
        public float zOffset = 0;
        public Shader shader;

        private float lacunarityCache;
        private float sampleSizeCache;
        private float worldWidthCache;
        private float worldHeightCache;
        private float xOffsetCache;
        private float zOffsetCache;

        private void Update()
        {
            if (
                lacunarity == lacunarityCache
                && sampleSize == sampleSizeCache
                && worldWidth == worldWidthCache
                && worldHeight == worldHeightCache
                && xOffset == xOffsetCache
                && zOffset == zOffsetCache
            )
                return;

            Mesh mesh = new Mesh();
            mesh.vertices = BuildVertices();
            mesh.uv = BuildUvs();
            mesh.triangles = BuildTriangles();
            mesh.RecalculateNormals();
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            int numHorizontalPixels = (int) (worldWidth / sampleSize) + 1;
            int numVerticalPixels = (int)(worldHeight / sampleSize) + 1;
            Material material = new Material(shader);
            Texture2D texture2D = new Texture2D(numHorizontalPixels, numVerticalPixels);
            Color[] pixels = BuildPixels(numHorizontalPixels, numVerticalPixels);
            texture2D.SetPixels(pixels);
            texture2D.Apply();
            material.SetTexture("_MainTex", texture2D);
            GetComponent<MeshRenderer>().material = material;

            lacunarityCache = lacunarity;
            sampleSizeCache = sampleSize;
            worldWidthCache = worldWidth;
            worldHeightCache = worldHeight;
            xOffsetCache = xOffset;
            zOffsetCache = zOffset;
        }

        private Vector3[] BuildVertices()
        {
            List<Vector3> vertices = new List<Vector3>();
            vertices.Add(new Vector3(0, 0, 0));
            vertices.Add(new Vector3(worldWidth, 0, 0));
            vertices.Add(new Vector3(worldWidth, 0, worldHeight));
            vertices.Add(new Vector3(0, 0, worldHeight));
            return vertices.ToArray();
        }

        private int[] BuildTriangles()
        {
            List<int> triangles = new List<int>();
            triangles.Add(0);
            triangles.Add(3);
            triangles.Add(2);
            triangles.Add(0);
            triangles.Add(2);
            triangles.Add(1);
            return triangles.ToArray();
        }

        private Color[] BuildPixels(int width, int height)
        {
            List<Color> pixels = new List<Color>();
            for (int c = 0; c < height; c++)
            {
                float z = c * sampleSize;
                for (int r = 0; r < width; r++)
                {
                    float x = r * sampleSize;
                    float y = Mathf.PerlinNoise(
                        (x * lacunarity) + xOffset,
                        (z * lacunarity) + zOffset
                    );
                    pixels.Add(new Color(y, y, y));
                }
            }
            return pixels.ToArray();
        }

        private Vector2[] BuildUvs()
        {
            List<Vector2> uvs = new List<Vector2>();
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(0, 1));
            return uvs.ToArray();
        }
    }
}
