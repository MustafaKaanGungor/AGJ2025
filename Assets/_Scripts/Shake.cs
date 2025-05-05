
using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public static Shake Instance {get; private set;}
    public bool start = false;
    public AnimationCurve animationCurve;
    public float duration = 0.2f;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        if(start) {
            start = false;
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking()
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0;

        while(elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float strength = animationCurve.Evaluate(elapsedTime/duration);
            transform.position = startPos + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPos;
    }
}
