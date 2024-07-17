using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.TectonicPlates
{
    public class TectonicPlates : MonoBehaviour
    {
        public bool update = true;
        public float lacunarity = 0.013F;
        public float xOffset = 0;
        public float zOffset = 0;
        public float tileSize = 1;
        public float worldWidth = 700;
        public float worldHeight = 500;
        public bool randomOffsets = true;
        public Shader shader;

        private void Update()
        {
            if (!update)
            {
                return;
            }

            if (randomOffsets)
            {
                xOffset = Random.Range(100, 9999);
                zOffset = Random.Range(100, 9999);
            }
            GetComponent<MeshFilter>().mesh = BuildMesh();
            GetComponent<MeshFilter>().mesh.RecalculateNormals();
            Material material = new Material(shader);
            float[] plates = GeneratePlates();
            bool[] queuedCache = new bool[plates.Length];
            Queue<int> edgeQueue = FindEdges(plates);
            int[] plateIds = IdentifyPlates(plates);
            Dictionary<int, bool> edges = new Dictionary<int, bool>();
            while (edgeQueue.Count > 0)
            {
                edgeQueue = ExpandPlates(edgeQueue, queuedCache, plateIds, plates, 1, edges);
            }
            material.SetTexture("_MainTex", GenerateTextureWithPlateIds(plateIds));
            GetComponent<MeshRenderer>().material = material;
            update = false;
        }

        private Queue<int> ExpandPlates(Queue<int> edgeQueue, bool[] queuedCache, int[] plateIds, float[] plates,
            int recursiveDepth, Dictionary<int, bool> edges)
        {
            if (edgeQueue.Count == 0 || recursiveDepth >= 50)
                return edgeQueue;

            int currentEdgeIndex = edgeQueue.Dequeue();
            if (!queuedCache[currentEdgeIndex])
            {
                int newId = GetNearestPlateId(currentEdgeIndex, plateIds);
                EnqueueEdges(currentEdgeIndex, edgeQueue, queuedCache, plates);
                plateIds[currentEdgeIndex] = newId;
                plates[currentEdgeIndex] = 1;
                queuedCache[currentEdgeIndex] = true;
            }
            recursiveDepth++;
            return ExpandPlates(edgeQueue, queuedCache, plateIds, plates, recursiveDepth, edges);
        }
        
        private void EnqueueSurroundings(int index, Queue<int> edgeQueue, bool[] queuedCache, float[] plates)
        {
            int top = index + GetNumHorizontalSamples();
            if (top < queuedCache.Length && !queuedCache[top] && plates[top] == 0)
                edgeQueue.Enqueue(top);
            int bottom = index - GetNumHorizontalSamples();
            if (bottom >= 0 && !queuedCache[bottom] && plates[bottom] == 0)
                edgeQueue.Enqueue(bottom);
            int left = index - 1;
            if (left >= 0 && index % GetNumHorizontalSamples() != 0 && !queuedCache[left] && plates[left] == 0)
                edgeQueue.Enqueue(left);
            int right = index + 1;
            if (right < queuedCache.Length && right % GetNumHorizontalSamples() != 0 && !queuedCache[right] && plates[right] == 0)
                edgeQueue.Enqueue(right);
            int topleft = index + GetNumHorizontalSamples() - 1;
            if (topleft < queuedCache.Length && index % GetNumHorizontalSamples() != 0 && !queuedCache[topleft] && plates[topleft] == 0)
                edgeQueue.Enqueue(topleft);
            int topright = index + GetNumHorizontalSamples() + 1;
            if (topright < queuedCache.Length && topright % GetNumHorizontalSamples() != 0 && !queuedCache[topright] && plates[topright] == 0)
                edgeQueue.Enqueue(topright);
            int bottomleft = index - GetNumHorizontalSamples() - 1;
            if (bottomleft >= 0 && index % GetNumHorizontalSamples() != 0 && !queuedCache[bottomleft] && plates[bottomleft] == 0)
                edgeQueue.Enqueue(bottomleft);
            int bottomright = index - GetNumHorizontalSamples() + 1;
            if (bottomright >= 0 && bottomright % GetNumHorizontalSamples() != 0 && !queuedCache[bottomright] && plates[bottomright] == 0)
                edgeQueue.Enqueue(bottomright);
        }


        private int GetNearestPlateId(int index, int[] plateIds)
        {
            int top = index + GetNumHorizontalSamples();
            if (top < plateIds.Length && plateIds[top] != 0)
                return plateIds[top];
            int bottom = index - GetNumHorizontalSamples();
            if (bottom >= 0 && plateIds[bottom] != 0)
                return plateIds[bottom];
            int left = index - 1;
            if (index % GetNumHorizontalSamples() != 0 && plateIds[left] != 0)
                return plateIds[left];
            int right = index + 1;
            if (right % GetNumHorizontalSamples() != 0 && plateIds[right] != 0)
                return plateIds[right];
            return 0;
        }
        
        private bool IsAtBoundary(int index, int[] plateIds)
        {
            Dictionary<int, bool> plateTypes = new Dictionary<int, bool>();
            int top = index + GetNumHorizontalSamples();
            if (top < plateIds.Length && plateIds[top] != 0)
                plateTypes.Add(top, true);
            int bottom = index - GetNumHorizontalSamples();
            if (bottom >= 0 && plateIds[bottom] != 0)
                plateTypes.Add(bottom, true);
            int left = index - 1;
            if (index % GetNumHorizontalSamples() != 0 && plateIds[left] != 0)
                plateTypes.Add(left, true);
            int right = index + 1;
            if (right % GetNumHorizontalSamples() != 0 && plateIds[right] != 0)
                plateTypes.Add(right, true);
            return plateTypes.Count > 1;
        }

        private Queue<int> FindEdges(float[] plates)
        {
            Queue<int> edgeQueue = new Queue<int>();
            bool[] queuedCache = new bool[plates.Length];
            for (int i = 0; i < plates.Length; i++)
            {
                if (plates[i] == 1 && !queuedCache[i])
                {
                    Queue<int> plateQueue = new Queue<int>();
                    plateQueue.Enqueue(i);
                    while (plateQueue.Count > 0)
                    {
                        plateQueue = FindEdges(plateQueue, edgeQueue, queuedCache, plates, 1);
                    }
                }
            }
            return edgeQueue;
        }

        private Queue<int> FindEdges(Queue<int> plateQueue, Queue<int> edgeQueue, bool[] queuedCache, float[] plates,
            int recursionDepth)
        {
            if (plateQueue.Count == 0 || recursionDepth >= 50)
                return plateQueue;

            int currentPlateIndex = plateQueue.Dequeue();
            if (!queuedCache[currentPlateIndex])
            {
                queuedCache[currentPlateIndex] = true;
                EnqueueNeighbors(currentPlateIndex, plateQueue, queuedCache, plates);
                EnqueueEdges(currentPlateIndex, edgeQueue, queuedCache, plates);
            }

            recursionDepth++;
            return FindEdges(plateQueue, edgeQueue, queuedCache, plates, recursionDepth);
        }
        
        private void EnqueueEdges(int currentPlateIndex, Queue<int> edgeQueue, bool[] queuedCache, float[] plates)
        {
            int top = currentPlateIndex + GetNumHorizontalSamples();
            if (top < queuedCache.Length && !queuedCache[top] && plates[top] == 0)
                edgeQueue.Enqueue(top);
            int bottom = currentPlateIndex - GetNumHorizontalSamples();
            if (bottom >= 0 && !queuedCache[bottom] && plates[bottom] == 0)
                edgeQueue.Enqueue(bottom);
            int left = currentPlateIndex - 1;
            if (left >= 0 && currentPlateIndex % GetNumHorizontalSamples() != 0 && !queuedCache[left] && plates[left] == 0)
                edgeQueue.Enqueue(left);
            int right = currentPlateIndex + 1;
            if (right < queuedCache.Length && right % GetNumHorizontalSamples() != 0 && !queuedCache[right] && plates[right] == 0)
                edgeQueue.Enqueue(right);
        }


        private int[] IdentifyPlates(float[] plates)
        {
            int id = 1;
            int[] plateIds = new int[plates.Length];
            bool[] identifiedCache = new bool[plates.Length];
            for (int i = 0; i < plates.Length; i++)
            {
                if (!identifiedCache[i] && plates[i] == 1.0)
                {
                    Queue<int> plateIndexQueue = new Queue<int>();
                    plateIndexQueue.Enqueue(i);
                    while (plateIndexQueue.Count > 0)
                        plateIndexQueue = IdentifyPlates(id, plateIndexQueue, plateIds, identifiedCache, plates, 1);
                    id++;
                }
                else if (plates[i] == 0)
                {
                    plateIds[i] = 0;
                    identifiedCache[i] = true;
                }
            }
            return plateIds;
        }

        private Queue<int> IdentifyPlates(int id, Queue<int> plateIndexQueue, int[] plateIds, bool[] identifiedCache, float[] plates, int recursionDepth)
        {
            if (plateIndexQueue.Count == 0)
                return plateIndexQueue;
            if (recursionDepth >= 50)
                return plateIndexQueue;

            int currentPlateIndex = plateIndexQueue.Dequeue();
            if (!identifiedCache[currentPlateIndex])
            {
                plateIds[currentPlateIndex] = id;
                identifiedCache[currentPlateIndex] = true;
                EnqueueNeighbors(currentPlateIndex, plateIndexQueue, identifiedCache, plates);
            }

            recursionDepth++;
            return IdentifyPlates(id, plateIndexQueue, plateIds, identifiedCache, plates, recursionDepth);
        }

        private void EnqueueNeighbors(int currentPlateIndex, Queue<int> plateIndexQueue, bool[] identifiedCache, float[] plates)
        {
            int top = currentPlateIndex + GetNumHorizontalSamples();
            if (top < identifiedCache.Length && !identifiedCache[top] && plates[top] == 1)
                plateIndexQueue.Enqueue(top);
            int bottom = currentPlateIndex - GetNumHorizontalSamples();
            if (bottom >= 0 && !identifiedCache[bottom] && plates[bottom] == 1)
                plateIndexQueue.Enqueue(bottom);
            int left = currentPlateIndex - 1;
            if (left >= 0 && currentPlateIndex % GetNumHorizontalSamples() != 0 && !identifiedCache[left] && plates[left] == 1)
                plateIndexQueue.Enqueue(left);
            int right = currentPlateIndex + 1;
            if (right < identifiedCache.Length && right % GetNumHorizontalSamples() != 0 && !identifiedCache[right] && plates[right] == 1)
                plateIndexQueue.Enqueue(right);
        }

        private float[] GeneratePlates()
        {
            List<float> plates = new List<float>();
            for (int zSample = 0; zSample < GetNumVerticalSamples(); zSample++)
            {
                for (int xSample = 0; xSample < GetNumHorizontalSamples(); xSample++)
                {
                    float yNoise = Mathf.PerlinNoise((xSample * tileSize * lacunarity) + xOffset,
                        (zSample * tileSize * lacunarity) + zOffset);
                    float y = yNoise >= 0.7 ? 1 : 0;
                    plates.Add(y);
                }
            }
            return plates.ToArray();
        }

        private Texture2D GenerateTexture(float[] yValues)
        {
            List<Color> pixels = new List<Color>();
            foreach (float y in yValues)
            {
                pixels.Add(new Color(y, y, y));
            }

            Texture2D texture = new Texture2D(GetNumHorizontalSamples(), GetNumVerticalSamples());
            texture.SetPixels(pixels.ToArray());
            texture.Apply();
            return texture;
        }

        private Texture2D GenerateTextureWithEdges(int[] plateIds, Queue<int> edgeQueue)
        {
            List<Color> pixels = new List<Color>();
            foreach (int id in plateIds)
            {
                float y = id == 0 ? 0 : 1;
                pixels.Add(new Color(y, y, y));
            }

            Color[] pixelArray = pixels.ToArray();
            while (edgeQueue.Count > 0)
            {
                pixelArray[edgeQueue.Dequeue()] = new Color(1, 0, 0);
            }

            Texture2D texture = new Texture2D(GetNumHorizontalSamples(), GetNumVerticalSamples());
            texture.SetPixels(pixelArray);
            texture.Apply();
            return texture;
        }

        private Texture2D GenerateTextureWithPlateIds(int[] plateIds)
        {
            List<Color> pixels = new List<Color>();
            foreach (int id in plateIds)
            {
                float y = id == 0 ? 0 : 1 - (0.05F * id);
                pixels.Add(new Color(y, y, y));
            }

            Texture2D texture = new Texture2D(GetNumHorizontalSamples(), GetNumVerticalSamples());
            texture.SetPixels(pixels.ToArray());
            texture.Apply();
            return texture;
        }

        private Mesh BuildMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = BuildVertices();
            mesh.triangles = BuildTriangles();
            mesh.uv = BuildUvs();
            return mesh;
        }

        private Vector3[] BuildVertices()
        {
            List<Vector3> vertices = new List<Vector3>();
            vertices.Add(new Vector3(0, 0, 0));
            vertices.Add(new Vector3(worldWidth, 0, worldHeight));
            vertices.Add(new Vector3(0, 0, worldHeight));
            vertices.Add(new Vector3(worldWidth, 0, 0));
            return vertices.ToArray();
        }

        private int[] BuildTriangles()
        {
            List<int> triangles = new List<int>();
            triangles.Add(0);
            triangles.Add(2);
            triangles.Add(1);
            triangles.Add(0);
            triangles.Add(1);
            triangles.Add(3);
            return triangles.ToArray();
        }

        private Vector2[] BuildUvs()
        {
            List<Vector2> uvs = new List<Vector2>();
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 0));
            return uvs.ToArray();
        }

        private int GetNumHorizontalSamples()
        {
            return (int)(worldWidth / tileSize) + 1;
        }

        private int GetNumVerticalSamples()
        {
            return (int)(worldHeight / tileSize) + 1;
        }
    }
}