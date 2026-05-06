using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text scoreText;

    public GameObject roundOverGreen;

    private void Start()
    {
        roundOverGreen.SetActive(false);
    }
}
