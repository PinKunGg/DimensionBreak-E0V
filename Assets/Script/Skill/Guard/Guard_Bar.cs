using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard_Bar : MonoBehaviour
{
    private Transform startPos;
    private Vector3 startScale;

    private void Start()
    {
        startPos = this.transform;
        startScale = this.transform.localScale;
    }
    void Update()
    {
        this.transform.position = startPos.position;
        this.transform.localScale = startScale;
    }
}
