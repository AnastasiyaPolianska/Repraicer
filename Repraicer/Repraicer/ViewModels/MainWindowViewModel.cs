using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Repraicer.Commands;
using Repraicer.Model;
using Repraicer.Services;

namespace Repraicer.ViewModels
{
    public class MainWindowViewModel : INPCBase
    {
        //  private BrowserWndowViewModel browserWndowViewModel = BrowserWndowViewModel.Instance;
        private ObservableCollection<Product> data;

        private Product currentData;
        private int currentPos;
        private DispatcherTimer _timer = new DispatcherTimer();
        private int _timerInterval = 5;
        private Process process;
        private List<string> sortItems = new List<string>();
        private string _selectedSortItem;

        public MainWindowViewModel()
        {

            data = new ObservableCollection<Product>();
            ParseFilesService service = new ParseFilesService();
            List<Product> products = service.ReadProductsFile("activeListing.txt");
            foreach (Product product in products)
            {
                data.Add(product);
            }

            sortItems.AddRange(new[] { "List Date Newest", "Oldest List Date", "Highest Price", "Lowest Price" });
            _selectedSortItem = sortItems[0];
            _timer.Tick += TimerTick;
            SetTimerInterval();
            //commands
            DecrementCommand = new SimpleCommand<Object, Object>(ExecuteDecrementCommand);
            IncrementCommand = new SimpleCommand<Object, Object>(ExecuteIncrementCommand);
            GenerateFileCommand = new SimpleCommand<Object, Object>(GenerateFile);
            PlayCommand = new SimpleCommand<Object, Object>(PlayCommanExecute);
            PauseCommand = new SimpleCommand<Object, Object>(PauseCommandExecute);

            //  _keepaWindow = new NavigationWindow();
            currentPos = 0;
            CurrentData = products[currentPos];

            //var browser = new BrowserWindow();
            //browser.Show();
        }

        public List<string> SortItems
        {
            get { return sortItems; }
            set
            {
                sortItems = value;
                NotifyPropertyChanged(nameof(SortItems));
            }
        }

        public string SelectedSortItem
        {
            get => _selectedSortItem;
            set
            {
                _selectedSortItem = value;
                SortProducts();

                NotifyPropertyChanged(nameof(SelectedSortItem));
            }
        }

        private void SetTimerInterval()
        {
            _timer.Interval = TimeSpan.FromSeconds(_timerInterval);
        }

        private void PlayCommanExecute(Object parameter)
        {
            _timer.Start();
        }

        private void PauseCommandExecute(Object parameter)
        {
            _timer.Stop();
        }

        private void ExecuteDecrementCommand(Object parameter)
        {
            if (currentPos > 0)
            {
                --currentPos;
                CurrentData = data[currentPos];
            }
        }

        public int CurrentPosition
        {
            get { return currentPos + 1; }
            set
            {
                currentPos = value;
                NotifyPropertyChanged(nameof(CurrentPosition));
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            currentPos++;
            CurrentData = data[currentPos];
        }

        private void ExecuteIncrementCommand(Object parameter)
        {
            if (currentPos < data.Count - 1)
            {
                ++currentPos;
                CurrentData = data[currentPos];
            }
        }

        private void SortProducts()
        {
            switch (SelectedSortItem)
            {
                case "List Date Newest":
                    Data = new ObservableCollection<Product>(Data.OrderBy(d => d.OpenDate).ToList());

                    break;
                case "Oldest List Date":
                    Data = new ObservableCollection<Product>(Data.OrderByDescending(d => d.OpenDate).ToList());
                    break;
                case "Highest Price":
                    Data = new ObservableCollection<Product>(Data.OrderByDescending(d => d.Price).ToList());
                    break;
                case "Lowest Price":
                    Data = new ObservableCollection<Product>(Data.OrderBy(d => d.Price).ToList());
                    break;
            }
            CurrentPosition = 0;
            CurrentData = Data[currentPos];
        }

        private void GenerateFile(Object parameter)
        {
            string delimiter = "\t";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("sku" + delimiter + "price" + delimiter + "quantity");
            foreach (Product product in data)
            {
                if (!string.IsNullOrEmpty(product.NewPrice) && product.NewPrice != product.Price)
                {
                    sb.AppendLine(product.SellerSku + delimiter + product.NewPrice);
                }
            }
            using (StreamWriter file =
                new StreamWriter("newPrice.txt"))
            {
                file.Write(sb);

            }


        }

        public ICommand DecrementCommand { get; private set; }
        public ICommand IncrementCommand { get; private set; }

        public ICommand GenerateFileCommand { get; }

        public ICommand PlayCommand { get; }
        public ICommand PauseCommand { get; }

        public ObservableCollection<Product> Data
        {
            get { return data; }
            set
            {
                data = value;
                NotifyPropertyChanged(nameof(Data));
            }
        }

        public int TimerInterval
        {
            get { return _timerInterval; }
            set
            {
                _timerInterval = value;
                SetTimerInterval();
                NotifyPropertyChanged(nameof(TimerInterval));
            }
        }

        public Product CurrentData
        {
            get { return currentData; }
            set
            {
                if (currentData != value)
                {
                    currentData = value;
                    CurrentPosition = data.IndexOf(currentData);

                    //  browserWndowViewModel.ProductUri = new Uri("https://www.amazon.com/gp/offer-listing/" + currentData.asin1 + "/", UriKind.Absolute);
                    Process.Start("https://www.amazon.com/gp/offer-listing/" + currentData.asin1 + "/ref=olp_f_primeEligible?ie=UTF8&f_all=true&f_primeEligible=true");
                    //    process = Process.Start("https://keepa.com/#!product/1-" + currentData.asin1 + "/");
                    //    if(currentPos > 0) CloseTab();
                    NotifyPropertyChanged("CurrentData");



                }
            }
        }

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow([In()] IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        byte W = 0x57; //the keycode for the W key

        public static void Send(byte KeyCode, bool Ctrl, bool Alt, bool Shift, bool Win)
        {
            byte Keycode = (byte)KeyCode;

            uint KEYEVENTF_KEYUP = 2;
            byte VK_CONTROL = 0x11;
            byte VK_MENU = 0x12;
            byte VK_LSHIFT = 0xA0;
            byte VK_LWIN = 0x5B;

            if (Ctrl)
                keybd_event(VK_CONTROL, 0, 0, 0);
            if (Alt)
                keybd_event(VK_MENU, 0, 0, 0);
            if (Shift)
                keybd_event(VK_LSHIFT, 0, 0, 0);
            if (Win)
                keybd_event(VK_LWIN, 0, 0, 0);

            //true keycode
            keybd_event(Keycode, 0, 0, 0); //down
            keybd_event(Keycode, 0, KEYEVENTF_KEYUP, 0); //up

            if (Ctrl)
                keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);
            if (Alt)
                keybd_event(VK_MENU, 0, KEYEVENTF_KEYUP, 0);
            if (Shift)
                keybd_event(VK_LSHIFT, 0, KEYEVENTF_KEYUP, 0);
            if (Win)
                keybd_event(VK_LWIN, 0, KEYEVENTF_KEYUP, 0);

        }

        private void CloseTab()
        {
            SetForegroundWindow(process.MainWindowHandle);
            Task.Delay(500);
            Send(W, true, false, false, false); //Ctrl+W
        }
    }
}
