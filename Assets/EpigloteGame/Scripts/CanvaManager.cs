using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvaManager : MonoBehaviour
{
    public TextMeshProUGUI pontuacaoTexto;
    // Start is called before the first frame update
    void Start()
    {
        
    }

     public void Score(int pontuacao)
    {
        pontuacaoTexto.text = pontuacao.ToString();
    }
}
