using UnityEngine;

public class LlaveInglesa : HerramientaBase
{
    public override bool PuedeReparar(TipoAveria averia)
    {
        // Evalúa la condición y retorna automáticamente true o false
        return averia == TipoAveria.FugaFuerte;
    }
}
