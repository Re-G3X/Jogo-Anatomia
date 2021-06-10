﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarBhv : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(resetScrollPos());
    }

    IEnumerator resetScrollPos()
    {
        yield return null; // Waiting just one frame is probably good enough, yield return null does that
        gameObject.GetComponent<Scrollbar>().value = 1;
    }
}
