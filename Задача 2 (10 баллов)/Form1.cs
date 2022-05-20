using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Задача_2__10_баллов_
{
    public partial class Form1 : Form
    {
        // Создаём массивы кнопок, присутствующих судей и поле для номера фигуриста.
        NumericUpDown[] mark;
        Button[] ratebutton;
        bool[] jpresent;
        Button[] presentbutton;
        int count;
        bool cansave; // Это надо для предотвращения сохранения неоценённого фигуриста, которое вызывает баг.
        // Задаём начальные значения массивам, кнопкам и задаём номер первого фигуриста.
        public Form1()
        {
            InitializeComponent();
            jpresent = new bool[10];
            for(int i = 0; i < 10; i++) { jpresent[i] = false; }
            mark = new NumericUpDown[] { j1mark, j2mark, j3mark, j4mark, j5mark, j6mark, j7mark, j8mark, j9mark, j10mark };
            ratebutton = new Button[] { j1rate, j2rate, j3rate, j4rate, j5rate, j6rate, j7rate, j8rate, j9rate, j10rate };
            presentbutton = new Button[] { j1present, j2present, j3present, j4present, j5present, j6present, j7present, j8present, j9present, j10present };
            foreach(Button button in ratebutton) { button.Enabled = false; }
            foreach(NumericUpDown m in mark) { m.Enabled = false; }
            count = 1;
        }
        // Создаём списки для оценок и для итоговых оценок.
        List<decimal> rates = new List<decimal>();
        List<decimal> finalrates = new List<decimal>();

        private void presentswitch(int i) // Этот метод нужен, чтобы судья мог занять своё место. 
        { // Если он присутствует, убираем его, а если нет - добавляем.
            if (jpresent[i])
            {
                jpresent[i] = false;
                ratebutton[i].Enabled = false;
                mark[i].Enabled = false;
                presentbutton[i].Text = "Сесть";
            }
            else
            {
                jpresent[i] = true;
                ratebutton[i].Enabled = true;
                mark[i].Enabled = true;
                presentbutton[i].Text = "Уйти";
            }
        }

        private void rate(int i) // Этот метод нужен, чтобы добавить оценку в список.
        {
            rates.Add(mark[i].Value);
            ratebutton[i].Enabled = false;
            mark[i].Enabled = false;
        }

        private void thankyounext_Click(object sender, EventArgs e) // Вот тут уже интереснее
        {
            if (rates.Count() > 2)
            {
                rates.Sort(); // Сортируем список оценок.
                rates.Remove(rates[rates.Count() - 1]); // Удаляем первую и последнюю (наибольшую и наименьшую) оценки.
                rates.Remove(rates[0]);
                finalrates.Add(Math.Round(rates.Sum() / rates.Count, 1)); // Добавляем округлённое среднее арифметическое значение получившегося списка.
                rates.Clear(); // Очищаем список для следующего участника
                count++; // Меняем номер участника
                label9.Text = $"Выступает фигурист {count}"; // Обновляем текст.
                for (int i = 0; i < 10; i++)
                {
                    if (jpresent[i])
                    {
                        ratebutton[i].Enabled = true;
                        mark[i].Enabled = true;
                    }
                    mark[i].Value = 0;
                }
                cansave = true;
            }
            else
            {
                MessageBox.Show("У фигуриста слишком мало оценок. Посадите больше судей и (или) попросите их поставить оценки.", "Ошибка");
                cansave = false;
            }
        }

        private void End_Click(object sender, EventArgs e) // Записываем результаты в текстовый файл и удаляем всех судей
        {
            thankyounext_Click(sender, e);
            if (cansave) // Нет, услолвия нельзя написать через &, иначе сейвфайлдиалог будет вызываться даже когда не нужен,
            {//             что может ввести в заблуждение пользователя.
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter streamwriter = new StreamWriter(saveFileDialog1.FileName); // Не забыть закрьть поток. Не забыть закрыть поток.
                    for (int i = 0; i < finalrates.Count(); i++)                            // НЕ ЗАБЫТЬ ЗАКРЫТЬ !@#$ЫЙ ПОТОК!!!
                    {
                        streamwriter.WriteLine($"Результат фигуриста {i + 1} - {finalrates[i]}");
                    }
                    streamwriter.Close();
                    MessageBox.Show("Результаты сохранены в текстовый файл.", "Готово!");
                    foreach (Button button in ratebutton) { button.Enabled = false; }
                    foreach (NumericUpDown m in mark) { m.Enabled = false; m.Value = 0; }
                    foreach (Button button in presentbutton) { button.Text = "Сесть"; }
                    count = 1;
                    label9.Text = $"Выступает фигурист {count}";
                    for (int i = 0; i < 10; i++) { jpresent[i] = false; }
                }
            }
        }
        /* Далее идёт абсолютно неупорядоченный набор обработчиков.
         * По сути ничего интересного, просто используются методы "Сесть/Уйти" и "Оценить", описанные выше.
         * Ещё там описан обработчик кнопки выхода из приложения, но это уже совсем скукотища и в пояснениях не нуждается.
         */
        private void j1present_Click(object sender, EventArgs e) 
        { 
            presentswitch(0); 
        }

        private void j1rate_Click(object sender, EventArgs e) 
        { 
            rate(0); 
        }

        private void j2rate_Click(object sender, EventArgs e) 
        { 
            rate(1); 
        }

        private void j2present_Click(object sender, EventArgs e)
        {
            presentswitch(1);
        }

        private void j3present_Click(object sender, EventArgs e)
        {
            presentswitch(2);
        }

        private void j3rate_Click(object sender, EventArgs e)
        {
            rate(2);
        }

        private void j4rate_Click(object sender, EventArgs e)
        {
            rate(3);
        }

        private void j5rate_Click(object sender, EventArgs e)
        {
            rate(4);
        }

        private void j4present_Click(object sender, EventArgs e)
        {
            presentswitch(3);
        }

        private void j5present_Click(object sender, EventArgs e)
        {
            presentswitch(4);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void j6rate_Click(object sender, EventArgs e)
        {
            rate(5);
        }

        private void j7rate_Click(object sender, EventArgs e)
        {
            rate(6);
        }

        private void j8rate_Click(object sender, EventArgs e)
        {
            rate(7);
        }

        private void j9rate_Click(object sender, EventArgs e)
        {
            rate(8);
        }

        private void j10rate_Click(object sender, EventArgs e)
        {
            rate(9);
        }

        private void j6present_Click(object sender, EventArgs e)
        {
            presentswitch(5);
        }

        private void j7present_Click(object sender, EventArgs e)
        {
            presentswitch(6);
        }

        private void j8present_Click(object sender, EventArgs e)
        {
            presentswitch(7);
        }

        private void j9present_Click(object sender, EventArgs e)
        {
            presentswitch(8);
        }

        private void j10present_Click(object sender, EventArgs e)
        {
            presentswitch(9);
        }
    }
}
