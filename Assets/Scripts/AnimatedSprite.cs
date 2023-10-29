using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public SpriteRenderer sr { get; private set; }
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private float animationTime = 0.25f;
    public int animationFrame { get; private set; }
    [SerializeField]
    private bool loop = true;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    private void Advance() // increment frame
    {
        if (!sr.enabled) // if spriterenderer not enabled, return
        {
            return;
        }

        animationFrame++; // increment frame

        if (animationFrame >= sprites.Length && loop) // if frame out of range go back to 0
        {
            animationFrame = 0;
        }

        if (animationFrame >= 0 && animationFrame < sprites.Length) // if frame valid, update sprite
        {
            sr.sprite = sprites[animationFrame];
        }
    }

    public void Restart() // starts animation over from first frame
    {
        animationFrame = -1;

        Advance();
    }
}
