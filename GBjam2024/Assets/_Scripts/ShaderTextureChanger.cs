using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShaderTextureChanger : MonoBehaviour
{
    private TilemapRenderer _SR;
    [SerializeField] private Texture myTexture;

    private void Awake()
    {
        _SR = GetComponent<TilemapRenderer>();
    }
    void Start()
    {
        _SR.material.mainTexture = myTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
