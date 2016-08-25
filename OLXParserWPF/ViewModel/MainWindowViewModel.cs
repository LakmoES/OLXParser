using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using OLXParser.Parsers;
using OLXParser.TableRepositories;
using OLXParserWPF.Messages;
using OLXParser;

namespace OLXParserWPF.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Constructor

        public MainWindowViewModel()
        {
            WindowIsEnabled = true;
            Pages = 1;
            MainProgressMaximum = 1;
            MainProgressValue = 0;
            ExportProgressMaximum = 1;
            ExportProgressValue = 0;

            RegisterYesNoMessageReceive();

            var dbProcessor = Initializer.Init();

            advertRepository = new AdvertRepository(dbProcessor);
            imageRepository = new ImageRepository(dbProcessor);
        }

        #endregion

        #region Private fields

        private readonly int advertsOnPage = 44;

        private AdvertRepository advertRepository;
        private ImageRepository imageRepository;

        private string _siteUrl;
        private int _pages;
        private bool _windowIsEnabled;
        private int _recordsInDb;
        private int _mainProgressValue;
        private int _mainProgressMaximum;
        private int _exportProgressValue;
        private int _exportProgressMaximum;

        #endregion

        #region Public properties

        public string SiteUrl
        {
            get { return _siteUrl; }
            set
            {
                _siteUrl = value;
                RaisePropertyChanged(() => SiteUrl);
            }
        }
        public int Pages
        {
            get { return _pages; }
            set
            {
                _pages = value;
                RaisePropertyChanged(() => Pages);
            }
        }
        public bool WindowIsEnabled
        {
            get { return _windowIsEnabled; }
            set
            {
                _windowIsEnabled = value;
                RaisePropertyChanged(() => WindowIsEnabled);
            }
        }
        public int RecordsInDb
        {
            get { return _recordsInDb; }
            set
            {
                _recordsInDb = value;
                RaisePropertyChanged(() => RecordsInDb);
            }
        }

        public int MainProgressValue
        {
            get { return _mainProgressValue; }
            set
            {
                _mainProgressValue = value;
                RaisePropertyChanged(() => MainProgressValue);
            }
        }
        public int MainProgressMaximum
        {
            get { return _mainProgressMaximum; }
            set
            {
                _mainProgressMaximum = value;
                RaisePropertyChanged(() => MainProgressMaximum);
            }
        }

        public int ExportProgressValue
        {
            get { return _exportProgressValue; }
            set
            {
                _exportProgressValue = value;
                RaisePropertyChanged(() => ExportProgressValue);
            }
        }
        public int ExportProgressMaximum
        {
            get { return _exportProgressMaximum; }
            set
            {
                _exportProgressMaximum = value;
                RaisePropertyChanged(() => ExportProgressMaximum);
            }
        }

        #endregion

        #region Events

        public event EventHandler<string> AlarmToUI;

        private void RaiseAlarm(string message)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    AlarmToUI?.Invoke(this, message));
        }

        public event EventHandler<string> QuestionYesNo;

        private void RaiseQuestionYesNo(string title)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    QuestionYesNo?.Invoke(this, title));
        }

        #endregion

        private static string RemovePageFromUrl(string url)
        {
            Regex regex = new Regex("[?|&]page=[0-9]+");
            var math = regex.Match(url);
            if (!math.Success)
                return url;
            int ampersandIndex = url.IndexOf('&', math.Index + math.Length - 1);
            if (math.Value[0] == '?' && ampersandIndex == math.Index + math.Length)
            {
                StringBuilder sb = new StringBuilder(url) {[ampersandIndex] = '?'};
                url = sb.ToString();
            }
            return url.Remove(math.Index, math.Length);
        }

        private void UpdateInfo()
        {
            try
            {
                RecordsInDb = advertRepository.NumberOfAdverts();
            }
            catch
            {
                RecordsInDb = -1;
            }
        }

        #region Commands

        private ICommand _startParsingCommand;

        public ICommand StartParsingCommand
        {
            get
            {
                return _startParsingCommand ??
                       (_startParsingCommand = new RelayCommand(async () => await Task.Run(() => StartParsing())));
            }
        }

        private ICommand _clearDbCommand;

        public ICommand ClearDbCommand
        {
            get
            {
                return _clearDbCommand ??
                       (_clearDbCommand = new RelayCommand(async () => await Task.Run(() => ClearDbStep1())));
            }
        }

        #endregion

        private void StartParsing()
        {
            if (String.IsNullOrWhiteSpace(SiteUrl))
            {
                RaiseAlarm("Пожалуйста, укажите ссылку на раздел");
                return;
            }
            int count = 0;
            string error = "";
            int errorCount = 0;

            WindowIsEnabled = false;
            MainProgressMaximum = advertsOnPage*Pages;
            MainProgressValue = 0;
            for (int i = 1; i <= Pages; ++i)
            {
                string url = SiteUrl;
                url = RemovePageFromUrl(url);
                if (url.IndexOf('?') != -1)
                    url += "&page=" + i;
                else
                    url += "?page=" + i;

                List<string> urls = new List<string>();
                try
                {
                    urls = OLXListParser.GetHrefs(url);
                }
                catch
                {
                    ++errorCount;
                    error = "\r\nПроизошла ошибка при взятии ссылок.\r\nКол-во ошибок: " + errorCount;
                }
                foreach (var u in urls)
                    try
                    {
                        ++MainProgressValue;
                        List<string> images;
                        var advert = OLXConcreteAdvertParser.GetAdvert(u, out images);
                        if (images.Count > 0)
                        {
                            int advertId = advertRepository.AddAdvert(advert);
                            foreach (var image in images)
                                imageRepository.AddImage(new OLXParser.DBEntities.Image
                                {
                                    sourceurl = image,
                                    advertid = advertId
                                });
                        }
                    }
                    catch
                    {
                    }
                count += urls.Count;
            }
            UpdateInfo();
            MainProgressValue = MainProgressMaximum;
            RaiseAlarm("Всего на страницах было найдено: " + count + " объявлений" + error);
            WindowIsEnabled = true;
            return;
        }

        private void RegisterYesNoMessageReceive()
        {
            Messenger.Default.Register<YesNoMessage>(
                this,
                message =>
                {
                    ClearDbStep2(message.Message);
                });
        }

        private void ClearDbStep1()
        {
            if (advertRepository.NumberOfAdverts() <= 0)
            {
                RaiseAlarm("Очистка не требуется. БД пуста.");
                return;
            }
            RaiseQuestionYesNo("Вы действительно хотите удалить все объявления из базы данных?");
            //DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить все объявления из базы данных?", "Подтверждение", MessageBoxButtons.YesNo);
        }

        private void ClearDbStep2(bool questionResult)
        {
            if (questionResult)
            {
                try
                {
                    int rows = advertRepository.ClearAdvert();
                    UpdateInfo();
                    RaiseAlarm(string.Format("БД успешно очищена. Удалено {0} строк.", rows));
                }
                catch
                {
                    RaiseAlarm("Произошла ошибка во время попытки очистки БД.");
                }
            }
        }

        //private void StartExport()
        //{
        //    if (advertRepository.NumberOfAdverts() <= 0)
        //    {
        //        RaiseAlarm("Нечего выгружать. БД пуста.");
        //        return;
        //    }

        //    FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
        //    DialogResult result = folderBrowserDialog1.ShowDialog();
        //    if (result == DialogResult.OK)
        //    {
        //        ExportProgressMaximum = advertRepository.NumberOfAdverts();
        //        ExportProgressValue = 0;
        //        WindowIsEnabled = false;

        //        string foldername = folderBrowserDialog1.SelectedPath;
        //        var fp = new FilesProcessor(foldername);
        //        var adverts = advertRepository.GetAdverts();
        //        string error = null;
        //        foreach (var advert in adverts)
        //        {
        //            ++ExportProgressValue;
        //            fp.Save(advert, imageRepository.GetImagesByAdvertID(advert.id), out error);
        //        }
        //        ExportProgressValue = ExportProgressMaximum;
        //        if (error != null)
        //            RaiseAlarm("Ошибка");

        //        Process.Start(folderBrowserDialog1.SelectedPath);
        //        RaiseAlarm("Процесс выгрузки БД прошел успешно.");
        //    }
        //    WindowIsEnabled = true;
        //}
    }
}
