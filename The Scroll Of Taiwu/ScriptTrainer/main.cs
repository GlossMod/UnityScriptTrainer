using System;
using HarmonyLib;
using UnityEngine;

namespace ScriptTrainer
{
    public class main: MonoBehaviour
    {
        public main instance;
        private static GameObject gameObject;

        public main()
        {
            instance = this;
        }

        void Start()
        {
            Debug.Log("ScriptTrainer Start");
            new ScriptTrainer().Initialize();
        }

        void OnDisable()
        {
            new ScriptTrainer().Dispose();
        }


    }
}
