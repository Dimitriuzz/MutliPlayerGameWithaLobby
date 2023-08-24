using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusPanel : MonoBehaviour
{
    [SerializeField] public TMP_Text coinsText;
    public int coinsNumber=0;

    // Update is called once per frame
    void Update()
    {
        coinsText.text = coinsNumber.ToString();
    }
}
