using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTexture : MonoBehaviour
{
    [SerializeField] private float tileSpeed = 1;

    private Material tileMesh;

    private float minMaxDirection = 4;
    // Start is called before the first frame update
    void Start()
    {
        tileMesh = GetComponent<MeshRenderer>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 scale = tileMesh.mainTextureOffset;

        scale.x += Mathf.Sin(Time.time)*tileSpeed*Time.deltaTime;
        scale.y += Mathf.Cos(Time.time)*tileSpeed*Time.deltaTime;
       // scale.x += tileSpeed * Time.deltaTime;
        //scale.y+= tileSpeed * Time.deltaTime;

        scale = new Vector2(Mathf.Clamp(scale.x,-minMaxDirection,minMaxDirection),Mathf.Clamp(scale.y,-minMaxDirection,minMaxDirection));
        tileMesh.mainTextureOffset = scale;
    }
}
