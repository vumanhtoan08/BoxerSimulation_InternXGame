using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    [SerializeField] Renderer targetRenderer;
    [SerializeField] string textureProperty = "_MainTex";
    [SerializeField] Vector2 scrollSpeed = new(0.2f, 0f);

    Vector2 currentOffset;

    void Update()
    {
        if (targetRenderer == null) return;

        // Tính offset mới theo thời gian
        currentOffset += scrollSpeed * Time.deltaTime;
        currentOffset.x %= 1f;
        currentOffset.y %= 1f;

        // Áp dụng lên tất cả material của renderer
        foreach (var mat in targetRenderer.materials)
        {
            if (mat.HasProperty(textureProperty))
                mat.SetTextureOffset(textureProperty, currentOffset);
        }
    }
}
