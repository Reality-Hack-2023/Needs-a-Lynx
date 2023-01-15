using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textDebug;

    [SerializeField] GameObject[] _animals;
    int _indexCurrentAnimal = 0;


    // Start is called before the first frame update
    void Start()
    {
        HideAllAnimals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  

    /**
     * HAND DETECT METHODS
     */
    // Left Palm
    public void InteractionLeftPalmIsUp()
    {
        _textDebug.text = "Left Palm Up";
    }

    public void InteractionLeftPalmIsDown()
    {
        _textDebug.text = "Left Palm Down";
    }

    public void InteractionLeftPalmIsLeft()
    {
        _textDebug.text = "Left Palm Is Left";
    }

    public void InteractionLeftPalmIsRight()
    {
        _textDebug.text = "Left Palm Is Right";
    }
    // Right Palm
    public void InteractionRightPalmIsUp()
    {
        _textDebug.text = "Right Palm Up";
    }

    public void InteractionRightPalmIsDown()
    {
        _textDebug.text = "Right Palm Down";
    }

    public void InteractionRightPalmIsLeft()
    {
        _textDebug.text = "Right Palm Left";
    }

    public void InteractionRightPalmIsRight()
    {
        _textDebug.text = "Right Palm Right";
    }


    // Finger Positions
    public void Idle()
    {

    }

    public void InteractPointPosition()
    {
        _textDebug.text = "Finger Pose: Point";
    }

    public void InteractPositionRockNRollP()
    {
        _textDebug.text = "Finger Pose: Rock N Roll";
    }

    public void InteractGunPosition()
    {
        _textDebug.text = " Finger Pose: Shoot Gun";
    }

    public void InteractPeacePosition()
    {
        _textDebug.text = "Finger Pose: Peace";
    }

    public void Interact3FingersPosition()
    {
        SwapAnimal(); // Update Animal Shown
    }

    public void Interact4FingersPosition()
    {
        _textDebug.text = "Finger Pose: 4 Fingers Up";
    }

    public void InteractFUPosition()
    {
        _textDebug.text = "Finger Pose: F*CK U!";
    }


    /**
     * ANIMAL INTERACTIONS
     */
    public void SwapAnimal()
    {
        _animals[_indexCurrentAnimal].SetActive(false); // Hide current animal
        _indexCurrentAnimal++;

        _indexCurrentAnimal = _indexCurrentAnimal == _animals.Length - 1 // Keep the animal index valid
            ? 0 : _indexCurrentAnimal;

        _animals[_indexCurrentAnimal].SetActive(true); // Show current animal
        _textDebug.text = $"New Animal: {_animals[_indexCurrentAnimal].name}";
    }

    public void HideAllAnimals()
    {
        foreach (var animal in _animals)
            animal.SetActive(false);
    }
}
