using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class DefaultState
    {
        /// <summary> 이름 </summary>
        public string Name;
        /// <summary> 직업 </summary>
        public string Job;
        /// <summary> 레벨 </summary>
        public int Level;
        /// <summary> 공격력 </summary>
        public float ATK;
        /// <summary> 방어력 </summary>
        public float DEF;
        /// <summary> 체력 </summary>
        public int HP;
        /// <summary> 골드(소지금) </summary>
        public int Gold;

        public DefaultState()
        {
            Name = string.Empty;
            Job = string.Empty;
            Level = 0;
            ATK = 0;
            DEF = 0;
            HP = 0;
            Gold = 0;
        }
    }
}
