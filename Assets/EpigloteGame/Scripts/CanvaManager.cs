using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvaManager : MonoBehaviour
{
    public TextMeshProUGUI pontuacaoTexto;
    public TextMeshProUGUI tempoTexto;
    public Slider Slider;

    public TextMeshProUGUI textoCentral;
    // Start is called before the first frame update
    void Start()
    {
        textoCentral.gameObject.SetActive(false);
        Slider.maxValue = 50;
    }

    public void Score(int pontuacao)
    {
        pontuacaoTexto.text = pontuacao.ToString();
    }
    public void Time(int tempo)
    {
        Slider.value = tempo;
    }
    public void GameOver(String text)
    {
        textoCentral.text = text;
        textoCentral.gameObject.SetActive(true);
    }
}
