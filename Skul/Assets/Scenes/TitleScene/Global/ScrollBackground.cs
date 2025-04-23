using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float baseSpeed = 0.05f;

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
        float distance = cameraTransform.position.x - cameraStartPos.x;

        for (int i = 0; i < layerRenderers.Length; i++)
        {
            if (layerRenderers[i] == null) continue;

            float offsetX = distance * parallaxSpeeds[i];
            layerRenderers[i].material.mainTextureOffset = new Vector2(offsetX, 0);
        }
        transform.position = new Vector3(cameraTransform.position.x, 0, 0);
    }
}