using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Проблемы_синхронизации_WinForn
{
    public partial class Form1 : Form
    {
        private Mutex mutex = new Mutex();
        private Thread thread1;
        private Thread thread2;
        public Form1()
        {
            InitializeComponent();
        }

        private void start_but_Click(object sender, EventArgs e)
        {
            
            thread1 = new Thread(Potok_One);
            thread1.Start();

            thread2 = new Thread(Potok_Two);
            thread2.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Завершаем выполнение потоков
            if (thread1 != null && thread1.IsAlive)
                thread1.Abort();

            if (thread2 != null && thread2.IsAlive)
                thread2.Abort();
        }
        private void Potok_One()
        {
            mutex.WaitOne();
            for (int i = 0; i <= 20; i++)
            {
                AppendText(textBox1, $"Поток 1: {i}");
                Thread.Sleep(100);
            }
            mutex.ReleaseMutex();
        }

        private void Potok_Two()
        {
            mutex.WaitOne();
            for (int i = 10; i >= 0; i--)
            {
                AppendText(textBox2, $"Поток 2: {i}");
                Thread.Sleep(100);
            }
            mutex.ReleaseMutex();
        }

        private void AppendText(TextBox textBox, string text)
        {
            // Обеспечиваем доступ к элементу управления из другого потока
            if (textBox2.InvokeRequired)
            {
                textBox.Invoke(new MethodInvoker(() => AppendText(textBox, text)));
            }
            else
            {
                textBox.AppendText(text + Environment.NewLine);
            }
        }
    }
}
