using Repraicer.Commands;
using Repraicer.Model;
using Repraicer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;

namespace Repraicer.ViewModels
{
    public class MainWindowViewModel : InpcBase
    {
        private ObservableCollection<Product> _data;

        private Product _currentData;

        private int _currentPos;

        private readonly DispatcherTimer _timer = new DispatcherTimer();

        private int _timerInterval = 5;

        private List<string> _sortItems = new List<string>();

        private string _selectedSortItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            _data = new ObservableCollection<Product>();

            var service = new ParseFilesService();
            var products = service.ReadProductsFile("activeListing.txt");

            foreach (var product in products)
            {
                _data.Add(product);
            }

            _sortItems.AddRange(new[]
            {
                "List Date Newest",
                "Oldest List Date",
                "Highest Price",
                "Lowest Price"
            });

            _timer.Tick += TimerTick;
            SetTimerInterval();

            //commands
            GenerateFileCommand = new SimpleCommand<object, object>(GenerateFile);
            PlayCommand = new SimpleCommand<object, object>(PlayCommanExecute);
            PauseCommand = new SimpleCommand<object, object>(PauseCommandExecute);

            _currentPos = 3;
            CurrentData = products[_currentPos];
        }

        /// <summary>
        /// Gets or sets the sort items.
        /// </summary>
        /// <value>
        /// The sort items.
        /// </value>
        public List<string> SortItems
        {
            get => _sortItems;
            set
            {
                _sortItems = value;
                NotifyPropertyChanged(nameof(SortItems));
            }
        }

        /// <summary>
        /// Gets or sets the selected sort item.
        /// </summary>
        /// <value>
        /// The selected sort item.
        /// </value>
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

        /// <summary>
        /// Sets the timer interval.
        /// </summary>
        private void SetTimerInterval() => _timer.Interval = TimeSpan.FromSeconds(_timerInterval);

        private void PlayCommanExecute(object parameter) => _timer.Start();

        /// <summary>
        /// Pauses the command execute.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void PauseCommandExecute(object parameter) => _timer.Stop();

        /// <summary>
        /// Gets or sets the current position.
        /// </summary>
        /// <value>
        /// The current position.
        /// </value>
        public int CurrentPosition
        {
            get => _currentPos + 1;
            set
            {
                _currentPos = value;
                NotifyPropertyChanged(nameof(CurrentPosition));
            }
        }

        /// <summary>
        /// Timers the tick.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void TimerTick(object sender, EventArgs e)
        {
            _currentPos++;

            if (_currentPos >= _data.Count)
            {
                _currentPos = 0;
            }

            CurrentData = _data[_currentPos];
        }

        /// <summary>
        /// Sorts the products.
        /// </summary>
        private void SortProducts()
        {
            switch (SelectedSortItem)
            {
                case "List Date Newest":
                    {
                        Data = new ObservableCollection<Product>(Data.OrderByDescending(d => d.OpenDate).ToList());
                    }
                    break;

                case "Oldest List Date":
                    {
                        Data = new ObservableCollection<Product>(Data.OrderBy(d => d.OpenDate).ToList());
                    }
                    break;

                case "Highest Price":
                    {
                        Data = new ObservableCollection<Product>(Data.OrderByDescending(d => d.Price).ToList());
                    }
                    break;

                case "Lowest Price":
                    {
                        Data = new ObservableCollection<Product>(Data.OrderBy(d => d.Price).ToList());
                    }
                    break;
            }

            CurrentPosition = 3;
            CurrentData = Data[_currentPos];
        }

        /// <summary>
        /// Generates the file.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void GenerateFile(object parameter)
        {
            const string delimiter = "\t";
            var sb = new StringBuilder();

            sb.AppendLine("sku" + delimiter + "price" + delimiter + "quantity");
            foreach (var product in _data)
            {
                if (product.NewPrice != product.Price)
                {
                    sb.AppendLine(product.SellerSku + delimiter + product.NewPrice + delimiter + product.Quantity);
                }
            }

            using (var file = new StreamWriter("newPrice.txt"))
            {
                file.Write(sb);
            }
        }

        public ICommand GenerateFileCommand { get; }

        public ICommand PlayCommand { get; }
        public ICommand PauseCommand { get; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public ObservableCollection<Product> Data
        {
            get => _data;
            set
            {
                _data = value;
                NotifyPropertyChanged(nameof(Data));
            }
        }

        /// <summary>
        /// Gets or sets the timer interval.
        /// </summary>
        /// <value>
        /// The timer interval.
        /// </value>
        public int TimerInterval
        {
            get => _timerInterval;
            set
            {
                _timerInterval = value;

                SetTimerInterval();
                NotifyPropertyChanged(nameof(TimerInterval));
            }
        }

        /// <summary>
        /// Gets or sets the current data.
        /// </summary>
        /// <value>
        /// The current data.
        /// </value>
        public Product CurrentData
        {
            get => _currentData;
            set
            {
                if (_currentData == value)
                {
                    return;
                }

                _currentData = value;
                CurrentPosition = _data.IndexOf(_currentData);

                Process.Start("https://www.amazon.com/gp/offer-listing/" + _currentData.Asin1 + "/ref=olp_f_primeEligible?ie=UTF8&f_all=true&f_primeEligible=true");
                NotifyPropertyChanged("CurrentData");
            }
        }

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow([In()] IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        /// <summary>
        /// Sends the specified key code.
        /// </summary>
        /// <param name="keyCode">The key code.</param>
        /// <param name="ctrl">if set to <c>true</c> [control].</param>
        /// <param name="alt">if set to <c>true</c> [alt].</param>
        /// <param name="shift">if set to <c>true</c> [shift].</param>
        /// <param name="win">if set to <c>true</c> [win].</param>
        public static void Send(byte keyCode, bool ctrl, bool alt, bool shift, bool win)
        {
            byte keycode = (byte)keyCode;

            uint KEYEVENTF_KEYUP = 2;
            byte VK_CONTROL = 0x11;
            byte VK_MENU = 0x12;
            byte VK_LSHIFT = 0xA0;
            byte VK_LWIN = 0x5B;

            if (ctrl)
            {
                keybd_event(VK_CONTROL, 0, 0, 0);
            }

            if (alt)
            {
                keybd_event(VK_MENU, 0, 0, 0);
            }

            if (shift)
            {
                keybd_event(VK_LSHIFT, 0, 0, 0);
            }

            if (win)
            {
                keybd_event(VK_LWIN, 0, 0, 0);
            }

            //true keycode
            keybd_event(keycode, 0, 0, 0); //down
            keybd_event(keycode, 0, KEYEVENTF_KEYUP, 0); //up

            if (ctrl)
            {
                keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);
            }

            if (alt)
            {
                keybd_event(VK_MENU, 0, KEYEVENTF_KEYUP, 0);
            }

            if (shift)
            {
                keybd_event(VK_LSHIFT, 0, KEYEVENTF_KEYUP, 0);
            }

            if (win)
            {
                keybd_event(VK_LWIN, 0, KEYEVENTF_KEYUP, 0);
            }
        }
    }
}