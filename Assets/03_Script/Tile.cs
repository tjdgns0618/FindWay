using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] GameObject _block;

    public void Init(Color color, bool isBlock)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = color;

        _block.SetActive(isBlock);
    }
}
