using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ScriptTrainer
{


    public class PlayerData : MonoBehaviour
    {
        /// <summary>
        /// 等级
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int age { get; set; }
        /// <summary>
        /// 寿元
        /// </summary>
        public int shouYuan { get; set; }
        /// <summary>
        /// 资质
        /// </summary>
        public int ZiZhi { get; set; }
        /// <summary>
        /// 神识
        /// </summary>
        public int _shengShi { get; set; }
        /// <summary>
        /// 悟性
        /// </summary>
        public int wuXin { get; set; }
        /// <summary>
        /// 遁速
        /// </summary>
        public int _dunSu { get; set; }

    }

}