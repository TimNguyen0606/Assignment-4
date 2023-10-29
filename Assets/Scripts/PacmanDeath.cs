using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanDeath : MonoBehaviour
{
    private AnimatedSprite animatedSprite;
    [SerializeField]
    private float disappearTime;

    private void Awake()
    {
        animatedSprite = GetComponent<AnimatedSprite>();
    }

    private void OnEnable()
    {
        animatedSprite.Restart();
        StartCoroutine(DisableAfterTime(disappearTime));
    }

    IEnumerator DisableAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
