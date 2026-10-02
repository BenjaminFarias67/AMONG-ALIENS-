using UnityEngine;
using UnityEngine.UI;

public class AlienHealthBar : MonoBehaviour
{
    [Header("POSIÇÃO DA BARRA")]

    [Tooltip("Altura da barra em relação ao Alien.")]
    public float Height = 1.55f;

    [Tooltip("Deslocamento horizontal da barra.")]
    public float OffsetX = 0f;

    [Tooltip("Deslocamento para frente/trás da barra.")]
    public float OffsetZ = 0f;


    [Header("TAMANHO DA BARRA")]

    [Tooltip("Largura máxima da barra.")]
    public float BarWidth = 160f;

    [Tooltip("Espessura/altura da barra.")]
    public float BarHeight = 12f;

    [Tooltip("Tamanho geral da barra no mundo.")]
    public float BarScale = 0.01f;


    [Header("ESTÉTICA")]

    [Tooltip("Cor da vida do Alien.")]
    public Color HealthColor = Color.green;

    [Tooltip("Cor do fundo da barra.")]
    public Color BackgroundColor = Color.black;

    [Range(0f, 1f)]
    [Tooltip("Transparência do fundo.")]
    public float BackgroundAlpha = 0.85f;


    private AlienHealth alienHealth;

    private Canvas canvas;
    private RectTransform fillRect;
    private Image fillImage;
    private Image backgroundImage;

    private Camera mainCamera;


    private void Start()
    {
        alienHealth =
            GetComponent<AlienHealth>();

        mainCamera =
            Camera.main;

        CreateHealthBar();
    }


    private void Update()
    {
        if (alienHealth == null)
            return;

        if (fillRect == null)
            return;

        if (alienHealth.maxHealth <= 0f)
            return;


        // ==========================================
        // VIDA
        // ==========================================

        float healthPercent =
            alienHealth.currentHealth /
            alienHealth.maxHealth;

        healthPercent =
            Mathf.Clamp01(healthPercent);


        // ==========================================
        // ATUALIZA TAMANHO DA BARRA
        // ==========================================

        fillRect.sizeDelta =
            new Vector2(
                BarWidth * healthPercent,
                BarHeight
            );


        // ==========================================
        // ATUALIZA POSIÇÃO
        // ==========================================

        canvas.transform.localPosition =
            new Vector3(
                OffsetX,
                Height,
                OffsetZ
            );


        // ==========================================
        // ATUALIZA TAMANHO
        // ==========================================

        canvas.transform.localScale =
            Vector3.one * BarScale;


        // ==========================================
        // ATUALIZA CORES
        // ==========================================

        if (fillImage != null)
        {
            fillImage.color =
                HealthColor;
        }

        if (backgroundImage != null)
        {
            Color backgroundColor =
                BackgroundColor;

            backgroundColor.a =
                BackgroundAlpha;

            backgroundImage.color =
                backgroundColor;
        }


        // ==========================================
        // OLHAR PARA A CÂMERA
        // ==========================================

        if (mainCamera != null)
        {
            canvas.transform.LookAt(
                canvas.transform.position +
                mainCamera.transform.rotation *
                Vector3.forward,

                mainCamera.transform.rotation *
                Vector3.up
            );
        }
    }


    private void CreateHealthBar()
    {
        // ==========================================
        // CANVAS
        // ==========================================

        GameObject canvasObject =
            new GameObject(
                "Alien Health Bar"
            );

        canvasObject.transform.SetParent(
            transform,
            false
        );

        canvas =
            canvasObject.AddComponent<Canvas>();

        canvas.renderMode =
            RenderMode.WorldSpace;


        CanvasScaler scaler =
            canvasObject.AddComponent<CanvasScaler>();

        scaler.dynamicPixelsPerUnit =
            10f;


        canvasObject.AddComponent<
            GraphicRaycaster
        >();


        canvasObject.transform.localPosition =
            new Vector3(
                OffsetX,
                Height,
                OffsetZ
            );

        canvasObject.transform.localRotation =
            Quaternion.identity;

        canvasObject.transform.localScale =
            Vector3.one * BarScale;


        // ==========================================
        // FUNDO
        // ==========================================

        GameObject backgroundObject =
            new GameObject(
                "Background"
            );

        backgroundObject.transform.SetParent(
            canvasObject.transform,
            false
        );


        backgroundImage =
            backgroundObject.AddComponent<Image>();


        Color backgroundColor =
            BackgroundColor;

        backgroundColor.a =
            BackgroundAlpha;

        backgroundImage.color =
            backgroundColor;


        RectTransform backgroundRect =
            backgroundImage.GetComponent<
                RectTransform
            >();


        backgroundRect.sizeDelta =
            new Vector2(
                BarWidth,
                BarHeight
            );


        // ==========================================
        // VIDA
        // ==========================================

        GameObject fillObject =
            new GameObject(
                "Fill"
            );

        fillObject.transform.SetParent(
            backgroundObject.transform,
            false
        );


        fillImage =
            fillObject.AddComponent<Image>();

        fillImage.color =
            HealthColor;


        fillRect =
            fillImage.GetComponent<
                RectTransform
            >();


        fillRect.sizeDelta =
            new Vector2(
                BarWidth,
                BarHeight
            );


        // A barra começa pela esquerda
        fillRect.anchorMin =
            new Vector2(
                0f,
                0.5f
            );

        fillRect.anchorMax =
            new Vector2(
                0f,
                0.5f
            );

        fillRect.pivot =
            new Vector2(
                0f,
                0.5f
            );

        fillRect.anchoredPosition =
            Vector2.zero;
    }
}