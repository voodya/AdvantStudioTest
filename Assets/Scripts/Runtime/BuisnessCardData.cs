using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuisnessCardData : MonoBehaviour
{
    [Header("Default fields")]
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Slider _progress;
    [SerializeField] private TextMeshProUGUI _targetLevel;
    [SerializeField] private TextMeshProUGUI _income;

    [Space]
    [Header("UpdateBtn")]
    [SerializeField] private Button _upgrade;
    [SerializeField] private TextMeshProUGUI _upgradePrice;

    [Space]
    [Header("Improvement One")]
    [SerializeField] private Button _improvementOne;
    [SerializeField] private TextMeshProUGUI _incomeProcentOne;
    [SerializeField] private TextMeshProUGUI _incomeNameOne;
    [SerializeField] private TextMeshProUGUI _incomePriceOne;

    [Space]
    [Header("Improvement Two")]
    [SerializeField] private Button _improvementTwo;
    [SerializeField] private TextMeshProUGUI _incomeProcentTwo;
    [SerializeField] private TextMeshProUGUI _incomeNameTwo;
    [SerializeField] private TextMeshProUGUI _incomePriceTwo;

    private RuntimeBuisness _thisBuisness;
    private float _targetIncome;
    private bool _isImprovrmrntOneGetted = false;
    private bool _isImprovrmrntTwoGetted = false;
    private int _cost;
    private float _step = 0f;
    private bool _isLoaded = false;
    
    /// <summary>
    /// ������������� �������� �������
    /// </summary>
    /// <param name="target"></param>
    public void BUildTargetCard(RuntimeBuisness target)
    {
        _thisBuisness = target;
        UpdateCardData();

        _name.text = _thisBuisness.Template.Name;
        _progress.value = _thisBuisness.TargetProgress;

        _isImprovrmrntOneGetted = _thisBuisness.IsFirstImprovementGetted;
        _isImprovrmrntTwoGetted = _thisBuisness.IsSecondImprovementGetted;

        _upgrade.onClick.AddListener(BuyUpgrade);

        _incomeNameOne.text = _thisBuisness.Template.ImprovementOne.Name;
        _incomeProcentOne.text = $"�����: {_thisBuisness.Template.ImprovementOne.Factor}%";

        _incomeNameTwo.text = _thisBuisness.Template.ImprovementTwo.Name;
        _incomeProcentTwo.text = $"�����: {_thisBuisness.Template.ImprovementTwo.Factor}%";

        if (!_isImprovrmrntOneGetted)
        {
            _improvementOne.onClick.AddListener(BuyFirstImprovement);
            _incomePriceOne.text = $"���� {_thisBuisness.Template.ImprovementOne.Price}$";
        }
        else
            _incomePriceOne.text = "�������";

        if (!_isImprovrmrntTwoGetted)
        {
            _improvementTwo.onClick.AddListener(BuySecondImprovement);
            _incomePriceTwo.text = $"���� {_thisBuisness.Template.ImprovementTwo.Price}$";
        }
        else
            _incomePriceTwo.text = "�������";


        _step = 1f / (float)_thisBuisness.Template.Delay;
        _isLoaded = true;
    }

    /// <summary>
    /// ���������� ������ ��� ��������
    /// ���������� ��������� �����
    /// </summary>
    private void UpdateCardData()
    {
        _cost = (_thisBuisness.Level + 1) * _thisBuisness.Template.BasePrice;
        _targetIncome = GetTargetIncome();
        _upgradePrice.text = $"����: {_cost}$";
        if (_thisBuisness.Level != 0)
            _income.text = $"{_targetIncome}$";
        else
            _income.text = $"{_thisBuisness.Template.BaseIncome}$";
        _targetLevel.text = _thisBuisness.Level.ToString();
    }

    /// <summary>
    /// ������� ������� ���������
    /// ��� ����� ������� ���-�� ����� ������������ ��� n-��� ���������� ���������, �� �� ���� ���������, �� � � �� ����� ��������� ��� ���������
    /// </summary>
    private void BuyFirstImprovement()
    {
        ProcessController.OnHaveMoney?.Invoke(_thisBuisness.Template.ImprovementOne.Price, call => 
        {
            if(call)
            {
                _progress.value = 0f;
                _isImprovrmrntOneGetted = true;
                _thisBuisness.IsFirstImprovementGetted = true;
                _incomePriceOne.text = "�������";
                UpdateCardData();
            }
        
        });
    }

    private void BuySecondImprovement()
    {
        ProcessController.OnHaveMoney?.Invoke(_thisBuisness.Template.ImprovementTwo.Price, call => 
        {
            if(call)
            {
                _progress.value = 0f;
                _isImprovrmrntTwoGetted = true;
                _thisBuisness.IsSecondImprovementGetted = true;
                _incomePriceTwo.text = "�������";
                UpdateCardData();
            }
        });
    }

    /// <summary>
    /// ������� ��������
    /// </summary>
    private void BuyUpgrade()
    {
        ProcessController.OnHaveMoney?.Invoke(_cost, call => 
        {
            if(call)
            {
                _progress.value = 0f;
                _thisBuisness.Level++;
                UpdateCardData();
            }
            //��� ��� ���� ����� "������������ �������", �� ��� ���� :)
        });
    }

    /// <summary>
    /// ������� �������� ������
    /// </summary>
    /// <returns></returns>
    private float GetTargetIncome()
    {
        float income = _thisBuisness.Level * _thisBuisness.Template.BaseIncome 
            * (1 + ((_thisBuisness.Template.ImprovementOne.Factor / 100f) * (_thisBuisness.IsFirstImprovementGetted ? 1 : 0)) + 
            ((_thisBuisness.Template.ImprovementTwo.Factor / 100f) * (_thisBuisness.IsSecondImprovementGetted ? 1 : 0)));
        return income;
    }

    /// <summary>
    /// ������ ���� (�� ��) ��������� �������� ��������� ������
    /// </summary>
    private void Update()
    {
        if (_thisBuisness.Level == 0 || !_isLoaded) return;

        _progress.value += _step*Time.deltaTime;

        _thisBuisness.TargetProgress = _progress.value; //��������� �������� ��������� ������, � ������ ������� � ��������� ����� � ���������� ������ ��� ������ �� ����������

        if (_progress.value >= 1)
        {
            _progress.value = 0;
            ProcessController.OnAddMoney?.Invoke(_targetIncome); //��������� ������� 
        }
    }

}
