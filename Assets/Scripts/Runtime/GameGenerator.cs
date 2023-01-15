using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class GameGenerator : MonoBehaviour
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private BuisnessCardData _prefab;
    [SerializeField] private CurrentGameConfig _gameConfig;
    [SerializeField] private BuisnesesData _buisnessConfig;
 

    private string _path;

    private async void Start()
    {
        Application.targetFrameRate = 60;
        
        _path = Application.persistentDataPath + "/TargetGameConfig.json"; //���� � ���������������� �������

        if (File.Exists(_path))
        {
            string json = await File.ReadAllTextAsync(_path);
            var sd = JsonConvert.DeserializeObject<CurrentGameConfig>(json);
            _gameConfig.Balance = sd.Balance;
            _gameConfig.Buisneses = sd.Buisneses;
            // ���� ����� �������� �������������� ���������� Tamplate �� ������� � ���������,
            // ����� ����� ���� ������� ��������� � ����������� ������ � ��� ������ �� ������� �������
            if(_gameConfig.Buisneses.Count < _buisnessConfig.Buisneses.Count) //��������� ����� �������, ���� � ������� �� ����� ������
            {
                for (int i = _gameConfig.Buisneses.Count-1; i < _buisnessConfig.Buisneses.Count; i++)
                {
                    _gameConfig.Buisneses.Add(new RuntimeBuisness(_buisnessConfig.Buisneses[i], i));
                }
            }
        }
        else
        {
            Debug.Log("File Not Exist");
            _gameConfig.Balance = 150;
            _gameConfig.Buisneses = new List<RuntimeBuisness>();
            int counter = 0;
            foreach(Buisness TargetBuisness in _buisnessConfig.Buisneses) //����������� ������ �� ������� � ��������� � ������� ������
            {
                _gameConfig.Buisneses.Add(new RuntimeBuisness(TargetBuisness, counter));
                counter++;
            }
        }
        ProcessController.OnSetMoney?.Invoke(); //���������� �������� �������
        if (_gameConfig.Buisneses.Count != 0)
            Generate();
    }

    /// <summary>
    /// ��������� �������� ��������
    /// </summary>
    private void Generate() 
    {
        foreach (RuntimeBuisness TargetRuntimeBuisness in _gameConfig.Buisneses)
        {
            Instantiate(_prefab, _container).BUildTargetCard(TargetRuntimeBuisness);
        }
    }

    /// <summary>
    /// ���������� ��������� ��� ������
    /// </summary>
    /// <param name="focus"></param>
    private void OnApplicationFocus(bool focus) 
    {
        if (!focus)
        {
            string json = JsonConvert.SerializeObject(_gameConfig, Formatting.Indented);
            File.WriteAllText(_path, json);
        }
    }

#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        string json = JsonConvert.SerializeObject(_gameConfig, Formatting.Indented);
        File.WriteAllText(_path, json);
        Debug.Log(Path.GetDirectoryName(_path)); //��� ������ ���� � ������, ���� �� ������ �� ������
    }
#endif

}
