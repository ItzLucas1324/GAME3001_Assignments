using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField] float scrollSpeed;
    private SpriteRenderer sr;
    private Vector2 offset;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        offset = sr.material.mainTextureOffset;
    }

    void Update()
    {
        offset.x += scrollSpeed * Time.deltaTime;
        sr.material.mainTextureOffset = offset;
    }
}
