﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using OLXParser.DataBaseConnection;
using OLXParser.TableRepositories;
using OLXParser.Parsers;
using System.Text.RegularExpressions;

namespace OLXParser
{
    public partial class FormMain : Form
    {
        private readonly int advertsOnPage = 44;
        private IDBProcessor dbProcessor;
        private AdvertRepository advertRepository;
        private ImageRepository imageRepository;
        public FormMain(IDBProcessor dbProcessor)
        {
            InitializeComponent();
            this.dbProcessor = dbProcessor;
            advertRepository = new AdvertRepository(dbProcessor);
            imageRepository = new ImageRepository(dbProcessor);
            UpdateInfo();

            //List<string> testUrls = new List<string> { "http://dakwdnawpage.com/?page=16&pwqe=12", "http://quwdjqwpodpage15.as/?sad&page=17", "http://dakwdnawpage.com/?pageSize=15&page=16&saka=A" };
            //MessageBox.Show(String.Join(Environment.NewLine, testUrls.Select(x => RemovePageFromUrl(x))));
        }
        private void UpdateInfo()
        {
            try
            {
                labelAdvertCount.Text = advertRepository.NumberOfAdverts().ToString();
            }
            catch { labelAdvertCount.Text = "-"; }
        }
        private void UpdateProgressBar(int val, int max)
        {
            this.progressBarMain.Maximum = max;
            this.progressBarMain.Value = val;
        }
        private void FinishProgressBar()
        {
            this.progressBarMain.Value = this.progressBarMain.Maximum;
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if(this.textBoxURL.Text.Length <=0)
            {
                MessageBox.Show("Пожалуйста, укажите ссылку на раздел", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            int count = 0;
            string error = ""; int errorCount = 0;
            int pagesCount;
            if(!Int32.TryParse(numericUpDownPages.Value.ToString(), out pagesCount))
            {
                MessageBox.Show("Неверно указано количество страниц.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            this.Enabled = false;
            UpdateProgressBar(0, advertsOnPage * pagesCount);
            for (int i = 1; i <= pagesCount; ++i)
            {
                string url = textBoxURL.Text;
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
                catch { ++errorCount; error = "\r\nПроизошла ошибка при взятии ссылок.\r\nКол-во ошибок: " + errorCount; }
                foreach (var u in urls)
                    try
                    {
                        Application.DoEvents();
                        progressBarMain.Increment(1);
                        List<string> images;
                        var advert = OLXConcreteAdvertParser.GetAdvert(u, out images);
                        if (images.Count > 0)
                        {
                            int advertId = advertRepository.AddAdvert(advert);
                            foreach (var image in images)
                                imageRepository.AddImage(new DBEntities.Image { sourceurl = image, advertid = advertId });
                        }
                    }
                    catch { }
                count += urls.Count;
            }
            UpdateInfo();
            FinishProgressBar();
            MessageBox.Show("Всего на страницах было найдено: " + count + " объявлений" + error);
            this.Enabled = true;
        }
        private static String RemovePageFromUrl(string url)
        {
            Regex regex = new Regex("[?|&]page=[0-9]+");
            var math = regex.Match(url);
            if (math.Success)
            {
                int ampersandIndex = url.IndexOf('&', math.Index + math.Length - 1);
                if (math.Value[0] == '?' && ampersandIndex == math.Index + math.Length)
                {
                    StringBuilder sb = new StringBuilder(url);
                    sb[ampersandIndex] = '?';
                    url = sb.ToString();
                }
                url = url.Remove(math.Index, math.Length);
            }
            return url;
        }
        private void buttonExport_Click(object sender, EventArgs e)
        {
            if(advertRepository.NumberOfAdverts() <= 0)
            {
                MessageBox.Show("Нечего выгружать. БД пуста.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.progressBarExport.Maximum = advertRepository.NumberOfAdverts();
                this.progressBarExport.Value = 0;
                this.Enabled = false;

                string foldername = folderBrowserDialog1.SelectedPath;
                var fp = new FilesProcessor(foldername);
                var adverts = advertRepository.GetAdverts();
                string error = null;
                foreach (var advert in adverts)
                {
                    Application.DoEvents();
                    this.progressBarExport.Increment(1);
                    fp.Save(advert, imageRepository.GetImagesByAdvertID(advert.id), out error);
                }
                this.progressBarExport.Value = this.progressBarExport.Maximum;
                if (error != null)
                    MessageBox.Show(error, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Process.Start(folderBrowserDialog1.SelectedPath);
                MessageBox.Show("Процесс выгрузки БД прошел успешно.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.Enabled = true;
        }

        private void buttonClearDB_Click(object sender, EventArgs e)
        {
            if (advertRepository.NumberOfAdverts() <= 0)
            {
                MessageBox.Show("Очистка не требуется. БД пуста.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить все объявления из базы данных?", "Подтверждение", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    int rows = advertRepository.ClearAdvert();
                    UpdateInfo();
                    MessageBox.Show("БД успешно очищена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch { MessageBox.Show("Произошла ошибка во время попытки очистки БД.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }
    }
}
