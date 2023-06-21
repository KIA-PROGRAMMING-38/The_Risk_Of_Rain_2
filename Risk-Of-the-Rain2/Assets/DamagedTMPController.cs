using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class DamagedTMPController : MonoBehaviour
{

    public TMP_Text tmpText;
    public Color fadeColor;
    public Vector3 offset;
    Material mat;
    void Start()
    {

        mat = tmpText.fontMaterial;


    }
    public float movingSpeed;
    public float dropingSpeed;
    public float duration;
    private float elapsedTime;
    public float alphaDropSpeed;
    // Update is called once per frame
    void Update()
    {
        MoveUp();
        ChangeAlpha();

        if (elapsedTime > duration - 0.3f)
        {
            PlayDestoryAnimation();
        }
        if (elapsedTime > duration)
        {
            Destroy();
        }
    }

    private void OnEnable()
    {
        mat = tmpText.fontMaterial;
        //////transform.position += offset;
    }
    private void MoveUp()
    {
        transform.position += (Vector3.up + Vector3.forward).normalized * movingSpeed;
    }
    private void ChangeAlpha()
    {

        elapsedTime += Time.deltaTime * alphaDropSpeed;


        Color currentColor = mat.GetColor("_FaceColor");
        currentColor.a = Mathf.Lerp(0, 1, 1 - elapsedTime / duration);

        mat.SetColor("_FaceColor", currentColor);

        mat.color = currentColor;
    }

    CancellationTokenSource cts = new();
    private void PlayDestoryAnimation()
    {
        transform.position += Vector3.down * dropingSpeed;
    }

    private void Destroy()
    {
        DestroyObject(gameObject);

    }


}
