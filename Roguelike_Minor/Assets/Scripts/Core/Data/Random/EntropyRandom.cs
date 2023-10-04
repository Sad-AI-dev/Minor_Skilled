using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Data {
    public class EntropyRandom<T>
    {
        private List<T> input;
        private Queue<T> randomQueue;

        //ctor
        public EntropyRandom(List<T> input)
        {
            this.input = input;
            Shuffle();
        }

        //========== Random Shuffle List =============
        private void Shuffle()
        {
            randomQueue = new Queue<T>();
            List<T> inputCopy = new List<T>(input);
            while (inputCopy.Count > 0)
            {
                int randIndex = Random.Range(0, randomQueue.Count);
                randomQueue.Enqueue(inputCopy[randIndex]);
                inputCopy.RemoveAt(randIndex);
            }
        }

        //============== Get Random =============
        public T Next()
        {
            T next = randomQueue.Peek();
            if (randomQueue.Count <= 0) { Shuffle(); }
            return next;
        }
    }
}
