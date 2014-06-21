using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        public GameObject[] Letters;

        private int currentIndex;

        public void Next()
        {
            if (currentIndex > Letters.Length - 1) return;

            Instantiate(Letters[currentIndex], Vector3.zero, Quaternion.identity);
            currentIndex += 1;
        }
    }
