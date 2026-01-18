using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneFader : MonoBehaviour
{
    public Image img;
    public float speed=1f;
    public AnimationCurve curve;

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(FadeIn(1.5f));
    }
    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));

    }

    IEnumerator FadeIn(float waitTime)
    {
        img.color = new Color(0f, 0f, 0f, 255f);
        yield return new WaitForSecondsRealtime(waitTime);
        float t = 1f;
        while(t > 0f)
        {
            t -= Time.unscaledDeltaTime*speed;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
        if(Time.timeScale == 0f)
        {
            //Time.timeScale = 1f;
        }

        Debug.Log(Time.timeScale + "  :1");
    }
    IEnumerator FadeOut(string scene)
    {
        Debug.Log(Time.timeScale);
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * speed;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
        SceneManager.LoadScene(scene);
    }

}
