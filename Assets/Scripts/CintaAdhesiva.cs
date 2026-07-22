using UnityEngine;

public class CintaAdhesiva : HerramientaBase
{
    public override bool PuedeReparar(TipoAveria averia)
    {
        return averia == TipoAveria.Fisura;
    }
}
