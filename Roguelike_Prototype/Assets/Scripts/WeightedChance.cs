using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedChance<T> : IList<T>
{
    [System.Serializable]
    public class WeightedOption {
        public T option;
        public float chance;
    }

    public List<WeightedOption> options;

    //vars
    private float totalChance;

    //ctor
    public WeightedChance()
    {
        options = new List<WeightedOption>();
    }

    //=============== compile options===============
    private void CalcTotalChance()
    {
        totalChance = 0;
        foreach (WeightedOption option in options) {
            totalChance += option.chance;
        }
    }

    //=============== random chance ===============
    public T GetRandomEntry()
    {
        if (totalChance <= 0f) { CalcTotalChance(); }
        //choose random option
        T chosenOption = default;
        float rand = Random.Range(0, totalChance);
        for (int i = 0; i < options.Count; i++) {
            //found chosen option
            if (rand < options[i].chance) {
                chosenOption = options[i].option;
                break;
            }
            rand -= options[i].chance;
        }
        return chosenOption;
    }

    public void SetChance(int index, float newChance = 1f)
    {
        if (newChance < 0f) { newChance = 1f; }
        options[index].chance = newChance;
    }

    //============== IList vars ==============
    public int Count { get { return options.Count; } }
    public T this[int index] {
        get { return options[index].option; }
        set { options[index].option = value; }
    }
    public bool IsReadOnly { get { return false; } }

    //============== IList funcs ==============
    public int IndexOf(T option) 
    { 
        for (int i = 0; i < Count; i++) {
            if (options[i].option.Equals(option)) { return i; }
        }
        return -1; //not found
    }

    public void Insert(int index, T option) {
        Insert(index, option, 1f);
    }
    public void Insert(int index, T option, float chance) {
        if (chance < 0) { chance = 0; }
        options.Insert(index, new WeightedOption { option = option, chance = chance });
        CalcTotalChance();
    }

    public void RemoveAt(int index) { 
        options.RemoveAt(index);
        CalcTotalChance();
    }

    //ICollection funcs
    public void Add(T option) {
        Add(option, 1f);
    }
    public void Add(T option, float chance) {
        if (chance < 0f) { chance = 1f; }
        options.Add(new WeightedOption { option = option, chance = chance });
        CalcTotalChance();
    }

    public void Clear() { 
        options.Clear();
        CalcTotalChance();
    }
    public bool Contains(T option) {
        foreach (WeightedOption weightedOption in options) {
            if (weightedOption.option.Equals(option)) {
                return true;
            }
        }
        return false;
    }
    public void CopyTo(T[] array, int startIndex) {
        array = new T[Count - startIndex];
        //fill new array
        for (int i = startIndex; i < Count; i++) {
            array[i] = options[i].option;
        }
    }
    public bool Remove(T option) {
        int foundIndex = IndexOf(option);
        if (foundIndex >= 0) { options.RemoveAt(foundIndex); }
        //recalculate chances
        CalcTotalChance();
        //return result
        return foundIndex >= 0;
    }
    //Enumerator funcs
    public IEnumerator<T> GetEnumerator() {
        T[] array = new T[0];
        CopyTo(array, 0);
        //return Enumerator
        return (IEnumerator<T>)array.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
