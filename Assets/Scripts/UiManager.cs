using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    [SerializeField] private GameObject queryPlacementPopUp;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [SerializeField] private PlaneSelector planeSelector;


    public bool? confirmPlacement = null;

    void Awake()
    {
        yesButton.onClick.AddListener(() => togglePlacement(true));
        noButton.onClick.AddListener(() => togglePlacement(false));
    }

    public void displayQueryPlacement(bool isOn) //turns ui element on.
    {
        queryPlacementPopUp.SetActive(isOn);
    }

    public void togglePlacement(bool toggle)
    {
        planeSelector.setPlayArea(toggle);
    }


}
