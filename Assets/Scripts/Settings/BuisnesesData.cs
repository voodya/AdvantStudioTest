using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buisneses", menuName = "Data/Buisneses")]
public class BuisnesesData : ScriptableObject
{
    public List<Buisness> Buisneses;
}

[System.Serializable]
public class Buisness
{
    public string Name;
    public int Delay;
    public int BasePrice;
    public int BaseIncome;
    public Improvement ImprovementOne;
    public Improvement ImprovementTwo;
}

[System.Serializable]
public class Improvement
{
    public int Price;
    public int Factor;
    public string Name;
}


