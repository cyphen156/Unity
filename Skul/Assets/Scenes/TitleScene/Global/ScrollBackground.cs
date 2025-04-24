using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float baseSpeed = 0.05f;
    [SerializeField] private float verticalOffset = -5f;

    private Renderer[] layerRenderers = new Renderer[11];
    private float[] parallaxSpeeds = new float[11];
    private Vector3 cameraStartPos;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        cameraStartPos = cameraTransform.position;

        for (int i = 0; i < layerRenderers.Length; i++)
        {
            Transform layer = transform.Find(i.ToString());
            if (layer != null)
            {
                layerRenderers[i] = layer.GetComponent<Renderer>();
                parallaxSpeeds[i] = baseSpeed / (i + 1f);
            }
        }
    }

    void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - cameraStartPos;

        for (int i = 0; i < layerRenderers.Length; i++)
        {
            if (layerRenderers[i] == null)
            {
                continue;
            }

            float offsetX = delta.x * parallaxSpeeds[i];
            float offsetY = -delta.y * parallaxSpeeds[i];

            layerRenderers[i].material.mainTextureOffset = new Vector2(offsetX, offsetY);
        }
        transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y + verticalOffset, 0);
    }
}