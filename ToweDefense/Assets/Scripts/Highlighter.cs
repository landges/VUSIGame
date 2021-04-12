using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider))]
public class Highlighter : MonoBehaviour
{
    // 2
    // reference to SpriteRenderer component
    private SpriteRenderer SpriteRenderer;

    [SerializeField]
    private Material originalMaterial;

    [SerializeField]
    private Material highlightedMaterial;

    void Start()
    {
        // 3
        // cache a reference to the SpriteRenderer
        SpriteRenderer = GetComponent<SpriteRenderer>();

        // 4
        // use non-highlighted material by default
        EnableHighlight(false);
    }

    // toggle betweeen the original and highlighted materials
    public void EnableHighlight(bool onOff)
    {
        // 5
        if (SpriteRenderer != null && originalMaterial != null &&
            highlightedMaterial != null)
        {
            // 6
            SpriteRenderer.material = onOff ? highlightedMaterial : originalMaterial;
        }
    }
    private void OnMouseOver()
    {
        EnableHighlight(true);
    }

    private void OnMouseExit()
    {
        EnableHighlight(false);
    }

}
