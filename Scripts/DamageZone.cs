using UnityEngine;
// Þessi klasi passar að leikmaður missir líf ef að hann er kyrr inni í trigger sem lætur hann missa líf
public class DamageZone : MonoBehaviour 
{
    void OnTriggerStay2D(Collider2D other)  // Þegar að leikmaður collidar við þetta
    {
        RubyController controller = other.GetComponent<RubyController>();  // Næ í scriptuna sem stjórnar Ruby

        if (controller != null)  // Ef að þetta er scriptan sem stjórnar Ruby
        {
            // RubyController scriptan sér um að láta hana missa líf
            controller.ChangeHealth(-1);
        }
    }
}
