using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimplApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            PrimeList.Items.Clear();
            FibonacciList.Items.Clear();

            if (!int.TryParse(StartBox.Text, out int start) ||
                !int.TryParse(EndBox.Text, out int end))
            {
                MessageBox.Show("Please enter valid numbers.");
                return;
            }

            if (start < 2 || start > end)
            {
                MessageBox.Show("Start must be >= 2 and less than End ");
                return;
            }
            // потік для простих
            Thread primeThread = new Thread(() => FindPrimes(start, end));
            primeThread.IsBackground = true;
            primeThread.Start();

            // потік для фібоначі 
            Thread fibThread = new Thread(() => FindFibonacci(start, end));
            fibThread.IsBackground = true;
            fibThread.Start();
        }
        private void FindPrimes(int start, int end)
        {
            int count = 0;
            for (int number = start; number <= end; number++)
            {
                if (IsPrime(number))
                {
                    count++; 
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        PrimeList.Items.Add(number);
                    });

                    Thread.Sleep(50); 
                }
            }
            // не обовязково але гарно виглядає як крапка в списку 
            Application.Current.Dispatcher.Invoke(() =>
            {
                PrimeList.Items.Add($"Total Prime numbers: {count}");
            });
        }

        private bool IsPrime(int n)
        {
            if (n < 2) return false;

            for (int i = 2; i <= Math.Sqrt(n); i++)
            {
                if (n % i == 0)
                    return false;
            }

            return true;
        }
        private void FindFibonacci(int start, int end)
        {
            int a = 0;
            int b = 1;
            int count = 0;
            while (a <= end)
            {
                if (a >= start)
                {
                    count++;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        FibonacciList.Items.Add(a);
                    });

                    Thread.Sleep(50); 
                }

                int tmp = a + b;
                a = b;
                b = tmp;
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                FibonacciList.Items.Add($"Total Fibonacci numbers: {count}");
            });
        }
    }
}