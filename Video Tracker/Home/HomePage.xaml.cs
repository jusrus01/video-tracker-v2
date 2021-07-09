﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace video_tracker_v2
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();

            // add false flag to remove button
            btnRemove.DataContext = false;

            DataManager.MainPath = "categories.data";
            DataManager.CreateDataFolder();

            CreateCategories();
        }

        /// <summary>
        /// Create buttons for each path saved in
        /// data file
        /// </summary>
        private void CreateCategories()
        {
            string[] categories = DataManager.LoadCategories();

            if (categories == null)
                return;

            foreach (string path in categories)
            {
                AddButton(path);
            }
        }

        private void AddButton(string directoryPath)
        {
            string buttonText = System.IO.Path.GetFileName(directoryPath);
            Button button = UI.CreateButton(buttonText);

            // bind button to event
            button.Click += Button_Click;

            // add directory path to button
            button.DataContext = directoryPath;

            // add button to panel
            panelCategories.Children.Add(button);
        }

        /// <summary>
        /// Handles page transition with button click
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event data</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool isDeleting = (bool)btnRemove.DataContext;
            if (isDeleting)
            {
                // delete button and remove from file
                Button btn = (sender as Button);
                DataManager.RemoveCategoryFromFile(btn.DataContext.ToString());
                panelCategories.Children.Remove(btn);
                ToogleRemove(null, null);
            }
            else
            {
                VideosPage videosPage = new VideosPage((sender as Button).DataContext as string);
                this.NavigationService.Navigate(videosPage);
            }
        }

        /// <summary>
        /// Sets state to deletion
        /// and changes button color
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event data</param>
        private void ToogleRemove(object sender, RoutedEventArgs e)
        {
            //if (deleting)
            //{
            //    btnRemove.Background = UI.ButtonNormalBrush;
            //    deleting = false;
            //}
            //else
            //{
            //    deleting = true;
            //    btnRemove.Background = UI.ButtonActiveBrush;
            //}
            if((bool)btnRemove.DataContext)
            {
                btnRemove.Background = UI.ButtonNormalBrush;
                btnRemove.DataContext = false;
            }
            else
            {
                btnRemove.Background = UI.ButtonActiveBrush;
                btnRemove.DataContext = true;
            }
        }

        /// <summary>
        /// Opens file dialog and allows user to
        /// select path to videos directory
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event data</param>
        private void AddCategory(object sender, RoutedEventArgs e)
        {
            if((bool)btnRemove.DataContext)
                ToogleRemove(null, null);

            CommonOpenFileDialog openDialog = new CommonOpenFileDialog();
            openDialog.IsFolderPicker = true;

            if (openDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                // check to see if it already exists
                if (!DataManager.EntryExists(openDialog.FileName))
                {
                    AddButton(openDialog.FileName);
                    DataManager.SaveCategory(openDialog.FileName);
                }
            }
        }
    }
}
