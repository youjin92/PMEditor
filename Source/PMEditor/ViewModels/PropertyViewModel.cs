using Common;
using Common.DB;
using Common.Model;
using Common.PubSubEvents;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static Common.CommonManager;

namespace PMEditor.ViewModels
{
    public class PropertyViewModel : BindableBase
    {
        #region 프로퍼티
        public ObservableCollection<Poketmon> ListBoxList { get; set; } = new ObservableCollection<Poketmon>();
        public string SearchText { get; set; }
        public Poketmon SelectedItem { get; set; } = new Poketmon();
        #endregion

        #region Field
        private readonly IEventAggregator _eventAggregator;
        private string originText;
        #endregion

        #region 커멘드
        private DelegateCommand _EnterCommand;
        public DelegateCommand EnterCommand =>_EnterCommand ?? (_EnterCommand = new DelegateCommand(ExecuteEnterCommand));
        void ExecuteEnterCommand()
        {
            SearchAndRefreshControl(SearchText);
        }

        #endregion

        public PropertyViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<SearchItemEvent>().Subscribe(ExcuteReceiveItem);
        }

        private void ExcuteReceiveItem(EventParam obj)
        {
            string ReceivedText = obj.Item.ToString().TrimStart().TrimEnd();
            if (originText != ReceivedText)
            {
                SearchText = ReceivedText;
                originText = ReceivedText;

                if(ReceivedText != "fail")
                    SearchAndRefreshControl(SearchText);
            }
        }

        private void SearchAndRefreshControl(string TargetText)
        {
            ObservableCollection<Poketmon> TempCollection = new ObservableCollection<Poketmon>(ListBoxList);

            Poketmon TempSelectedItem = new Poketmon();

            if (SelectedItem != null)
            {
                TempSelectedItem = SelectedItem.Clone(SelectedItem);
            }

            ListBoxList.Clear();

            Search(TargetText);

            if (ListBoxList.Count > 0)
            {
                foreach (Poketmon item in ListBoxList)
                {
                    if (TempSelectedItem.Name == item.Name)
                    {
                        SelectedItem = item;
                        return;
                    }
                }

                SelectedItem = ListBoxList[0];
            }
            else
            {
                ListBoxList = TempCollection;

                foreach (Poketmon item in ListBoxList)
                {
                    if (TempSelectedItem.Name == item.Name)
                    {
                        SelectedItem = item;
                        return;
                    }
                }
            }
        }

        private void Search(string TargetText)
        {
            DBManager.Connect();
            //여기서부터 다시 하면 된다.

            string sql = $"SELECT * FROM Pocketmons WHERE Name LIKE '%{TargetText}%'";

            DBManager.ReadRow(sql, (System.Action<SQLiteDataReader>)((rdr) =>
            {
                while (rdr.Read())
                {
                    ListBoxList.Add(new Poketmon()
                    {
                        Number = rdr["Number"].ToString(),
                        Name = rdr["Name"].ToString(),
                        Health = rdr["Health"].ToString(),
                        Attack = rdr["Attack"].ToString(),
                        Defense = rdr["Defense"].ToString(),
                        SPAttack = rdr["SPAttack"].ToString(),
                        SPDefense = rdr["SPDefense"].ToString(),
                        Speed = rdr["Speed"].ToString(),
                        TotalSum = rdr["TotalSum"].ToString(),
                        Property1 = rdr["Property1"].ToString(),
                        Property2 = rdr["Property2"].ToString(),
                    });
                }
            }));

            if (ListBoxList.Count == 0)
                AdditionalSearch(TargetText);

            DBManager.Close();
        }

        private void  AdditionalSearch(string TargetText)
        {
            if (string.IsNullOrEmpty(TargetText))
                return;

            ObservableCollection<Poketmon> temp = new ObservableCollection<Poketmon>();

            #region 열의 총 갯수 확인
            int TotalCount = -1;
            string sql = @"select count(*) from Pocketmons";

            DBManager.ReadRow(sql, (System.Action<SQLiteDataReader>)((rdr) =>
            {
                while (rdr.Read())
                {
                    TotalCount = Convert.ToInt32(rdr["count(*)"].ToString());
                }
            }));
            #endregion

            #region 모든 열에 대해서 Name이 TargetText에 대해서 매칭 카운트 확인
            int[] InclusionCheckArray = Enumerable.Repeat(0, TotalCount + 1).ToArray();

            foreach (char item in TargetText)
            {
                string sql4 = $"select count(*) from UsedWords where UsedWord like '{item}'";
                int WordMatchingCount = 0;
                DBManager.ReadRow(sql4, (System.Action<SQLiteDataReader>)((rdr) =>
                {
                    while (rdr.Read())
                    {
                        WordMatchingCount = Convert.ToInt32(rdr["count(*)"].ToString());
                    }
                }));

                if (WordMatchingCount == 0)
                    continue;

                for (int index = 1; index <= TotalCount; index++)
                {
                    string sql2 = $"select * from Pocketmons where Number like '{index}'";

                    DBManager.ReadRow(sql2, (System.Action<SQLiteDataReader>)((rdr) =>
                    {
                        while (rdr.Read())
                        {
                            string Name = rdr["Name"].ToString();
                            if (Name.Contains(item))
                            {
                                ++InclusionCheckArray[index];
                            }
                        }
                    }));
                }
            }
            #endregion

            int MaxMatchCount = MaxNumberInArray(InclusionCheckArray);

            for (int index = 1; index <= TotalCount; index++)
            {
                if (InclusionCheckArray[index] != MaxMatchCount)
                    continue;

                string sql3 = $"SELECT * FROM Pocketmons WHERE Number like '{index}'";
                Poketmon AddPoketmon = new Poketmon();
                DBManager.ReadRow(sql3, (System.Action<SQLiteDataReader>)((rdr) =>
                {
                    while (rdr.Read())
                    {
                        ListBoxList.Add( new Poketmon()
                        {
                            Number = rdr["Number"].ToString(),
                            Name = rdr["Name"].ToString(),
                            Health = rdr["Health"].ToString(),
                            Attack = rdr["Attack"].ToString(),
                            Defense = rdr["Defense"].ToString(),
                            SPAttack = rdr["SPAttack"].ToString(),
                            SPDefense = rdr["SPDefense"].ToString(),
                            Speed = rdr["Speed"].ToString(),
                            TotalSum = rdr["TotalSum"].ToString(),
                            Property1 = rdr["Property1"].ToString(),
                            Property2 = rdr["Property2"].ToString(),
                        });
                    }
                }));
            }
        }

        private int MaxNumberInArray(int[] array)
        {
            if (array.Length == 0)
                return -1;

            int maxNumber = -1;

            for (int i = 0; i < array.Length; i++)
            {
                if (maxNumber < array[i])
                    maxNumber = array[i];
            }

            return maxNumber;
        }
    }
}
