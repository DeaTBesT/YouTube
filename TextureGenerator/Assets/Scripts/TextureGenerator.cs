using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TextureGenerator : MonoBehaviour
{
    [SerializeField] private int width = 256;
    [SerializeField] private int height = 256;

    [SerializeField] private FilterMode filterMode;

    [SerializeField] private float scale = 20;

    [SerializeField] private Color[] randomColors;

    [ContextMenu("Generate texture")]
    private void Generate()
    {
        Renderer m_render = GetComponent<Renderer>();
        m_render.material.mainTexture = GenerateTexture();
    }

    private Texture GenerateTexture()
    {
        Texture2D m_texture = new Texture2D(width, height);

        m_texture.filterMode = filterMode;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color m_color = CalculateColor(x, y);
                m_texture.SetPixel(x, y, m_color);
            }
        }

        m_texture.Apply();
        return m_texture;
    }

    private Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / height * scale;

        float m_noise = Mathf.PerlinNoise(xCoord, yCoord);

        float m_random = (Random.value - 0.5f) * m_noise;

        Color m_color = new Color();
        m_color = randomColors[Random.Range(0, randomColors.Length)] + new Color(m_random, m_random, m_random);
        
        return m_color;
    }
}
