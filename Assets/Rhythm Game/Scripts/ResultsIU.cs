using UnityEngine;
using TMPro;

public class ResultsUI : MonoBehaviour
{
    public TMP_Text titleText;     
    public TMP_Text rankText;
    public TMP_Text scoreText;
    public TMP_Text accuracyText;
    void Start()
    {
        SetResults(
            GameData.score,
            GameData.perfect,
            GameData.good,
            GameData.miss
        );
    }

    public void SetResults(int score, int perfect, int good, int miss)
    {
        int total = perfect + good + miss;

        float accuracy = 0f;
        if (total > 0)
        {
            accuracy = ((perfect * 1f + good * 0.5f) / total) * 100f;
        }

        string rank = GetRank(accuracy);
        string feedback = GetFeedback(rank);

        // 👇 agora o título é o feedback
        titleText.text = feedback;

        rankText.text = "Rank " + rank;
        scoreText.text = "Score: " + score;
        accuracyText.text = "Precisão: " + accuracy.ToString("F1") + "%";
    }

    string GetRank(float accuracy)
    {
        if (accuracy >= 95f) return "S";
        if (accuracy >= 85f) return "A";
        if (accuracy >= 70f) return "B";
        if (accuracy >= 50f) return "C";
        return "D";
    }

    string GetFeedback(string rank)
    {
        switch (rank)
        {
            case "S": return "Perfeito!";
            case "A": return "Excelente!";
            case "B": return "Muito bom!";
            case "C": return "Bom!";
            default: return "Tente novamente!";
        }
    }
}