using Common;
using Common.Excel;
using Common.Model;
using Common.PubSubEvents;
using Microsoft.Office.Interop.Excel;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using static Common.CommonManager;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace PMEditor.ViewModels
{
    public class ExcelInfoResultViewModel : BindableBase
    {
        #region  프로퍼티
        public string ProgressStateText { get; set; } = "엑셀파일 작업중...";
        public ObservableCollection<Poketmon> DataGridCollection { get; set; } = new ObservableCollection<Poketmon>();
        public double ProgressMin { get; set; } = 0;
        public double ProgressMax { get; set; } = 150;
        public double ProgressValue { get; set; }
        public bool isLoadingShow { get; set; } = false;
        #endregion

        #region 필드
        BackgroundWorker _worker = null;
        IEventAggregator _eventAggregator;
        #endregion

        public ExcelInfoResultViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            #region 앱 닫기(App.thread.Abort())
            //App.loadingWindow.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
            //{
            //    App.loadingWindow.Close();
            //}));

            //App.thread.Abort();
            #endregion

            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.DoWork += _worker_DoWork;
            _worker.ProgressChanged += _worker_ProgressChanged;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
            //_worker.RunWorkerAsync();
        }

        #region 커멘드
        private DelegateCommand _ParsingCommand;
        public DelegateCommand ParsingCommand =>_ParsingCommand ?? (_ParsingCommand = new DelegateCommand(ExecuteParsingCommand));
        void ExecuteParsingCommand()
        {
            ExcelfileInfo excelfileInfo = ExcelManager.GetExcelFileInfo($"{FileManager.ResourceFielPath}\\pokemon_db_v1.1.xlsx", @"SubstractSheet");
            ProgressMax = excelfileInfo.RowCount;

            _worker.RunWorkerAsync();
        }
        #endregion

        #region progressbar function
        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            string filePath = $"{FileManager.ResourceFielPath}\\pokemon_db_v1.1.xlsx";
            string sheet = @"SubstractSheet";

            int successCount = 0;
            isLoadingShow = true;

            if (FileManager.IsFileExist(filePath))
            {
                Application application = new Application();
                ExcelManager.currentexcel = application;
                Workbook workbook = application.Workbooks.Open(Filename: filePath);
                Worksheet worksheet1 = (Worksheet)workbook.Worksheets.get_Item(sheet);

                application.Visible = false;
                Range range = worksheet1.UsedRange;

                for (int i = 1; i <= range.Rows.Count; ++i)
                {
                    string[] array = new string[range.Columns.Count];

                    for (int j = 1; j <= range.Columns.Count; ++j)
                    {
                        //작업 진행 여부 확인
                        if (!CommonManager.isAppWorking)
                            return;

                        if (range.Cells[i, j] != null && (range.Cells[i, j] as Range) != null && (range.Cells[i, j] as Range).Value2 != null)
                        {
                            try
                            {
                                array[j - 1] = (range.Cells[i, j] as Range).Value2.ToString();
                            }
                            catch (Exception)
                            {
                                Console.WriteLine($"error : range.Cells[{i}, {j}] : error");
                                array[j - 1] = "null";
                            }
                        }
                        else
                            array[j - 1] = "null";
                    }

                    DispatcherService.Invoke((System.Action)(() =>
                    {
                        //row 1줄 다 파싱 완료 
                        Poketmon poketmon = new Poketmon()
                        {
                            Number = array[0],
                            Name = array[1],
                            Health = array[2],
                            Attack = array[3],
                            Defense = array[4],
                            SPAttack = array[5],
                            SPDefense = array[6],
                            Speed = array[7],
                            TotalSum = array[8],
                            Property1 = array[9],
                            Property2 = array[10]
                        };

                        //여기서 포켓몬오브젝트 파싱해서 보내자
                        _eventAggregator.GetEvent<SendObjectPoketmonTypeEvent>().Publish(new EventParam(poketmon));

                        Console.WriteLine(poketmon.ToString());
                        DataGridCollection.Add(poketmon);
                        _worker.ReportProgress(++successCount);
                    }));

                }
                application.Quit();
                ExcelManager.DeleteObject(worksheet1);
                ExcelManager.DeleteObject(workbook);
                ExcelManager.DeleteObject(application);
            }
            else
            {
                Console.WriteLine($"경로 : {filePath} 확인바람.");
                MessageBox.Show($"경로 : {filePath} 확인바람.");
            }

        }
        private void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressValue = e.ProgressPercentage;
        }
        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressValue = ProgressMax;
            isLoadingShow = false;
            ProgressStateText = "엑셀 작업 완료!";
        }
        #endregion
    }
}
