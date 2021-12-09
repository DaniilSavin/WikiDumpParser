using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Threading;
using System.Windows.Threading;
using WikiDumpParser;
using ICSharpCode.SharpZipLib.BZip2;
using System.Net;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace Demo
{
    //https://dumps.wikimedia.org/ruwiki/20211201/ruwiki-20211201-pages-articles-multistream.xml.bz2
    //https://dumps.wikimedia.org/ruwiki/latest/ruwiki-latest-pages-articles.xml.bz2
    //https://dumps.wikimedia.org/ruwiki/latest/ruwiki-latest-pages-articles-multistream.xml.bz2

    public class Config
    {

        //public DateTimeOffset Date { get; set; }
        public double count_pages { get; set; }
    }

    public partial class MainWindow : Window
    {
        private readonly string URL = "https://dumps.wikimedia.org/ruwiki/latest/ruwiki-latest-pages-articles.xml.bz2";
        DispatcherTimer _timer; string ConfigFileName = "config.json";
        TimeSpan _time; 
        string path; int pageCount = 0; int number_of_articles_found = 0; bool cancel = false;
        Dictionary<string, string> findedPages = new Dictionary<string, string>();
        List<string> KeyWords = new List<string>();
        StreamWriter sw;
        bool configLoaded = false; bool exit = false; bool ifCanceled = false;
        Config config = new Config();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WriteToFile()
        {
            try
            {
                string line = "";
                string fileName = "";
                while (true)
                {
                    if (findedPages.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> fp in findedPages.ToArray())
                        {
                            fileName = (string)fp.Key;
                            line = (string)fp.Value;
                            if (line != "" && line != " ")
                            {
                                try
                                {
                                    fileName = Regex.Replace(fileName, @"[/\\\|:""']", " ");
                                    if (!File.Exists(path + @"\" + fileName + ".txt"))
                                    {
                                        using (sw = new StreamWriter(path + @"\" + fileName + ".txt"))
                                        {
                                            sw.Write(line);
                                            sw.Close();
                                        }
                                    }                
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Exception: " + e.Message);
                                }
                                //numberOfArticleWrited++;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e.Message);
            }
        }
        private void RunTimer()
        {
            _time = TimeSpan.FromSeconds(0);
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            { 
                timer.Text = _time.ToString("c");
                _time = _time.Add(TimeSpan.FromSeconds(+1));
            }, Application.Current.Dispatcher);
            _timer.Start();
        }

        private async Task ReadDumpFromFile(Stream inputStream)
        {
            btnOpenFile.IsEnabled = false;
            btnDownloadFile1.IsEnabled = false;
            progressBar.Visibility = Visibility.Visible;
            ifCanceled = false;
            try
            {
                string[] tempKeyWordsStringA = keyWords.Text.Split(' ');

                for (int i = 0; i < tempKeyWordsStringA.Length; i++)
                {
                    KeyWords.Add(tempKeyWordsStringA[i]);
                }

                using (BZip2InputStream stream = new BZip2InputStream(inputStream))
                {
                    Parser parser = Parser.Create(stream);
                    number_of_articles_found = 0;
                    
                    await Task.Run(() =>
                    {
                        RunTimer();
                        pageCount = 0;
                        
                        Parallel.ForEach(parser.ReadPages(), (page, state) =>
                        {
                            
                            if (cancel)
                            {
                                cancel = false;
                                _timer.Stop();
                                ifCanceled = true;
                                state.Break();
                            }
                            pageCount++;
                            
                            foreach (string keyWord in KeyWords)
                            {
                                if (page.Title.ToLower().Contains(keyWord) && page.Text != "" && !page.Text.ToLower().Contains("#redirect") && !page.Text.ToLower().Contains("#перенаправление"))
                                {
                                    findedPages.Add(page.Title, page.Text);
                                    number_of_articles_found++;
                                    Dispatcher.BeginInvoke(() => { UpdateStatus(); });
                                }
                            }
                            if (pageCount % 100 == 0)
                            {
                                Dispatcher.BeginInvoke(() => { UpdateStatus(); });
                            }
                        });                       
                    });
                    UpdateStatus();
                    _timer.Stop();          
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnOpenFile.IsEnabled = true;
                btnDownloadFile1.IsEnabled = true;
                btnOpenFile.Visibility = Visibility.Visible;
                btnDownloadFile1.Visibility = Visibility.Visible;
                progressBar.Visibility = Visibility.Hidden;
                if (!ifCanceled && !exit)
                {
                    await SaveConfig();
                }
                MessageBox.Show("Поиск завершен: найдено " + number_of_articles_found + " из " + pageCount + " статей." + "\nВремя: " + timer.Text, "Завершено");
            }
        }

        private async Task ProcessDump(Stream inputStream)
        {
            btnOpenFile.IsEnabled = false;
            btnDownloadFile1.IsEnabled = false;
            progressBar.Visibility = Visibility.Visible;

            try
            {
                
                string[] tempKeyWordsStringA = keyWords.Text.Split(' ');
                for (int i = 0; i < tempKeyWordsStringA.Length; i++)
                {
                    KeyWords.Add(tempKeyWordsStringA[i]);
                }

                using (BZip2InputStream stream = new BZip2InputStream(inputStream))
                {
                    Parser parser = Parser.Create(stream);
                    number_of_articles_found = 0;
                    await Task.Run(() =>
                    {
                        RunTimer();
                        pageCount = 0;
                        foreach (var page in parser.ReadPages())
                        {
                            if (cancel)
                            {
                                cancel = false;
                                _timer.Stop();
                                break;
                            }
                            pageCount++;
                            foreach (string keyWord in KeyWords)
                            {
                                if (page.Title.ToLower().Contains(keyWord) && page.Text != "" && !page.Text.ToLower().Contains("#redirect") && !page.Text.ToLower().Contains("#перенаправление"))
                                {
                                    findedPages.Add(page.Title, page.Text);
                                    number_of_articles_found++;
                                    Dispatcher.BeginInvoke(() => { UpdateStatus(); });
                                }
                            }
                            if (pageCount % 100 == 0)
                            {
                                Dispatcher.BeginInvoke(() => { UpdateStatus(); });
                                
                            }

                        }
                    });
                    UpdateStatus();
                    _timer.Stop();    
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnOpenFile.IsEnabled = true;
                btnDownloadFile1.IsEnabled = true;
                btnOpenFile.Visibility = Visibility.Visible;
                btnDownloadFile1.Visibility = Visibility.Visible;
                progressBar.Visibility = Visibility.Hidden;
                if (!ifCanceled && !exit)
                {
                    await SaveConfig();
                }
                MessageBox.Show("Поиск завершен: найдено " + number_of_articles_found + " из " + pageCount + " статей." + "\nВремя: " + timer.Text, "Завершено");
            }
        }

        private void UpdateStatus()
        {
            if (configLoaded && config.count_pages > pageCount)
            {
                progressBar.Value = Math.Truncate((100 * (double)pageCount) / config.count_pages);
                countPages.Text = $"Найдено: {number_of_articles_found} | {progressBar.Value}%";
            }
            else
            {
                config.count_pages = pageCount;
                progressBar.Value = 20;
                progressBar.IsIndeterminate = true;
                countPages.Text = $"Найдено: {number_of_articles_found}";
            }
            txtProgress.Text = $"Страниц: {pageCount}";         
        }

        private async Task SaveConfig ()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                config.count_pages = pageCount;
                using FileStream createStream = File.Create(ConfigFileName);
                await JsonSerializer.SerializeAsync(createStream, config, options);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения конфига.\n" + ex.Message, "Error");
            }
        }
        
        private async Task LoadConfig()
        {
            try
            {
                if (File.Exists(ConfigFileName))
                {
                    using FileStream openStream = File.OpenRead(ConfigFileName);
                    config = await JsonSerializer.DeserializeAsync<Config>(openStream);
                    configLoaded = true;
                }
                else
                {
                    MessageBox.Show("Конфиг не найден, будет создан новый.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void BtnParse_Click(object sender, RoutedEventArgs e)
        {
            if (keyWords.Text != "")
            {
                OpenFileDialog dlg = new OpenFileDialog()
                {
                    Filter = "latest articles bz2|*.bz2",
                };

                if (dlg.ShowDialog() != true)
                {
                    return;
                }
                Thread _thread = new Thread(WriteToFile);
                _thread.Start();
                using (Stream fs = dlg.OpenFile())
                {
                    path2TB.Text = dlg.FileName;
                    chooseSaveFolderBT.IsEnabled = false;
                    btnCancel.IsEnabled = true;
                    path2TB.Visibility = Visibility.Visible;
                    path2TB.IsEnabled = true;
                    await ReadDumpFromFile(fs);
                }
            }
            else
            {
                MessageBox.Show("Вы не ввели ключевые слова. Искать нечего.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDownloadFile_Click(object sender, RoutedEventArgs e){}

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            cancel = true;
            btnCancel.IsEnabled = false;
        }

        private async void btnDownloadFile1_Click(object sender, RoutedEventArgs e)
        {
            if (keyWords.Text != "")
            {
                btnCancel.IsEnabled = true;
                chooseSaveFolderBT.IsEnabled = false;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                Thread _thread = new Thread(WriteToFile);
                _thread.Start();
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream receiveStream = response.GetResponseStream())
                    {
                        await ProcessDump(receiveStream);
                    }
                }
            }
            else
            {
                MessageBox.Show("Вы не ввели ключевые слова. Искать нечего.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) { }
        private void Button_Click(object sender, RoutedEventArgs e){}

        private void chooseSaveFolderBT_Click(object sender, RoutedEventArgs e)
        { 
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            var local_path_ = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            dialog.SelectedPath = local_path_;
            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
                path = dialog.SelectedPath;
                pathTB.Text = path;
                btnOpenFile.IsEnabled = true;
                btnDownloadFile1.IsEnabled = true;
                pathTB.Visibility = Visibility.Visible;
            }
        }

        private void WDP_Closed(object sender, EventArgs e)
        {
            exit = true;
            Environment.Exit(0);
        }

        private async void WDP_Loaded(object sender, RoutedEventArgs e)
        {           
            btnOpenFile.ToolTip = "Быстрее.";
            btnDownloadFile1.ToolTip = "Медленнее.";
            btnOpenFile.Visibility = Visibility.Visible;
            progressBar.Visibility = Visibility.Hidden;
            btnCancel.IsEnabled = false;
            keyWords.ToolTip = "Введите слова через пробел.";
            chooseSaveFolderBT.ToolTip = "Выбрать место для сохранения статей.";
            btnCancel.ToolTip = "Прекратить поиск.";
            path2TB.Visibility = Visibility.Hidden;
            pathTB.Visibility = Visibility.Hidden;
            await LoadConfig();
        }
    }
}
