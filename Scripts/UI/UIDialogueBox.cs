using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class take care of updating the dialogue text, hiding/showing the dialogue box and changing the portrait.
/// It is a singleton so the text/portrait can be changed from anywhere.
/// </summary>
public class UIDialogueBox : MonoBehaviour
{
	// Tek inn dialogue boxið
	public static UIDialogueBox Instance { get; private set; }
	
	// Geri breytur fyrir mynd og mesh fyrir textann
	public Image portrait;
	public TextMeshProUGUI text;

	void Awake()  // Byrja scriptuna með því að ná í hlutinn sem scriptan er á
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		// Slekk á dialogue boxinu á fyrsta ramma
		gameObject.SetActive(false);	
	}


	public void DisplayText(string content)
	{
		text.text = content;  // Tek inn texta og geri hann tilbúinn
	}

	public void DisplayPortrait(Sprite spr)
	{
		portrait.sprite = spr;  // Tek inn sprite og geri það tilbúið
	}

	public void Show()
	{
		gameObject.SetActive(true);  // Get kallað í þetta til að sýna dialogue boxið
	}
	
	public void Hide()
	{
		gameObject.SetActive(false);  // Get kallað í þetta til að fela dialogue boxið
	}
}
