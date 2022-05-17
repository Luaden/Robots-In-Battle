using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BaseUIElement<T> : MonoBehaviour
{
    public abstract void UpdateUI(T primaryData);

    protected abstract bool ClearedIfEmpty(T newData);


    protected void UpdateText(Text target, string text) { target.text = text; }
    protected void UpdateText(TMP_Text target, string text) { target.text = text; }

    protected void UpdateSprite(Image image, Sprite sprite) { image.sprite = sprite; }
    protected void UpdateNumericText(Text target, string textformatting, float value) { UpdateText(target, string.Format(textformatting, value)); }
    protected void UpdateNumericText(TMP_Text target, string textformatting, float value) { UpdateText(target, string.Format(textformatting, value)); }
    protected void SetPercentage(Image target, float percent) { target.fillAmount = percent; }
}

public abstract class BaseUIElement<T, U> : MonoBehaviour
{
    public abstract void UpdateUI(T primaryData, U secondaryData);

    protected abstract bool ClearedIfEmpty(T newData, U secondaryData);


    protected void UpdateText(Text target, string text) { target.text = text; }
    protected void UpdateText(TMP_Text target, string text) { target.text = text; }

    protected void UpdateSprite(Image image, Sprite sprite) { image.sprite = sprite; }
    protected void UpdateNumericText(Text target, string textformatting, float value) { UpdateText(target, string.Format(textformatting, value)); }
    protected void UpdateNumericText(TMP_Text target, string textformatting, float value) { UpdateText(target, string.Format(textformatting, value)); }
    protected void SetPercentage(Image target, float percent) { target.fillAmount = percent; }
}

public abstract class BaseUIElement<T, U, V> : MonoBehaviour
{
    public abstract void UpdateUI(T primaryData, U secondaryData, V tertiaryData);

    protected abstract bool ClearedIfEmpty(T newData, U secondaryData, V tertiaryData);


    protected void UpdateText(Text target, string text) { target.text = text; }
    protected void UpdateText(TMP_Text target, string text) { target.text = text; }

    protected void UpdateSprite(Image image, Sprite sprite) { image.sprite = sprite; }
    protected void UpdateNumericText(Text target, string textformatting, float value) { UpdateText(target, string.Format(textformatting, value)); }
    protected void UpdateNumericText(TMP_Text target, string textformatting, float value) { UpdateText(target, string.Format(textformatting, value)); }
    protected void SetPercentage(Image target, float percent) { target.fillAmount = percent; }
}
