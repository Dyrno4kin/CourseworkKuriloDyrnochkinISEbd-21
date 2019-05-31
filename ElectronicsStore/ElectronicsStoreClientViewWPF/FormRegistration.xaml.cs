﻿using ElectronicsStoreServiceDAL.BindingModel;
using ElectronicsStoreServiceDAL.Interfaces;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using Unity;

namespace ElectronicsStoreClientViewWPF
{
    /// <summary>
    /// Логика взаимодействия для FormRegistration.xaml
    /// </summary>
    public partial class FormRegistration : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly ICustomerService service;

        private int? id;

        public FormRegistration(ICustomerService service)
        {
            InitializeComponent();
            this.service = service;
        }



        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxLogin.Text))
            {
                MessageBox.Show("Заполните Логин", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxFIO.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPass.Text))
            {
                MessageBox.Show("Придумайте пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxMail.Text))
            {
                MessageBox.Show("Введите майл", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                string fio = textBoxFIO.Text;
                string login = textBoxLogin.Text;
                string mail = textBoxMail.Text;
                string pass = textBoxPass.Text;
                if (!string.IsNullOrEmpty(mail))
                {
                    if (!Regex.IsMatch(mail, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"))
                    {
                        MessageBox.Show("Неверный формат для электронной почты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (pass.Length > 8)
                    {
                        MessageBox.Show("Пароль должен содержать меньше 8 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                if (id.HasValue)
                {
                    service.UpdElement(new CustomerBindingModel
                    {
                        Id = id.Value,
                        CustomerFIO = textBoxFIO.Text,
                        Login = login,
                        Password = pass,
                        Email = mail
                    });
                }
                else
                {
                    service.AddElement(new CustomerBindingModel
                    {
                        CustomerFIO = textBoxFIO.Text,
                        Login = login,
                        Password = pass,
                        Email = mail
                    });
                }
                MessageBox.Show("Регистрация прошла успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
