using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake_Original : MonoBehaviour
{
    public static CamShake_Original instance;
    private void Awake()
    {
        if(instance != null)
        {
            print("Bruh");
        }
        else
        {
            instance = this;
        }
    }
    public IEnumerator ShakeOrigin(float duration, float magniture)
    {
        Vector2 originPoint = transform.localPosition;

        float elapes = 0.0f;

        while(elapes < duration)
        {
            float x = Random.Range(-1f,1f) * magniture;
            float y = Random.Range(-1f,1f) * magniture;

            transform.localPosition = new Vector2(x,y);

            elapes += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originPoint;
    }
}
