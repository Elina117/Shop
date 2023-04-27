using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Net.Mail;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Threading;
using Npgsql;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Shop.project
{
    public partial class Form1 : Form
    {
        private User user;
        public Random reserve_code;

        private List<string> style = new List<string>() { "Класический", "Спортивный", "Домашний", "Рабочий", "Специальный"};
        private List<string> sex = new List<string>() { "Мужской", "Женсикй", "Унисекс" };
        private List<string> season = new List<string>() { "Зима", "Осень/Весна", "Лето", "Демисезон"};
        private List<string> material = new List<string>() { "Хлопок", "Синтетика", "Кожа", "Мех" };
        private List<string> color = new List<string>() { "Белый", "Черный", "Красный", "Синий", "Зеленый", "Желтый" };

        private string add_style, add_sex, add_season, add_material, add_color;

        private void Init()
        {
            reserve_code = new Random();
            user = new User();
            textBox_password.UseSystemPasswordChar = true;
            repeat_new_password.UseSystemPasswordChar = true;
        }


        public Form1()
        {
            InitializeComponent();
            Init();
        }
        //Покдлючение БД
        private static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Server=ep-lucky-rice-772590.us-east-2.aws.neon.tech;Port=5432;User Id=elina3galimova;Password=6mSLTHBxAth1;Database=registration");
        }


        //поле ввода логина вход
        private void textBox_login_TextChanged(object sender, EventArgs e)
        {
            if (textBox_login.BackColor != Color.White)
            {
                textBox_login.BackColor = Color.White;
            }

        }
        //поле ввода пароля вход
        private void textBox_password_TextChanged(object sender, EventArgs e)
        {
            if (textBox_password.BackColor != Color.White)
                textBox_password.BackColor = Color.White;
        }

        //rнопка перехода к регистрации
        private void registration_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            textBox_ligin_in.Clear();
            textBox_email_in.Clear();
            textBox_password_in.Clear();
            checkBox_agryment.Checked = false;
        }

        private void forgot_password_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }


        //отправка кода на email
        private void button5_Click(object sender, EventArgs e)
        {
            NpgsqlConnection conn = GetConnection();
            NpgsqlCommand cmd = conn.CreateCommand();

            if (textBox_email.Text == string.Empty | textBox_reserve_login.Text == string.Empty)
            {
                textBox_email.BackColor = Color.Red;
                textBox_reserve_login.BackColor = Color.Red;
            }

            else
            {
                SendCode_Mail(conn, cmd, textBox_email);
                Program.Write($"\nINFO {DateTime.Now} Было отправлено электронное письмо на почту {textBox_email.Text}");
            }

        }

        //подборка имэйла для отправки кода
        private void SendCode_Mail(NpgsqlConnection conn, NpgsqlCommand cmd, TextBox textBox_email)
        {
            if (textBox_email.Text != string.Empty)
            {
                try
                {

                    conn.Open();

                    cmd.CommandText = $"SELECT EXISTS (SELECT * from user WHERE email = '{textBox_email.Text}')";
                    NpgsqlDataReader dr = cmd.ExecuteReader();

                    if (!dr.Read())
                    {
                        MessageBox.Show("Ошибка подключения с базой данных!\nПопробуйте ещё раз!");
                        return;
                    }
                    if (dr.GetBoolean(0) == true)
                    {

                        dr.Close();

                        cmd.CommandText = $"SELECT email FROM user WHERE login = '{textBox_reserve_login.Text}'";
                        dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {

                            string email = dr.GetString(0);

                            user.code = reserve_code.Next(1000, 10000);

                            string text = $"<b>Ваш резервный код:<br>{user.code}</br>.<br>Если код пришел вам случайно, никому не отправляйте этот код!</br></b>";
                            SendMessage(email, "Восстановление пароля", text);

                            MessageBox.Show("Код успешно отправлен на почту, указанную Вами в личном кабинете!\n(Возможно код лежит в папке \"СПАМ\")\nВАЖНО: после того, как вы ввели код, если он правильный, то вы автоматиечски попадёте во вкладку восстановления пароля, никакие другие кнопки нажимать не нужно!");
                            tabControl1.SelectedIndex = 3;
                            Program.Write($"\n[ERROR] {DateTime.Now} Резервный код отправлен на почту");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ошибка! Данный пользователь не существует!");
                        Program.Write($"\nINFO {DateTime.Now} Пользователь {textBox_reserve_login.Text} не существует при попытке отправить резервный код");
                    }


                }

                catch (NpgsqlException ex)
                {
                    MessageBox.Show("Ошибка подключения к серверу, попробуйте ещё раз!\n" + ex.Message);
                    Program.Write($"\n[ERROR] {DateTime.Now} Ошибка подключения к серверу");
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        //метод для соединения с имэйлом
        public void SendMessage(string user_email, string theme, string text)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.mail.ru");
                smtpClient.UseDefaultCredentials = true;
                smtpClient.EnableSsl = true;

                System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential("store.pm@mail.ru", "ddCUxQBjt5aYCcNLW2Ly");
                smtpClient.Credentials = networkCredential;

                MailAddress from = new MailAddress("store.pm@mail.ru", "Store");
                MailAddress to = new MailAddress(user_email, "for you");
                MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

                MailAddress replyTo = new MailAddress("store.pm@mail.ru");
                myMail.ReplyToList.Add(to);

                myMail.Subject = theme;
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                myMail.Body = $"{text}";
                myMail.BodyEncoding = System.Text.Encoding.UTF8;

                myMail.IsBodyHtml = true;

                smtpClient.Send(myMail);

                smtpClient.Dispose();
            }
            catch (SmtpException)
            {
                MessageBox.Show("Ошибка отправки сообщения, попробуйте ещё раз!");
                Program.Write($"\n[INFO] {DateTime.Now} Ошибка отправки сообщения на почту");
            }
        
        }
        private void button6_Click(object sender, EventArgs e)
        {
            string code = user.code.ToString();

            if (code.Equals(textBox_enter_reserve_code.Text))
            {
                tabControl1.SelectedIndex = 4;
            }
            else
            {
                MessageBox.Show("Введеный код не совпадает с отправленным!");
                Program.Write($"\n[INFO] {DateTime.Now} введеный код {textBox_enter_reserve_code.Text} не совпадает с отправленным");
            }
        
        }

        private void button_enter_Click(object sender, EventArgs e)
        {
            if (textBox_login.Text == string.Empty && textBox_password.Text == string.Empty)
            {
                textBox_login.BackColor = Color.Red;
                textBox_password.BackColor = Color.Red;
                MessageBox.Show("Неверно заполнены поля!");
                return;
            }

            pr.PerformStep();

            NpgsqlConnection conn = GetConnection();
            pr.PerformStep();
            NpgsqlCommand cmd = conn.CreateCommand();
            pr.PerformStep();
            cmd.CommandText = $"SELECT \"id\" FROM \"user\" WHERE login = '{textBox_login.Text}' AND password = '{textBox_password.Text}'";
            pr.PerformStep();
            try
            {
                pr.PerformStep();
                conn.Open();
                pr.PerformStep();

                if (conn.State == ConnectionState.Open)
                {
                    pr.PerformStep();
                    NpgsqlDataReader dr = cmd.ExecuteReader();
                    pr.PerformStep();
                    if (dr.Read())
                    {
                        pr.PerformStep();
                        textBox_login.Text = string.Empty;
                        textBox_password.Text = string.Empty;
                        pr.PerformStep();
                        user.id = dr.GetInt32(0);
                        PersonalSpace();
                        pr.Value = 0;

                        tabControl1.SelectedIndex = 5;

                        Program.Write($"\nINFO {DateTime.Now} Пользователь {textBox_login.Text} зашел в личный кабинет");
                    }
                    else
                    {
                        textBox_login.BackColor = Color.Red;
                        textBox_password.BackColor = Color.Red;
                        MessageBox.Show("Неправильный логин или пароль!");

                        Program.Write($"\n[INFO] {DateTime.Now} Пользовательно {textBox_login.Text} неверно ввел логин или пароль");
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            finally
            {
                conn.Close();

            }

        }

        private void textBox_ligin_in_TextChanged(object sender, EventArgs e)
        {
            int cnt = 0;

            if (textBox_ligin_in.Text != string.Empty)
            {
                foreach (char i in textBox_ligin_in.Text.ToString())
                {
                    cnt++;
                }

                if (cnt <= 10)
                {
                    textBox_ligin_in.ForeColor = Color.Green;
                }
                else
                {
                    textBox_ligin_in.ForeColor = Color.Red;
                }

            }
            else
            {
                textBox_ligin_in.ForeColor = Color.Red;
            }
        }

        private void textBox_email_TextChanged(object sender, EventArgs e)
        {
            string mail = textBox_email_in.Text;
            if (mail.Contains("@") && textBox_email_in.Text != string.Empty && mail.Length <= 35)
            {
                textBox_email_in.ForeColor = Color.Green;
            }
            else
            {
                textBox_email_in.ForeColor = Color.Red;
            }
        }

        private void textBox_password_in_TextChanged(object sender, EventArgs e)
        {
            string password = textBox_password.Text;
            int cnt = 0;

            if (textBox_password_in.Text != string.Empty && password.Contains(" ") == false)
            {
                foreach (char i in textBox_password_in.Text.ToString())
                {
                    cnt++;
                }
                if (cnt < 8)
                {
                    textBox_password_in.ForeColor = Color.Red;
                }
                else
                {
                    textBox_password_in.ForeColor = Color.Green;
                }

            }
            else
            {
                textBox_password_in.ForeColor = Color.Red;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (textBox_password_in.Text == string.Empty | textBox_ligin_in.Text == string.Empty | textBox_email_in.Text == string.Empty)
            {
                textBox_email_in.BackColor = Color.Red;
                textBox_ligin_in.BackColor = Color.Red;
                textBox_password_in.BackColor = Color.Red;

                MessageBox.Show("Поля не заполнены, попробуйте ввести данные");
                Program.Write($"\n[INFO] {DateTime.Now} Поля не заполнены");

                return;
            }


            NpgsqlConnection conn = GetConnection();
            NpgsqlCommand cmd = conn.CreateCommand();
            NpgsqlDataReader reader;

            try
            {

                conn.Open();

                if (conn.State == ConnectionState.Open)
                {

                    cmd.CommandText = $"SELECT EXISTS (SELECT * from \"user\" WHERE email = '{textBox_email_in.Text}')";
                    reader = cmd.ExecuteReader();

                    if (reader.Read() && reader.GetBoolean(0) == true)
                    {
                        MessageBox.Show("Данная почта уже занята!");
                        Program.Write($"\n[INFO] {DateTime.Now} Данная почта {textBox_email_in.Text} уже занята");
                        return;
                    }
                    reader.Close();

                    cmd.CommandText = $"SELECT EXISTS(SELECT * from \"user\" WHERE login = '{textBox_ligin_in.Text}');";
                    reader = cmd.ExecuteReader();

                    if (reader.Read() && reader.GetBoolean(0) == true)
                    {
                        MessageBox.Show("Логин уже занят!");
                        Program.Write($"\n[INFO] {DateTime.Now} Данный логин {textBox_ligin_in.Text} уже занят");
                        return;
                    }
                    reader.Close();



                    cmd.CommandText = $"INSERT INTO \"user\"(login, password, email) values ('{textBox_ligin_in.Text}', '{textBox_password_in.Text}', '{textBox_email_in.Text}')";
                    reader = cmd.ExecuteReader();


                    MessageBox.Show($"Форма заполнена верно");
                    Program.Write($"\n[INFO] {DateTime.Now} Форма заполнена верно, новый пользователь зарегистрирован");

                    tabControl1.SelectedIndex = 0;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к серверу, попробуйте ещё раз!\n" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NpgsqlConnection conn = GetConnection();
            NpgsqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = $"SELECT COUNT(fk_characteristic_id) FROM \"user\" WHERE \"id\" = {user.id}";

            try
            {
                conn.Open();

                NpgsqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (dr.GetInt32(0) == 0)
                    {
                        MessageBox.Show("Сначала настройте рекомендации!\n(Настройка рекомендации)");
                        Program.Write($"\n[INFO] {DateTime.Now} Необходимо заполнить сначала рекомендации");
                    } else
                    {
                        tabControl1.SelectedIndex = 6;
                    }
                }
            } catch(NpgsqlException ex) {
                MessageBox.Show("Ошибка!\n" + ex);
            }
            finally { conn.Close(); }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 7;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 8;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 9;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 10;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 6;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 7;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 8;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 6;
        }

        private void button26_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 7;
        }

        private void button27_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 8;
        }

        private void button28_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 6;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 7;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 8;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 11;
        }

        private void button29_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 6;
        }

        private void button30_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 7;
        }

        private void button31_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 8;
        }

        private void button32_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void button33_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void button34_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 9;
        }

        private void button35_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 10;
        }

        private void button36_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 6;
        }

        private void button37_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 7;
        }

        private void button38_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 8;
        }

        private void button39_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void button40_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void button41_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 9;
        }

        private void button42_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 10;
        }

        private void label32_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 8;
        }

        private void new_password_TextChanged(object sender, EventArgs e)
        {
            string password = new_password.Text;
            int cnt = 0;

            if (new_password.Text != string.Empty && password.Contains(" ") == false)
            {
                foreach (char i in new_password.Text.ToString())
                {
                    cnt++;
                }
                if (cnt < 8)
                {
                    new_password.ForeColor = Color.Red;
                }
                else
                {
                    new_password.ForeColor = Color.Green;
                }

            }
            else
            {
                new_password.ForeColor = Color.Red;
            }
        }

        private void repeat_new_password_TextChanged(object sender, EventArgs e)
        {
            string password = repeat_new_password.Text;
            int cnt = 0;

            if (repeat_new_password.Text != string.Empty && password.Contains(" ") == false)
            {
                foreach (char i in repeat_new_password.Text.ToString())
                {
                    cnt++;
                }
                if (cnt < 8)
                {
                    repeat_new_password.ForeColor = Color.Red;
                }
                else
                {
                    repeat_new_password.ForeColor = Color.Green;
                }


            }
            else
            {
                repeat_new_password.ForeColor = Color.Red;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            


            if (!new_password.Text.Equals(repeat_new_password.Text))
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }

            NpgsqlConnection conn = GetConnection();
            NpgsqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"UPDATE user SET password = '{new_password.Text}' WHERE login = '{textBox_reserve_login.Text}'";


            try
            {
                conn.Open();

                if (cmd.ExecuteNonQuery() != -1)
                {
                    PersonalSpace();

                    MessageBox.Show("Вы успешно обновили пароль!");
                    Program.Write($"\n[INFO] {DateTime.Now} Данный пользователь {textBox_reserve_login.Text} обновил пароль на {new_password.Text}");
                    tabControl1.SelectedIndex = 0;

                }

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Ошибка подключения к серверу, попробуйте ещё раз!\n" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {


        }

        private void label33_Click(object sender, EventArgs e)
        {
            textBox_lc_login.Enabled = true;
            textBox_lc_name.Enabled = true;
            textBox_lc_email.Enabled = true;
            textBox_lc_birthday.Enabled = true;
            MessageBox.Show("Вы в режиме редактирования\nПосле изменения данных кликните по кнопке 'Сохранить' для сохранения изменений");
            Program.Write($"\n[INFO] {DateTime.Now} Пользователь {user.login} в режиме редактирования");
            label_end_of_changing.Visible = true;
            label33.Visible = false;

        }



        private void label35_Click(object sender, EventArgs e)
        {
            bool correct = true;

            if (textBox_lc_login.Text == string.Empty | textBox_lc_login.Text.Contains(" ") | textBox_lc_login.Text.Length > 10)
            {
                textBox_lc_login.BackColor = Color.Red;
                correct = false;
            }
            if (textBox_lc_name.Text == string.Empty | textBox_lc_name.Text.Contains(" ") | textBox_lc_name.Text.Length > 15)
            {
                textBox_lc_name.BackColor = Color.Red;
                correct = false;
            }
            if (textBox_lc_email.Text == string.Empty | !textBox_lc_email.Text.Contains("@") | textBox_lc_email.Text.Length > 35)
            {
                textBox_lc_email.BackColor = Color.Red;
                correct = false;
            }
            if (textBox_lc_birthday.Text == string.Empty | !textBox_lc_birthday.Text.Contains(".") | textBox_lc_birthday.Text.Length != 10)
            {
                textBox_lc_birthday.BackColor = Color.Red;
                correct = false;
            }


            if (correct)
            {
                NpgsqlConnection conn = GetConnection();
                NpgsqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = $"UPDATE \"user\" SET \"name\" = '{textBox_lc_name.Text}',email = '{textBox_lc_email.Text}', login = '{textBox_lc_login.Text}', birthday = '{textBox_lc_birthday.Text}' WHERE id = {user.id}";

                label_end_of_changing.Visible = false;

                try
                {
                    conn.Open();


                    if (cmd.ExecuteNonQuery() == -1)
                    {
                        MessageBox.Show("Ошибка подключения к серверу, попробуйте ещё раз!");
                    }
                    else
                    {
                        textBox_lc_name.ReadOnly = true;
                        textBox_lc_email.ReadOnly = true;
                        textBox_lc_login.ReadOnly = true;
                        textBox_lc_birthday.ReadOnly = true;


                        textBox_lc_name.BackColor = Color.White;
                        textBox_lc_email.BackColor = Color.White;
                        textBox_lc_login.BackColor = Color.White;
                        textBox_lc_birthday.BackColor = Color.White;

                        conn.Close();

                        textBox_lc_login.Enabled = false;
                        textBox_lc_name.Enabled = false;
                        textBox_lc_email.Enabled = false;
                        textBox_lc_birthday.Enabled = false;

                        MessageBox.Show("Данные обновлены!");
                        Program.Write($"\n[INFO] {DateTime.Now} Пользователь {user.login} обновил данные");
                        label33.Visible = true;

                    }
                }
                catch (NpgsqlException ex)
                {
                    MessageBox.Show("Ошибка подключения к серверу, попробуйте ещё раз!\n" + ex.Message);
                }
                finally { }

            }

        }


        public void PersonalSpace()
        {
            NpgsqlConnection conn = GetConnection();
            NpgsqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM \"user\" WHERE \"id\" = {user.id}";

            try
            {
                conn.Open();

                if (conn.State == ConnectionState.Open)
                {
                    NpgsqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        user.id = dr.GetInt32(0);
                        user.login = dr.GetString(1);
                        user.password = dr.GetString(2);
                        user.email = dr.GetString(3);
                        user.name = dr.GetString(4);
                        user.birthday = dr.GetString(5);

                        textBox_lc_name .Text = user.name;
                        textBox_lc_email.Text = user.email;
                        textBox_lc_login.Text = user.login;
                        textBox_lc_birthday.Text = user.birthday;
                        

                        dr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к серверу, попробуйте ещё раз!\n" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void button_exit_from_lc_Click(object sender, EventArgs e)
        {
            DialogResult dresult = MessageBox.Show("Вы действительно хотите выйти из личного кабинета?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

         
            if (dresult == DialogResult.Yes)
            {
                tabControl1.SelectedIndex = 0;
                Program.Write($"\n[INFO] {DateTime.Now} Пользователь {user.login} вышел из личного кабинета");
            }
        }

        private void button_back_from_forgot_password_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            textBox_reserve_login.Clear();
            textBox_email.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void pictureBox_show_password_Click(object sender, EventArgs e)
        {
            textBox_password.UseSystemPasswordChar = false;        
            pictureBox_show_password.Visible = false;
            pictureBox_hide_password.Visible = true;
        }

        private void pictureBox_hide_password_Click(object sender, EventArgs e)
        {
            textBox_password.UseSystemPasswordChar = true;
            pictureBox_hide_password.Visible = false;
            pictureBox_show_password.Visible = true;
        }

        private void pictureBox_hide_reserve_pass_Click(object sender, EventArgs e)
        {
            repeat_new_password.UseSystemPasswordChar = true;
            pictureBox_hide_reserve_pass.Visible = false;
            pictureBox_show_reserve_pass.Visible = true;
        }

        private void pictureBox_show_reserve_pass_Click(object sender, EventArgs e)
        {
            repeat_new_password.UseSystemPasswordChar = false;
            pictureBox_show_reserve_pass.Visible = false;
            pictureBox_hide_reserve_pass.Visible = true;
        }

        private void button_save_rec_Click(object sender, EventArgs e)
        {
            if (textBox_photo_link.Text == string.Empty || textBox_name_clothe.Text == string.Empty || textBox_discription.Text == string.Empty)
            {
                MessageBox.Show("Заполните пустые поля!");
                return;
            }
            webBrowser1.Url = new Uri(textBox_photo_link.Text);
            webBrowser1.Document.Body.Style = "zoom:50%";
            label_prev_name.Text = textBox_name_clothe.Text;
            label_prev_where_buy.Text = textBox_discription.Text;

            tabControl1.SelectedIndex = 11;
        }

        private void button_back_to_offer_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 8;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage9_Click(object sender, EventArgs e)
        {

        }

        private void button_revoke_Click(object sender, EventArgs e)
        {
            add_color = add_material = add_season = add_sex = add_style = null;
            textBox_name_clothe.Text = string.Empty;
            textBox_photo_link.Text = string.Empty;
            textBox_discription.Text = string.Empty;
        }

        private void comboBox_characteristic_SelectedValueChanged(object sender, EventArgs e)
        {
            panel_add.Visible = true;
            switch (comboBox_characteristic.SelectedItem.ToString())
            {
                case "Стиль":
                    {
                        radioButton_add1.Text = "Класический";
                        radioButton_add2.Text = "Спортивный";
                        radioButton_add3.Text = "Домашний";
                        radioButton_add4.Text = "Рабочий";
                        radioButton_add5.Text = "Специальнынй";
                        radioButton_add6.Visible = false;
                        break;
                    }
                case "Пол":
                    {
                        radioButton_add1.Text = "Мужской";
                        radioButton_add2.Text = "Женский";
                        radioButton_add3.Text = "Унисекс";
                        radioButton_add4.Visible = false;
                        radioButton_add5.Visible = false;
                        radioButton_add6.Visible = false;
                        break;
                    }
                case "Сезон":
                    {
                        radioButton_add1.Text = "Зима";
                        radioButton_add2.Text = "Осень/Весна";
                        radioButton_add3.Text = "Лето";
                        radioButton_add4.Text = "Демисезон";
                        radioButton_add5.Visible = false;
                        radioButton_add6.Visible = false;
                        break;
                    }
                case "Материал":
                    {
                        radioButton_add1.Text = "Хлопок";
                        radioButton_add2.Text = "Синтетика";
                        radioButton_add3.Text = "Кожа";
                        radioButton_add4.Text = "Мех";
                        radioButton_add5.Visible = false;
                        radioButton_add6.Visible = false;
                        break;
                    }
                case "Цвет":
                    {
                        radioButton_add1.Text = "Белый";
                        radioButton_add2.Text = "Черный";
                        radioButton_add3.Text = "Красный";
                        radioButton_add4.Text = "Синий";
                        radioButton_add5.Text = "Зеленый";
                        radioButton_add6.Text = "Желтый";
                        break;
                    }
                default: { break; }
            }
        }

        private void pictureBox_set_style_Click(object sender, EventArgs e)
        {
            panel_set.Visible = true;
            radioButton1.Text = "Класический";
            radioButton2.Text = "Спортивный";
            radioButton3.Text = "Домашний";
            radioButton4.Text = "Рабочий";
            radioButton5.Text = "Специальнынй";
            radioButton6.Visible = false;
        }

        private void SaveCurSet(object sender, EventArgs e)
        {
            string str = string.Empty;
            if (radioButton1.Checked)
            {
                str = radioButton1.Text;
            }
            else if (radioButton2.Checked)
            {
                str = radioButton2.Text;
            }
            else if (radioButton3.Checked)
            {
                str = radioButton3.Text;
            }
            else if (radioButton4.Checked)
            {
                str = radioButton4.Text;
            }
            else if (radioButton5.Checked)
            {
                str = radioButton5.Text;
            } else if (radioButton6.Checked)
            {
                str = radioButton6.Text;
            }

            if (str != string.Empty)
            {
                pictureBox_set_del_style.Visible = true;
                panel_set.Visible = false;

                if (style.Contains(str))
                {
                    label_set_cur_style.Text = str;
                    label_set_cur_style.Visible = true;
                    pictureBox_set_del_style.Visible = true;
                }
                else if (sex.Contains(str))
                {
                    label_set_cur_sex.Text = str;
                    label_set_cur_sex.Visible = true;
                    pictureBox_set_del_sex.Visible = true;
                }
                else if (season.Contains(str))
                {
                    label_set_cur_season.Text = str;
                    label_set_cur_season.Visible = true;
                    pictureBox_set_del_season.Visible = true;
                }
                else if (material.Contains(str))
                {
                    label_set_cur_material.Text = str;
                    label_set_cur_material.Visible = true;
                    pictureBox_set_del_material.Visible = true;
                } else if (color.Contains(str))
                {
                    label_set_cur_color.Text = str;
                    label_set_cur_color.Visible = true;
                    pictureBox_set_del_color.Visible = true;
                }
            }

            radioButton1.Visible = true;
            radioButton2.Visible = true;
            radioButton3.Visible = true;
            radioButton4.Visible = true;
            radioButton5.Visible = true;
            radioButton6.Visible = true;
        }

        private void pictureBox_set_del_style_Click(object sender, EventArgs e)
        {
            label_set_cur_style.Text = string.Empty;
            label_set_cur_style.Visible = false;
            pictureBox_set_del_style.Visible = false;
        }

        private void pictureBox_set_sex_Click(object sender, EventArgs e)
        {
            panel_set.Visible = true;
            radioButton1.Text = "Мужской";
            radioButton2.Text = "Женский";
            radioButton3.Text = "Унисекс";
            radioButton4.Visible = false;
            radioButton5.Visible = false;
            radioButton6.Visible = false;
        }

        private void pictureBox_set_season_Click(object sender, EventArgs e)
        {
            panel_set.Visible = true;
            radioButton1.Text = "Зима";
            radioButton2.Text = "Осень/Весна";
            radioButton3.Text = "Лето";
            radioButton4.Text = "Демисезон";
            radioButton5.Visible = false;
            radioButton6.Visible = false;
        }

        private void pictureBox_set_material_Click(object sender, EventArgs e)
        {
            panel_set.Visible = true;
            radioButton1.Text = "Хлопок";
            radioButton2.Text = "Синтетика";
            radioButton3.Text = "Кожа";
            radioButton4.Text = "Мех";
            radioButton5.Visible = false;
            radioButton6.Visible = false;
        }

        private void pictureBox_set_color_Click(object sender, EventArgs e)
        {
            panel_set.Visible = true;
            radioButton1.Text = "Белый";
            radioButton2.Text = "Черный";
            radioButton3.Text = "Красный";
            radioButton4.Text = "Синий";
            radioButton5.Text = "Зеленый";
            radioButton6.Text = "Желтый";
        }

        private void pictureBox_set_del_sex_Click(object sender, EventArgs e)
        {
            label_set_cur_sex.Text = string.Empty;
            label_set_cur_sex.Visible = false;
            pictureBox_set_del_sex.Visible = false;
        }

        private void pictureBox_set_del_season_Click(object sender, EventArgs e)
        {
            label_set_cur_season.Text = string.Empty;
            label_set_cur_season.Visible = false;
            pictureBox_set_del_season.Visible = false;
        }

        private void pictureBox_set_del_material_Click(object sender, EventArgs e)
        {
            label_set_cur_material.Text = string.Empty;
            label_set_cur_material.Visible = false;
            pictureBox_set_del_material.Visible = false;
        }

        private void button_prev_send_offer_Click(object sender, EventArgs e)
        {
            int ch_style = style.IndexOf(add_style) + 1;
            int ch_sex = sex.IndexOf(add_sex) + 1;
            int ch_season = season.IndexOf(add_season) + 1;
            int ch_material = material.IndexOf(add_material) + 1;
            int ch_color = color.IndexOf(add_color) + 1;

            NpgsqlConnection conn = GetConnection();
            NpgsqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = $"INSERT INTO characteristic(fk_using_id, fk_sex_id, fk_season_id, fk_material_id, fk_color_id) VALUES({ch_style}, {ch_sex}, {ch_season}, {ch_material}, {ch_color})";

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();

                cmd.CommandText = $"SELECT * FROM characteristic ORDER BY id DESC";

                NpgsqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                int char_id = dr.GetInt32(0);
                dr.Close();

                cmd.CommandText = $"INSERT INTO clothe(description, \"like\", \"name\", url, fk_characteristic_id) VALUES('{textBox_discription.Text}', 0, '{textBox_name_clothe.Text}', '{textBox_photo_link.Text}', {char_id})";

                cmd.ExecuteNonQuery();

                MessageBox.Show("Одежда успешно добавлена!");
                Program.Write($"\n[INFO] {DateTime.Now} Одежда добавлена пользователем {user.login}");

                add_color = add_material = add_season = add_sex = add_style = textBox_discription.Text = textBox_name_clothe.Text = textBox_photo_link.Text = string.Empty;

                label_prev_name.Text = label_prev_where_buy.Text = string.Empty;
                webBrowser1.Url = null;

                tabControl1.SelectedIndex = 8;

            } catch (NpgsqlException ex)
            {
                MessageBox.Show("Ошибка!\n" + ex);
            }
            finally { conn.Close(); }
        }

        private void pictureBox_set_del_color_Click(object sender, EventArgs e)
        {
            label_set_cur_color.Text = string.Empty;
            label_set_cur_color.Visible = false;
            pictureBox_set_del_color.Visible = false;
        }

        private void button_set_save_sets_Click(object sender, EventArgs e)
        {
            int ch_style = style.IndexOf(label_set_cur_style.Text) + 1;
            int ch_sex = sex.IndexOf(label_set_cur_sex.Text) + 1;
            int ch_season = season.IndexOf(label_set_cur_season.Text) + 1;
            int ch_material = material.IndexOf(label_set_material.Text) + 1;
            int ch_color = color.IndexOf(label_set_cur_color.Text) + 1;

            NpgsqlConnection conn = GetConnection();
            NpgsqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = $"SELECT COUNT(fk_characteristic_id) FROM \"user\" WHERE \"id\" = {user.id}";

            try
            {
                conn.Open();

                NpgsqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (dr.GetInt32(0) != 0)
                    {
                        dr.Close();
                        cmd.CommandText = $"SELECT fk_characteristic_id FROM \"user\"";

                        dr = cmd.ExecuteReader();
                        dr.Read();
                        int fk_char_id = dr.GetInt32(0);
                        dr.Close();
                        cmd.CommandText = $"UPDATE characteristic SET fk_using_id = {ch_style}, fk_sex_id = {ch_sex}, fk_season_id = {ch_season}, fk_material_id = {ch_material}, fk_color_id = {ch_color} WHERE characteristic.id = {fk_char_id}";
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Рекомендации успешно обновлены!");
                        Program.Write($"\n[INFO] {DateTime.Now} Добавлены рекомендации");

                    } else
                    {
                        dr.Close();
                        cmd.CommandText = $"INSERT INTO characteristic(fk_using_id, fk_sex_id, fk_season_id, fk_material_id, fk_color_id) VALUES({ch_style}, {ch_sex}, {ch_season}, {ch_material}, {ch_color})";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = $"SELECT * FROM characteristic ORDER BY \"id\" DESC";
                        dr = cmd.ExecuteReader();
                        dr.Read();
                        int fk_char_id = dr.GetInt32(0);
                        dr.Close();
                        cmd.CommandText = $"UPDATE \"user\" SET fk_characteristic_id = {fk_char_id}";
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Рекомендации успешно обновлены!");
                        Program.Write($"\n[INFO] {DateTime.Now} Рекомендации обновлены");
                    }
                }
            } catch(NpgsqlException ex)
            {
                MessageBox.Show("Ошибка!\n" + ex);
            }
            finally { conn.Close(); }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Document.Body.Style = "zoom:20%";
        }

        private void button_add_save_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            if (radioButton_add1.Checked)
            {
                str = radioButton_add1.Text;
            }
            else if (radioButton_add2.Checked)
            {
                str = radioButton_add2.Text;
            }
            else if (radioButton_add3.Checked)
            {
                str = radioButton_add3.Text;
            }
            else if (radioButton_add4.Checked)
            {
                str = radioButton_add4.Text;
            }
            else if (radioButton_add5.Checked)
            {
                str = radioButton_add5.Text;
            }
            else if (radioButton_add6.Checked)
            {
                str = radioButton_add6.Text;
            }

            if (str != string.Empty)
            {
                panel_add.Visible = false;

                if (style.Contains(str))
                {
                    add_style = str;
                }
                else if (sex.Contains(str))
                {
                    add_sex = str;
                }
                else if (season.Contains(str))
                {
                    add_season = str;
                }
                else if (material.Contains(str))
                {
                    add_material = str;
                }
                else if (color.Contains(str))
                {
                    add_color = str;
                }
            }

            radioButton_add1.Visible = true;
            radioButton_add2.Visible = true;
            radioButton_add3.Visible = true;
            radioButton_add4.Visible = true;
            radioButton_add5.Visible = true;
            radioButton_add6.Visible = true;
        }
    }
   
}
