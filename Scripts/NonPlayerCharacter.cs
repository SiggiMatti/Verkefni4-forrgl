using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public float displayTime = 4.0f;  // tími hversu langt textinn kemur á skjáinn
    public GameObject dialogBox;  // Boxið sem heldur um textann
    float timerDisplay;  // Sýnir tíma sem að textinn er
    
    void Start()
    {
        dialogBox.SetActive(false);  // Læt boxið hverfa
        timerDisplay = -1.0f;  // Læt tíma boxins á núll
    }

    void Update()
    {
        if (timerDisplay >= 0)  // Ef að er verið að sýna textann
        {
            timerDisplay -= Time.deltaTime;  // Þá byrjar að telja niður með því að mínusa delta time frá gildinu sem ég lét í timerdisplay breytuna
            if (timerDisplay < 0)  // Ef að tíminn er orðinn minni en 0
            {
                dialogBox.SetActive(false);  // Læt textann hverfa
            }
        }
    }
    
    public void DisplayDialog()
    {
        timerDisplay = displayTime;  // Læt displayTime vera sami og timerDisplay
        dialogBox.SetActive(true);  // Læt textaboxið í gang
    }
}
