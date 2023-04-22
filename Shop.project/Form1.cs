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
namespace Shop.project
{

    public partial class Form1 : Form
    {
        private User user;
        public Random reserve_code;

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

                    cmd.CommandText = $"SELECT EXISTS (SELECT * from registration WHERE user_signin_email = '{textBox_email.Text}')";
                    NpgsqlDataReader dr = cmd.ExecuteReader();

                    if (!dr.Read())
                    {
                        MessageBox.Show("Ошибка подключения с базой данных!\nПопробуйте ещё раз!");
                        return;
                    }
                    if (dr.GetBoolean(0) == true)
                    {

                        dr.Close();

                        cmd.CommandText = $"SELECT user_signin_email FROM registration WHERE user_signin_login = '{textBox_reserve_login.Text}'";
                        dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {

                            string email = dr.GetString(0);

                            user.user_signin_code = reserve_code.Next(1000, 10000);

                            string text = $"<b>Ваш резервный код:<br>{user.user_signin_code}</br>.<br>Если код пришел вам случайно, никому не отправляйте этот код!</br></b>";
                            SendMessage(email, "Восстановление пароля", text);

                            MessageBox.Show("Код успешно отправлен на почту, указанную Вами в личном кабинете!\n(Возможно код лежит в папке \"СПАМ\")\nВАЖНО: после того, как вы ввели код, если он правильный, то вы автоматиечски попадёте во вкладку восстановления пароля, никакие другие кнопки нажимать не нужно!");
                            tabControl1.SelectedIndex = 3;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ошибка! Данный пользователь не существует!");
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
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            string code = user.user_signin_code.ToString();

            if (code.Equals(textBox_enter_reserve_code.Text))
            {
                tabControl1.SelectedIndex = 4;
            }
            else
            {
                MessageBox.Show("Введеный код не совпадает с отправленным!");
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

            NpgsqlConnection conn = GetConnection();
            NpgsqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT user_signin_id FROM registration WHERE user_signin_login = '{textBox_login.Text}' AND user_signin_password = '{textBox_password.Text}'";
            try
            {
                conn.Open();

                if (conn.State == ConnectionState.Open)
                {
                    NpgsqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        textBox_login.Text = string.Empty;
                        textBox_password.Text = string.Empty;

                        user.user_signin_id = dr.GetInt32(0);
                        PersonalSpace();

                        tabControl1.SelectedIndex = 5;
                    }
                    else
                    {
                        textBox_login.BackColor = Color.Red;
                        textBox_password.BackColor = Color.Red;
                        MessageBox.Show("Неправильный логин или пароль!");
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

                    cmd.CommandText = $"SELECT EXISTS (SELECT * from registration WHERE user_signin_email = '{textBox_email_in.Text}')";
                    reader = cmd.ExecuteReader();

                    if (reader.Read() && reader.GetBoolean(0) == true)
                    {
                        MessageBox.Show("Данная почта уже занята!");
                        return;
                    }
                    reader.Close();




                    cmd.CommandText = $"SELECT EXISTS(SELECT * from registration WHERE user_signin_login = '{textBox_ligin_in.Text}');";
                    reader = cmd.ExecuteReader();

                    if (reader.Read() && reader.GetBoolean(0) == true)
                    {
                        MessageBox.Show("Логин уже занят!");
                        return;
                    }
                    reader.Close();



                    cmd.CommandText = $"INSERT INTO registration(user_signin_login, user_signin_password, user_signin_email) values ('{textBox_ligin_in.Text}', '{textBox_password_in.Text}', '{textBox_email_in.Text}')";
                    reader = cmd.ExecuteReader();


                    MessageBox.Show($"Форма заполнена верно");

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
            tabControl1.SelectedIndex = 6;
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
            cmd.CommandText = $"UPDATE registration SET user_signin_password = '{new_password.Text}' WHERE user_signin_login = '{textBox_reserve_login.Text}'";


            try
            {
                conn.Open();

                if (cmd.ExecuteNonQuery() != -1)
                {
                    PersonalSpace();

                    MessageBox.Show("Вы успешно обновили пароль!");
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

                cmd.CommandText = $"UPDATE registration SET user_lc_name = '{textBox_lc_name.Text}',user_signin_email = '{textBox_lc_email.Text}', user_signin_login = '{textBox_lc_login.Text}', user_lc_birthday = '{textBox_lc_birthday.Text}' WHERE user_signin_id = {user.user_signin_id}";

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
            cmd.CommandText = $"SELECT * FROM registration WHERE user_signin_id = {user.user_signin_id}";

            try
            {
                conn.Open();

                if (conn.State == ConnectionState.Open)
                {
                    NpgsqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        user.user_signin_id = dr.GetInt32(0);
                        user.user_signin_login = dr.GetString(1);
                        user.user_signin_password = dr.GetString(2);
                        user.user_signin_email = dr.GetString(3);

                        textBox_lc_name .Text = user.user_lc_name;
                        textBox_lc_email.Text = user.user_signin_email;
                        textBox_lc_login.Text = user.user_signin_login;

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
            tabControl1.SelectedIndex = 11;
        }

        private void button_back_to_offer_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 8;
        }
    }
   
}
