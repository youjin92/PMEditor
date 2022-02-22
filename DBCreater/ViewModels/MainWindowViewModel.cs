using Common;
using Common.Model;
using Common.PubSubEvents;
using DBCreater.DB;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBCreater.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;

        public string Title { get; set; } = "DBCreater";
        public string InputText { get; set; } = "";
        
        public ObservableCollection<Poketmon> ListBoxList { get; set; } = new ObservableCollection<Poketmon>();
        public Poketmon SelectedItem { get; set; } = new Poketmon();

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<SendObjectPoketmonTypeEvent>().Subscribe(ExcuteReceiveItem);

        }

        private DelegateCommand _CreateDBCommand;
        public DelegateCommand CreateDBCommand => _CreateDBCommand ?? (_CreateDBCommand = new DelegateCommand(ExecuteCreateDBCommand));
        void ExecuteCreateDBCommand()
        {
            DBManager.CreatDBFile(); 
        }

        private DelegateCommand _ConnectCommand;
        public DelegateCommand ConnectCommand =>
            _ConnectCommand ?? (_ConnectCommand = new DelegateCommand(ExecuteConnectCommand));
        void ExecuteConnectCommand()
        {
            DBManager.Connect();
        }

        private DelegateCommand _CreateTableCommand;
        public DelegateCommand CreateTableCommand =>
            _CreateTableCommand ?? (_CreateTableCommand = new DelegateCommand(ExecuteCreateTableCommand));
        void ExecuteCreateTableCommand()
        {
            //string sql = "create table Pocketmons (Number varchar(20), Name varchar(20), Health varchar(20), Attack varchar(20), Defense varchar(20), " +
            //                                        "SPAttack varchar(20), SPDefense varchar(20), Speed varchar(20), " +
            //                                        "TotalSum varchar(20), Property1 varchar(20), Property2 varchar(20))";
            string sql2 = "create table UsedWords (UsedWord varchar(20))";
            DBManager.CreateTable(sql2);
        }

        private DelegateCommand _ReadRowCommand;
        public DelegateCommand ReadRowCommand =>
            _ReadRowCommand ?? (_ReadRowCommand = new DelegateCommand(ExecuteReadRowCommand));
        void ExecuteReadRowCommand()
        {
            string sql = $"SELECT * FROM Pocketmons WHERE Name LIKE '%{InputText}%'";

            DBManager.ReadRow(sql,(System.Action<SQLiteDataReader>)((rdr) =>
            {
                while (rdr.Read())
                {
                    ListBoxList.Add(new Poketmon() { 
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

        private DelegateCommand _CloseCommand;
        public DelegateCommand CloseCommand =>
            _CloseCommand ?? (_CloseCommand = new DelegateCommand(ExecuteCloseCommand));
        void ExecuteCloseCommand()
        {

            DBManager.Close();
        }

        private DelegateCommand _InSertRowCommand;
        public DelegateCommand InSertRowCommand =>_InSertRowCommand ?? (_InSertRowCommand = new DelegateCommand(ExecuteInSertRowCommand));
        void ExecuteInSertRowCommand()
        {
            //불러오기
            StringBuilder stringBuilder = new StringBuilder();
       
            string sql2 = $"SELECT Name from Pocketmons";

            DBManager.ReadRow(sql2, (System.Action<SQLiteDataReader>)((rdr) =>
            {
                while (rdr.Read())
                {
                    string Name = rdr["Name"].ToString();
                    stringBuilder.Append(Name);
                }
            }));

            string OneLineString = stringBuilder.ToString();
            List<string> stringList = new List<string>();

            foreach (char word in OneLineString)
            {
                if (!stringList.Contains(word.ToString()))
                    stringList.Add(word.ToString());
            }
            string sql3 = "";

            foreach (string item in stringList)
            {
                sql3 = $"insert into UsedWords (UsedWord) values ('{item}')";
                //엑셀파일 받아서 DB인서트
                DBManager.InsertRow(sql3);
            }
        }

        private void ExcuteReceiveItem(EventParam obj)
        {
            Poketmon ReceivedItem = obj.Item as Poketmon;
            string insertInfo = $"'{ReceivedItem.Number}', '{ReceivedItem.Name}', '{ReceivedItem.Health}', '{ReceivedItem.Attack}', '{ReceivedItem.Defense}'" +
                $", '{ReceivedItem.SPAttack}', '{ReceivedItem.SPDefense}', '{ReceivedItem.Speed}', '{ReceivedItem.TotalSum}', '{ReceivedItem.Property1}', '{ReceivedItem.Property2}'";

            string sql = "insert into Pocketmons (Number, Name, Health, Attack, Defense, SPAttack, SPDefense, Speed, TotalSum, Property1, Property2) " +
                "values (" + insertInfo + ")";

            //엑셀파일 받아서 DB인서트
            DBManager.InsertRow(sql);
        }
    }
}

