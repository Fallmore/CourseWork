using System;
using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneralSettings : MonoBehaviour
{
    [SerializeField] List<int> _arr;
    [SerializeField] HoarAndInsertion _sorting;
    [SerializeField] Graph _graphContainer;
    [SerializeField] TMP_Dropdown _size;
    [SerializeField] TMP_Dropdown _type;
    [SerializeField] TMP_Dropdown _countTests;
    [SerializeField] TMP_Dropdown _typeSort;
    [SerializeField] Slider sortSpeed;
    [SerializeField] Button _sortb;
    [SerializeField] Button _refreshb;
    [SerializeField] Button _stopb;
    [SerializeField] Button _clearb;
    [SerializeField] Button _testb;
    [SerializeField] Button _resultb;
    [SerializeField] TextMeshProUGUI _timeText;
    [SerializeField] TextMeshProUGUI _permutationText;
    [SerializeField] public TextMeshProUGUI WaitText;
    [SerializeField] Toggle _realTime;

    [SerializeField] int _sizeMas;
    [SerializeField] int _countSort = 0;
    [SerializeField] int _minCountPermutation = int.MaxValue;
    [SerializeField] int _maxCountPermutation = 0;
    [SerializeField] int _totalCountPermutation = 0;
    [SerializeField] double _totalEndTimeSort = 0;
    [SerializeField] double _minEndTimeSort = double.MaxValue;
    [SerializeField] double _maxEndTimeSort = 0d;
    [SerializeField] string _statistic;

    [SerializeField] public bool StopSort = false;
    [SerializeField] public int CountPermutation;
    [SerializeField] public double EndTimeSort;

    private void Start()
    {
        sortSpeed.gameObject.SetActive(_realTime.isOn);
        _stopb.enabled = false;
        _clearb.enabled = false;
    }

    public void UpdateTime()
    {
        _sorting.TimeEditGraph = 1 - sortSpeed.value;
    }

    public void UpdateAll()
    {
        UpdateSize();
        UpdateType();
        UpdateGraph();
    }

    public void UpdateSize()
    {
        _sizeMas = int.Parse(_size.options[_size.value].text);
    }

    public void UpdateType()
    {
        _arr.Clear();
        int i = _type.value;
        switch (i)
        {
            case 0:
                IncreasingRow();
                break;
            case 1:
                DecreasingRow();
                break;
            case 2:
                RandomRow();
                break;
            default:
                break;
        }
    }

    public void UpdateGraph()
    {
        int cnt = _graphContainer.gameObject.transform.childCount;
        for (int i = 0; i < cnt; ++i)
        {
            Destroy(_graphContainer.gameObject.transform.GetChild(i).gameObject);
        }
        _graphContainer.ShowGraph(_arr, _sizeMas);
    }

    public void EditGraph(int lindex, int rindex)
    {
        _graphContainer.EditGraph(lindex, rindex);
    }

    private void IncreasingRow()
    {
        for (int i = 1; i < _sizeMas + 1; ++i)
        {
            _arr.Add(i);
        }
    }

    private void DecreasingRow()
    {
        for (int i = 1; i < _sizeMas + 1; ++i)
        {
            _arr.Add(_sizeMas + 1 - i);
        }
    }

    private void RandomRow()
    {
        for (int i = 0; i < _sizeMas; ++i)
        {
            _arr.Add(UnityEngine.Random.Range(-_sizeMas, _sizeMas));
        }
    }

    public void StopSorting()
    {
        StopSort = true;
    }

    public void CallSort()
    {
        if (_typeSort.value == 0) CallHoar();
        else CallInsertion();
    }
    private void CallHoar()
    {
        if (_realTime.isOn)
            StartCoroutine(_sorting.QuickSortHoaraRealTime(_arr));
        else _sorting.QuickSortHoara(_arr);
    }
    private void CallInsertion()
    {
        if (_realTime.isOn)
            StartCoroutine(_sorting.StartInsertionSortRealTime(_arr));
        else _sorting.InsertionSort(_arr);
    }

    public void SetRealTimeSort()
    {
        _stopb.gameObject.SetActive(_realTime.isOn);
        sortSpeed.gameObject.SetActive(_realTime.isOn);
        _testb.enabled = _countTests.enabled = !_realTime.isOn;
    }

    private void UpdateStats()
    {
        ++_countSort;
        _totalCountPermutation += CountPermutation;
        _totalEndTimeSort += EndTimeSort;
        if (_maxEndTimeSort < EndTimeSort) _maxEndTimeSort = EndTimeSort;
        if (_minEndTimeSort > EndTimeSort) _minEndTimeSort = EndTimeSort;
        if (_maxCountPermutation < CountPermutation) _maxCountPermutation = CountPermutation;
        if (_minCountPermutation > CountPermutation) _minCountPermutation = CountPermutation;
    }

    private void ResetStats()
    {
        _countSort = _totalCountPermutation = CountPermutation = _maxCountPermutation = 0;
        _minEndTimeSort = double.MaxValue;
        _maxEndTimeSort = _totalEndTimeSort = 0d;
        _minCountPermutation = int.MaxValue;
    }

    private string UnitOfTime()
    {
        if (_realTime.isOn) return " сек.\n";
        else return " нс.\n";
    }

    public void EditStatistic()
    {
        _clearb.enabled = true;

        if (!StopSort)
        {
            UpdateStats();

            _permutationText.text = "Перестановки: " + CountPermutation;
            if (_realTime.isOn) _timeText.text = "Время: " + (float)EndTimeSort + UnitOfTime()
                + "Среднее: " + (float)_totalEndTimeSort / _countSort + UnitOfTime()
                + "Минимальное: " + (float)_minEndTimeSort + UnitOfTime()
                + "Максимальное: " + (float)_maxEndTimeSort + UnitOfTime();
            else _timeText.text = "Время: " + EndTimeSort + UnitOfTime()
                + "Среднее: " + _totalEndTimeSort / _countSort + UnitOfTime()
                + "Минимальное: " + _minEndTimeSort + UnitOfTime()
                + "Максимальное: " + _maxEndTimeSort + UnitOfTime();
        }
        else
        {
            _permutationText.text = "Перестановки: " + CountPermutation;
            if (_realTime.isOn) _timeText.text = "Время: " + (float)EndTimeSort + UnitOfTime() + "Среднее: Н/Д\nМинимальное: Н/Д\nМаксимальное: Н/Д";
            else _timeText.text = "Время: " + EndTimeSort + UnitOfTime() + "Среднее: Н/Д\nМинимальное: Н/Д\nМаксимальное: Н/Д";
        }
        CountPermutation = 0;
    }

    public void ResetStatistic()
    {
        _permutationText.text = "Перестановки: 0";
        _timeText.text = "Время: Н/Д\nСреднее: Н/Д\nМинимальное: Н/Д\nМаксимальное: Н/Д";

        ResetStats();
    }

    public void AvailableButtons()
    {
        _sortb.enabled = true;
        _refreshb.enabled = true;
        _size.enabled = true;
        _type.enabled = true;
        _resultb.enabled = true;
        _typeSort.enabled = true;
        _stopb.enabled = false;
    }

    public void DisableButtons()
    {
        _sortb.enabled = false;
        _typeSort.enabled = false;
        _resultb.enabled = false;
        _refreshb.enabled = false;
        _size.enabled = false;
        _type.enabled = false;
        _stopb.enabled = true;
    }

    private void ClearStatisticFile()
    {
        File.WriteAllText("C:/Users/" + Environment.UserName + "/Desktop/Statistic.txt", _typeSort.options[_typeSort.value].text + " " + DateTime.Now);
    }

    private void WriteStatistic(int i)
    {
        _statistic = "\n" + _sizeMas + " " + _type.options[i].text + "\nВсего времени: " + _totalEndTimeSort + UnitOfTime()
        + "Среднее: " + (_totalEndTimeSort / _countSort) + UnitOfTime()
        + "Минимальное: " + _minEndTimeSort + UnitOfTime()
        + "Максимальное: " + _maxEndTimeSort + UnitOfTime()
        + "\nВсего перестановок: " + _totalCountPermutation + "\nМинимальное: " + _minCountPermutation + "\nМаксимальное: " + _maxCountPermutation
        + "\n\n=============================================\n";
    }

    public void WriteStatisticFile(int i)
    {
        WriteStatistic(i);
        File.AppendAllText("C:/Users/" + Environment.UserName + "/Desktop/Statistic.txt", _statistic);
    }

    public void StartTest()
    {
        ClearStatisticFile();

        if (_typeSort.value == 0) TestHoar();
        else TestInsertion();

        Results();
        ResetStatistic();
    }

    public void Results()
    {
        System.Diagnostics.Process.Start("C:/Users/" + Environment.UserName + "/Desktop/Statistic.txt");
    }

    private void UpdateSize(int i)
    {
        switch (i)
        {
            case 0:
                _sizeMas = 10;
                break;
            case 1:
                _sizeMas = 100;
                break;
            case 2:
                _sizeMas = 1000;
                break;
            case 3:
                _sizeMas = 10000;
                break;
            case 4:
                _sizeMas = 100000;
                break;
            case 5:
                _sizeMas = 1000000;
                break;
            default:
                break;
        }
    }

    private void UpdateType(int i)
    {
        _arr.Clear();
        switch (i)
        {
            case 0:
                IncreasingRow();
                break;
            case 1:
                DecreasingRow();
                break;
            case 2:
                RandomRow();
                break;
            default:
                break;
        }
    }

    private void TestHoar()
    {
        for (int i = 0; i < 6; ++i)
        {
            UpdateSize(i);
            for (int j = 0; j < 3; ++j)
            {
                ResetStats();
                for (int k = 0; k < int.Parse(_countTests.options[_countTests.value].text); ++k)
                {
                    UpdateType(j);
                    _sorting.QuickSortHoaraForTests(_arr);
                    UpdateStats();
                    CountPermutation = 0;
                }
                WriteStatisticFile(j);
            }
        }
    }

    private void TestInsertion()
    {
        for (int i = 0; i < 4; ++i)
        {
            UpdateSize(i);
            for (int j = 0; j < 3; ++j)
            {
                ResetStats();
                for (int k = 0; k < int.Parse(_countTests.options[_countTests.value].text); ++k)
                {
                    UpdateType(j);
                    _sorting.InsertionSortForTests(_arr);
                    EditStatistic();
                }
                WriteStatisticFile(j);
            }
        }
    }

    public void OpenMe()
    {
        System.Diagnostics.Process.Start("https://github.com/Fallmore");
    }
}