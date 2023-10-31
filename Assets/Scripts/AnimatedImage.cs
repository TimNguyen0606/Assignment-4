using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// class animated image 
public class AnimatedImage : MonoBehaviour
{
    public Image sr { get; private set; }
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private float animationTime = 0.25f;
    public int animationFrame { get; private set; }
    [SerializeField]
    private bool loop = true;

    private void Awake()
    {
        sr = GetComponent<Image>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    private void Advance() // increment frame
    {
        if (!sr.enabled) // if image not enabled
        {
            return;
        }

        animationFrame++; // increment frame

        if (animationFrame >= sprites.Length && loop) // go back to 0
        {
            animationFrame = 0;
        }

        if (animationFrame >= 0 && animationFrame < sprites.Length) // if frame valid, update sprite
        {
            sr.sprite = sprites[animationFrame];
        }
    }

    public void Restart() // starts animation on the game 
    {
        animationFrame = -1;

        Advance();
    }
}
