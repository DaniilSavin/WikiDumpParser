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

namespace Demo
{
    public partial class MainWindow : Window
    {
        DispatcherTimer _timer;
        TimeSpan _time; 
        string path; int pageCount = 0; int number_of_articles_found = 0; bool cancel = false;
        Dictionary<string, string> findedPages = new Dictionary<string, string>();
        List<string> KeyWords = new List<string>(); bool findPages = false;
        StreamWriter sw;
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
                            if (line != "" && !line.Contains("#REDIRECT") && !line.Contains("#перенаправление") && !line.Contains("#redirect"))
                            {
                                try
                                {
                                    fileName = Regex.Replace(fileName, @"[/\\\|:""']", " ");
                                    using (sw = new StreamWriter(path + @"\" + fileName + ".txt"))
                                    {
                                        sw.Write(line);
                                        sw.Close();
                                    }
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Exception: " + e.Message);
                                }
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
            findPages = false;
            btnOpenFile.IsEnabled = false;
            btnDownloadFile1.IsEnabled = false;
            progressBar.Visibility = Visibility.Visible;

            try
            {
                if (keyWords.Text != "")
                {
                    string[] tempKeyWordsStringA = keyWords.Text.Split(' ');

                    for (int i = 0; i < tempKeyWordsStringA.Length; i++)
                    {
                        KeyWords.Add(tempKeyWordsStringA[i]);
                    }
                    findPages = true;
                }

                using (BZip2InputStream stream = new BZip2InputStream(inputStream))
                {
                    Parser parser = Parser.Create(stream);
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
                                state.Break();
                            }
                            pageCount++;
                            if (findPages)
                            {
                                foreach (string keyWord in KeyWords)
                                {
                                    int i = 0;
                                    if (page.Title.ToLower().Contains(keyWord))
                                    {
                                        findedPages.Add(page.Title.ToString(), page.Text.ToString());
                                        number_of_articles_found++;
                                        Dispatcher.BeginInvoke(() => { UpdateStatus(); });
                                    }
                                    i++;
                                }
                               
                            }
                            else
                            {
                                //Debug.WriteLine(page.Title);
                            }
                            if (pageCount % 100 == 0)
                            {
                                Dispatcher.BeginInvoke(() => { UpdateStatus(); });
                            }
                        });

                    });

                    UpdateStatus();
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
            }
        }

        private async Task ProcessDump(Stream inputStream)
        {
            findPages = false;
            btnOpenFile.IsEnabled = false;
            btnDownloadFile1.IsEnabled = false;
            progressBar.Visibility = Visibility.Visible;

            try
            {
                if (keyWords.Text != "")
                {
                    string[] tempKeyWordsStringA = keyWords.Text.Split(' ');
                    for (int i = 0; i < tempKeyWordsStringA.Length; i++)
                    {
                        KeyWords.Add(tempKeyWordsStringA[i]);
                    }
                    findPages = true;
                }

                using (BZip2InputStream stream = new BZip2InputStream(inputStream))
                {
                    Parser parser = Parser.Create(stream);
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
                            if (findPages)
                            {
                                foreach (string keyWord in KeyWords)
                                {
                                    int i = 0;
                                    if (page.Title.ToLower().Contains(keyWord))
                                    {
                                        findedPages.Add(page.Title, page.Text);
                                        number_of_articles_found++;
                                        Dispatcher.BeginInvoke(() => { UpdateStatus(); });
                                    }
                                    i++;
                                }
                                string line = "";
                                string fileName = "";
                                foreach (KeyValuePair<string, string> fp in findedPages)
                                {
                                    fileName = fp.Key;
                                    try
                                    {
                                        line = fp.Value;
                                        sw = new StreamWriter(path +@"\"+ fileName + ".txt");
                                        sw.Write(line);
                                        sw.Close();
                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show("Exception: " + e.Message);
                                    }
                                }
                            }
                            else
                            {
                                //Debug.WriteLine(page.Title);
                            }
                            if (pageCount % 100 == 0)
                            {
                                Dispatcher.BeginInvoke(() => { UpdateStatus(); });
                            }
                        }

                    });

                    UpdateStatus();
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
            }
        }

        private void UpdateStatus()
        {
            txtProgress.Text = $"Pages: {pageCount}";
            countPages.Text = $"Found: {number_of_articles_found}";
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

                using (Stream fs = dlg.OpenFile())
                {
                    chooseSaveFolderBT.IsEnabled = false;
                    btnCancel.IsEnabled = true;
                    Thread _thread = new Thread(WriteToFile);
                    _thread.Start();
                    await ReadDumpFromFile(fs);
                }
            }
            else
            {
                MessageBox.Show("Вы не ввели ключевые слова. Искать нечего.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDownloadFile_Click(object sender, RoutedEventArgs e){ }

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
                HttpWebRequest request = WebRequest.Create("https://dumps.wikimedia.org/ruwiki/latest/ruwiki-latest-pages-articles.xml.bz2") as HttpWebRequest;
                using (HttpWebResponse response = (HttpWebResponse)(request.GetResponse()))
                using (Stream receiveStream = response.GetResponseStream())
                {
                    await ProcessDump(receiveStream);
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
                
            }
        }

        private void WDP_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void WDP_Loaded(object sender, RoutedEventArgs e)
        {           
            btnOpenFile.ToolTip = "Быстрее.";
            btnDownloadFile1.ToolTip = "Медленнее.";
            btnOpenFile.Visibility = Visibility.Visible;
            progressBar.Visibility = Visibility.Hidden;
            btnCancel.IsEnabled = false;
            keyWords.ToolTip = "Введите слова через пробел.";
            chooseSaveFolderBT.ToolTip = "Выбрать место для сохранения статей.";
            btnCancel.ToolTip = "Прекратить поиск.";
        }
    }
}
