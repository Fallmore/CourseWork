using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HoarAndInsertion : MonoBehaviour
{
    [SerializeField] GeneralSettings _general;
    public float TimeEditGraph;

    private void EndSorting()
    {
        _general.EditStatistic();
        _general.WaitText.enabled = false;
        _general.StopSort = false;
    }

    private int PartOfSortHoaraRealTime(List<int> arr, int left, int right)
    {
        int pivot = arr[(left + right) / 2];
        while (left <= right)
        {
            while (arr[left] < pivot) ++left;
            while (arr[right] > pivot) --right;

            if (left <= right)
            {
                (arr[right], arr[left]) = (arr[left], arr[right]);
                _general.EditGraph(left, right);
                if (left != right) ++_general.CountPermutation;
                ++left;
                --right;
            }
        }
        return left;
    }
    private IEnumerator QuickSortHoaraRealTime(List<int> arr, int start, int end, float time)
    {
        if (start >= end || _general.StopSort) yield break;

        yield return new WaitForSecondsRealtime(TimeEditGraph);

        int rightStart = PartOfSortHoaraRealTime(arr, start, end);
        yield return StartCoroutine(QuickSortHoaraRealTime(arr, start, rightStart - 1, time));
        yield return StartCoroutine(QuickSortHoaraRealTime(arr, rightStart, end, time));
    }
    public IEnumerator QuickSortHoaraRealTime(List<int> arr)
    {
        _general.DisableButtons();
        _general.WaitText.enabled = true;
        float time = Time.realtimeSinceStartup;
        yield return StartCoroutine(QuickSortHoaraRealTime(arr, 0, arr.Count - 1, time));
        _general.EndTimeSort = Time.realtimeSinceStartup - time;
        _general.AvailableButtons();
        EndSorting();
    }




    private int PartOfSortHoara(List<int> arr, int left, int right)
    {
        int pivot = arr[(left + right) / 2];
        while (left <= right)
        {
            while (arr[left] < pivot) ++left;
            while (arr[right] > pivot) --right;

            if (left <= right)
            {
                (arr[right], arr[left]) = (arr[left], arr[right]);
                if (left != right) ++_general.CountPermutation;
                ++left;
                --right;
            }
        }
        return left;
    }
    private void QuickSortHoara(List<int> arr, int start, int end)
    {
        if (start >= end || _general.StopSort) return;

        int rightStart = PartOfSortHoara(arr, start, end);
        QuickSortHoara(arr, start, rightStart - 1);
        QuickSortHoara(arr, rightStart, end);
    }
    public void QuickSortHoara(List<int> arr)
    {
        QuickSortHoaraForTests(arr);
        EndSorting();
        _general.UpdateGraph();
    }
    public void QuickSortHoaraForTests(List<int> arr)
    {
        double time = Time.realtimeSinceStartupAsDouble;
        QuickSortHoara(arr, 0, arr.Count - 1);
        _general.EndTimeSort = (int)((Time.realtimeSinceStartupAsDouble - time) * math.pow(10, 9));
    }











    private IEnumerator InsertionSortRealTime(List<int> arr)
    {

        for (int i = 1; i < arr.Count; ++i)
        {
            int k = arr[i];
            int j = i - 1;

            while (j >= 0 && arr[j] > k)
            {
                if (_general.StopSort) yield break;
                arr[j + 1] = arr[j];
                arr[j] = k;
                _general.EditGraph(j, j + 1);
                ++_general.CountPermutation;
                --j;
                yield return new WaitForSecondsRealtime(TimeEditGraph);
            }
        }
    }
    public IEnumerator StartInsertionSortRealTime(List<int> arr)
    {
        _general.DisableButtons();
        _general.WaitText.enabled = true;
        double time = Time.realtimeSinceStartupAsDouble;
        yield return StartCoroutine(InsertionSortRealTime(arr));
        _general.EndTimeSort = Time.realtimeSinceStartupAsDouble - time;
        _general.AvailableButtons();
        EndSorting();
    }
    public void InsertionSort(List<int> arr)
    {
        InsertionSortForTests(arr);

        EndSorting();
        _general.UpdateGraph();
    }
    public void InsertionSortForTests(List<int> arr)
    {
        double time = Time.realtimeSinceStartupAsDouble;

        for (int i = 1; i < arr.Count; ++i)
        {
            int k = arr[i];
            int j = i - 1;

            while (j >= 0 && arr[j] > k)
            {
                if (_general.StopSort) return;
                arr[j + 1] = arr[j];
                arr[j] = k;
                ++_general.CountPermutation;
                --j;
            }
        }

        _general.EndTimeSort = (int)((Time.realtimeSinceStartupAsDouble - time) * math.pow(10, 9));
    }
}