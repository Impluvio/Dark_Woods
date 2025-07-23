using UnityEngine;
using TMPro;

public class PrintPlaneID : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI outputTextField;

    //quick test of the plane id output

    string output = "Output: ";

    public void PrintMessage(string message)
    {
      
        output = output + "\n" + message;
        outputTextField.text = output;


    }

}
