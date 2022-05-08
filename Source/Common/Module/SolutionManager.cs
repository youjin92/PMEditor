using Common.IService;
using Common.Model;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Module
{
    public class SolutionManager : BindableBase, ISolutionManager
    {
        private readonly IEventAggregator _eventAggregator;

        public SolutionManager(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public Solution Solution { get; set; } = new Solution();

    }
}
