using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentData", menuName = "Data/CurrentData")]
public class CurrentGameConfig : ScriptableObject
{
    public List<RuntimeBuisness> Buisneses;
    public float Balance = 150f;
}

[System.Serializable]
public class RuntimeBuisness
{
    public Buisness Template;
    public int Level;
    public float TargetProgress;
    public bool IsFirstImprovementGetted;
    public bool IsSecondImprovementGetted;

    public RuntimeBuisness(Buisness temp, int counter)
    {
        Template = temp;
        if(counter == 0) //первый бизнес куплен (при первом запуске)
        Level = 1;
        else
            Level = 0;
        TargetProgress = 0;
        IsFirstImprovementGetted = false;
        IsSecondImprovementGetted = false;
    }

    public RuntimeBuisness()
    {

    }
}

