using UnityEngine;
using System;
using TMPro;

public class ProcessController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _balance;
    [SerializeField] private CurrentGameConfig _config;

    public static Action<float, Action<bool>> OnHaveMoney;
    public static Action<float> OnAddMoney;
    public static Action OnSetMoney;

    private void Awake()
    {
        OnAddMoney += AddMoney;
        OnSetMoney += ShowTargetBalance;
        OnHaveMoney += CheckMoney;
        // ������������ ������ ���
    }

    /// <summary>
    /// ���������� ��������� "�������"
    /// </summary>
    /// <param name="Price"></param>
    /// <param name="Callback"></param>
    private void CheckMoney(float Price, Action<bool> Callback)
    {
        if (Price < _config.Balance)
        {
            _config.Balance -= Price;
            ShowTargetBalance();
            Callback.Invoke(true);
        }
        else
            Callback.Invoke(false);
    }

    /// <summary>
    /// ��������� ������� � �������
    /// </summary>
    /// <param name="obj"></param>
    private void AddMoney(float obj)
    {
        _config.Balance += obj;
        ShowTargetBalance();
    }

    /// <summary>
    /// ��������� ��������� �������
    /// </summary>
    private void ShowTargetBalance()
    {
        _balance.text = $"������: {_config.Balance}$";
    }
}
