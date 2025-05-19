using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    public Image damageImage;

    //esto es la opacidad maxima que va a tener
    public float maxAlpha = 0.6f;
    //velocidad a la que desaparece
    public float speed = 1f;
    private float currentAlpha = 0f;

    void Update()
    {
        if(currentAlpha > 0){
            currentAlpha -= speed * Time.deltaTime;
            currentAlpha = Mathf.Clamp(currentAlpha, 0f, maxAlpha);
            SetAlpha(currentAlpha);
        }
    }

    public void AddDamageEffect(float amount)
    {
        currentAlpha += amount;
        currentAlpha = Mathf.Clamp(currentAlpha, 0f, maxAlpha);
        SetAlpha(currentAlpha);
    }

    private void SetAlpha(float alpha)
    {
        Color c = damageImage.color;
        c.a = alpha;
        damageImage.color = c;
    }
}
