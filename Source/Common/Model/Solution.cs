using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Solution : BindableBase
    {
        public bool Is1stSolved { get; set; } = false;
        public bool Is2ndSolved { get; set; } = false;
        public bool Is3thSolved { get; set; } = false;
        public bool Is4thSolved { get; set; } = false;
        public bool Is5thSolved { get; set; } = false;
        public bool Is6thSolved { get; set; } = false;

        public bool IsEventFire { get; set; } = false;
    }
}
