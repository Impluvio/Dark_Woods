using UnityEngine;
using TMPro;

public class PrintPlaneID : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI outputTextField;

    //quick test of the plane id output

    public void PrintMessage(string message)
    {
        outputTextField.text = message;
    }

}
