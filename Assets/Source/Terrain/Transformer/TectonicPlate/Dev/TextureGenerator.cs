using ProjectRise.ProceduralGeneration.External;
using UnityEngine;

namespace ProjectRise.Terrain.Transformer.TectonicPlate.Dev
{
    internal class TextureGenerator
    {
        internal static Texture2D GenerateTextureFromModel(
            PerlinModel model,
            TerrainModel terrainModel
        )
        {
            Texture2D texture = new Texture2D(
                terrainModel.HorizontalTiles,
                terrainModel.VerticalTiles
            );
            Color[] pixels = new Color[terrainModel.HorizontalTiles * terrainModel.VerticalTiles];
            int p = 0;
            for (int y = 0; y < terrainModel.VerticalTiles; y++)
            {
                for (int x = 0; x < terrainModel.HorizontalTiles; x++)
                {
                    float sample = Mathf.PerlinNoise(
                        (x * model.Lacunarity) + model.XOffset,
                        (y * model.Lacunarity) + model.ZOffset
                    );
                    pixels[p] = new Color(sample, sample, sample);
                    p++;
                }
            }
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        internal static Texture2D GeneratePlateTextureFromModel(
            PerlinModel model,
            TerrainModel terrainModel
        )
        {
            Texture2D texture = new Texture2D(
                terrainModel.HorizontalTiles,
                terrainModel.VerticalTiles
            );
            Color[] pixels = new Color[terrainModel.HorizontalTiles * terrainModel.VerticalTiles];
            int p = 0;
            for (int y = 0; y < terrainModel.VerticalTiles; y++)
            {
                for (int x = 0; x < terrainModel.HorizontalTiles; x++)
                {
                    float sample = Mathf.PerlinNoise(
                        (x * model.Lacunarity) + model.XOffset,
                        (y * model.Lacunarity) + model.ZOffset
                    );
                    float color = sample >= 0.7 ? 1 : 0;
                    pixels[p] = new Color(color, color, color);
                    p++;
                }
            }
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
    }
}
