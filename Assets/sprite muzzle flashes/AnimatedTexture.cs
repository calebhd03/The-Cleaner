using UnityEngine;
using System.Collections;

public class AnimatedTexture : MonoBehaviour
{
    public float fps = 30.0f;
    public Sprite[] frames;

    private int frameIndex;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        NextFrame();
        InvokeRepeating("NextFrame", 1 / fps, 1 / fps);
    }

    void NextFrame()
    {
        sr.sprite = frames[frameIndex];
        frameIndex = (frameIndex + 1) % frames.Length;
    }
}