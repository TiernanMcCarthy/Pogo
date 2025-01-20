using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterials : MonoBehaviour
{

    [SerializeField] private List<MeshRenderer> renderers = new List<MeshRenderer>();
    
    [SerializeField] private Material newMaterial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ApplyMaterial()
    {
        foreach (var renderer in renderers)
        {
            renderer.material= newMaterial;
        }
    }
}
