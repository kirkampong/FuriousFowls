using UnityEngine;
using System.Collections;

/// <summary>
/// Your Score.
/// </summary>
public class Score : MonoBehaviour
{
    private static TextMesh storeTextMesh;//text mesh component
    private static string score;//your score
    public static bool basket1Made;
    public static bool basket2Made;
    public static bool basket3Made;

    //Use this for Initialization
    void Start()
    {
        score = "0/3";
        basket1Made = false;
        basket2Made = false;
        basket3Made = false;
        if (storeTextMesh == null)
        {
            storeTextMesh = GetComponent<TextMesh>();
        }
        ApplyScore();
    }

    //apply the score to the text mesh component
    public static void ApplyScore()
    {
        storeTextMesh.text = "" + CalculateScore();
    }


    public static string CalculateScore()
    {
        if (basket1Made && basket2Made && basket3Made) return "3/3";

        else if (!basket1Made && !basket2Made && !basket3Made) return "0/3";

        else if ((basket1Made && basket2Made) ||
                 (basket2Made && basket3Made) ||
                 (basket1Made && basket3Made)) return "2/3";
        return "1/3";
    }

    public void baskt1Made()
    {
        basket1Made = true;
    }
    public void baskt2Made()
    {
        basket2Made = true;
    }
    public void baskt3Made()
    {
        basket3Made = true;
    }
}
