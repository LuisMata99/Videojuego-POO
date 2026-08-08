using UnityEngine;

public class FeedbackVisual : MonoBehaviour
{
    [Header("Configuración Visual")]
    [SerializeField] private Material materialResaltado;

    // Arrays para soportar modelos 3D compuestos por múltiples piezas/hijos
    private Renderer[] renderizadores;
    private Material[] materialesOriginales;
    private bool estaResaltado = false;

    private void Awake()
    {
        // GetComponentsInChildren busca automáticamente el componente Renderer 
        // tanto en este objeto como en todos los objetos anidados dentro de él.
        renderizadores = GetComponentsInChildren<Renderer>();

        // Inicializamos el array de memoria con la cantidad exacta de piezas encontradas
        materialesOriginales = new Material[renderizadores.Length];

        // Guardamos en caché el material original de cada pieza para poder restaurarlo
        for (int i = 0; i < renderizadores.Length; i++)
        {
            materialesOriginales[i] = renderizadores[i].material;
        }
    }

    /// <summary>
    /// Recorre todas las mallas del modelo 3D y les aplica el material de emisión.
    /// </summary>
    public void Resaltar()
    {
        if (estaResaltado) return;

        foreach (Renderer rnd in renderizadores)
        {
            rnd.material = materialResaltado;
        }
        estaResaltado = true;
    }

    /// <summary>
    /// Devuelve a cada pieza del modelo 3D su material y textura original.
    /// </summary>
    public void Restaurar()
    {
        if (!estaResaltado) return;

        for (int i = 0; i < renderizadores.Length; i++)
        {
            renderizadores[i].material = materialesOriginales[i];
        }
        estaResaltado = false;
    }
}