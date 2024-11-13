using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public GameObject[] BackgroundLayers;
    public float scrollOffset = 0.05f ;
    public float parallaxModifier = 1.0f;
    private GameObject _camera;
    private GameObject layer;
    private Material[] Materials;
    private Material _material;
    private float[] PreviousPositionsX;
    private float deltaX;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize Main Camera
        _camera = Camera.main.gameObject;

        Materials = new Material[BackgroundLayers.Length];

        PreviousPositionsX = new float[BackgroundLayers.Length];

        // Initialize Materials array and starting positions
        for (int i=0; i < Materials.Length; i++)
        {
            Materials[i] = BackgroundLayers[i].GetComponent<Renderer>().material;
            PreviousPositionsX[i] = BackgroundLayers[i].transform.position.x;

        }
        _material = null;


    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i < BackgroundLayers.Length; i++)
        {
            // Set layer transform in x as the camera's.
            layer = BackgroundLayers[i];
            layer.transform.position = new Vector3 (_camera.transform.position.x, layer.transform.position.y, layer.transform.position.z);

            // Set delta x of the layer position
            deltaX =  layer.transform.position.x - PreviousPositionsX[i];

            // Scroll the layer's texture.
            _material = Materials[i];
            _material.SetTextureOffset("_MainTex", _material.GetTextureOffset("_MainTex") + deltaX * scrollOffset * Vector2.right / (1.0f + i * parallaxModifier));

            // Set this as the layer's previous position;
            PreviousPositionsX[i] = layer.transform.position.x;

        }
    }
}
