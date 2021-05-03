using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
	// Næ í healht barinn
	public static UIHealthBar Instance { get; private set; }

	// Geri breytu sem heldur utan um mynd
	public Image bar;

	// Geri float breytu
	float originalSize;

	// Byrja instance sem hluturinn sem scriptan er á
	void Awake ()
	{
		Instance = this;
	}

	void OnEnable()
	{
		// Tek inn upprunalegu stærðina
		originalSize = bar.rectTransform.rect.width;
	}

	public void SetValue(float value)
	{		
		// Útreikningar til þess að minnka stærð health barins samkvæmt lífi leikmanns
		bar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
	}
}
