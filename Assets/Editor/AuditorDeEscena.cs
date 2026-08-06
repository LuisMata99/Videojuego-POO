using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class AuditorDeEscena : EditorWindow
{
    [MenuItem("Herramientas POO/Generar Reporte de Escena Avanzado")]
    public static void GenerarReporte()
    {
        StringBuilder reporte = new StringBuilder();
        reporte.AppendLine("=== REPORTE DE AUDITORÍA ARQUITECTÓNICA ===");

        GameObject[] objetosRaiz = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject obj in objetosRaiz)
        {
            EscanearJerarquia(obj, reporte, 0);
        }

        string ruta = Path.Combine(Application.dataPath, "ReporteEscenaPOO.txt");
        File.WriteAllText(ruta, reporte.ToString());
        Debug.LogWarning("Auditoría avanzada completada. Reporte guardado en: " + ruta);
    }

    private static void EscanearJerarquia(GameObject obj, StringBuilder reporte, int nivel)
    {
        string sangria = new string('-', nivel * 2);
        string capa = LayerMask.LayerToName(obj.layer);

        string datosComponentes = ExtraerTelemetriaCritica(obj);

        reporte.AppendLine($"{sangria}> {obj.name} (Capa: {capa}) {datosComponentes}");

        foreach (Transform hijo in obj.transform)
        {
            EscanearJerarquia(hijo.gameObject, reporte, nivel + 1);
        }
    }

    private static string ExtraerTelemetriaCritica(GameObject obj)
    {
        StringBuilder telemetria = new StringBuilder();

        // Auditoría de UI 
        Canvas canvas = obj.GetComponent<Canvas>();
        if (canvas != null)
        {
            telemetria.Append($"[Canvas: {canvas.renderMode}] ");
        }

        GraphicRaycaster raycaster = obj.GetComponent<GraphicRaycaster>();
        if (raycaster != null)
        {
            string estadoRaycaster = raycaster.enabled ? "ACTIVO (¡Peligro de bloqueo físico!)" : "Inactivo";
            telemetria.Append($"[Raycaster: {estadoRaycaster}] ");
        }

        // Auditoría de Físicas
        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
        {
            string estadoFisico = collider.isTrigger ? "Trigger (Atravesable)" : "Sólido";
            telemetria.Append($"[Collider: {estadoFisico}] ");
        }

        return telemetria.ToString();
    }
}