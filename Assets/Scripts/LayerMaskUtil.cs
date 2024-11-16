using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerMaskUtil : MonoBehaviour
{
    public static bool ContainsLayer(LayerMask layerMask, GameObject gameObject)
    {
        return (layerMask.value & 1 << gameObject.layer) > 0;
    }   
}
