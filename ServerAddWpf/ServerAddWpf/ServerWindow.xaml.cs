using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.VisualStudio.Utilities;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;

namespace ServerAddWpf
{ 
    public partial class ServerWindow : Window
    {
        public ServerWindow()
        {
            InitializeComponent();

            const int snugContentWidth = 380;
            const int snugContentHeight = 500;

            var horizontalBorderHeight = SystemParameters.ResizeFrameHorizontalBorderHeight;
            var verticalBorderWidth = SystemParameters.ResizeFrameVerticalBorderWidth;
            var captionHeight = SystemParameters.CaptionHeight;

            Width = snugContentWidth + 2 * verticalBorderWidth;
            Height = snugContentHeight + captionHeight + 2 * horizontalBorderHeight;

            ServerHandle.initializeServerInWindow(ServerListbox);

        }

        public String ServerSelectedDisplayName = "";
        private void select_server_file_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            Nullable<bool> dialogOk = fileDialog.ShowDialog();
            if(dialogOk == true)
            {
                server_file_textbox.Text = fileDialog.FileName;
            }

        }

        private void add_server_toSettingsbtn_Click(object sender, RoutedEventArgs e)
        {
            int done =  ServerHandle.Add_New_Server(ServerListbox, serverDispalyNameText.Text, server_file_textbox.Text);
            if(done == 1)
            {
                server_file_textbox.Text = "";
                serverDispalyNameText.Text = "";
                return;
            }
            return;
        }

        public int find_dispaly_name_index(String name)
        {
            
            for(int i=1;i<=5;i++)
            {
                String propertyName = "configServer" + i.ToString();
                String settingFileValue = Properties.Settings.Default[propertyName].ToString();
                if(!settingFileValue.Equals("-1"))
                {
                    string toBeSearched = "####";
                    string code = settingFileValue.Substring(settingFileValue.IndexOf(toBeSearched) + toBeSearched.Length);
                    if(code.Equals(name))
                        return i;
                }
            }
            return -1;
        }

        public int first_empty_rowSetting()
        {
            for(int i=1;i<=5;i++)
            {
                String propertyName = "configServer" + i.ToString();
                String settingFileValue = Properties.Settings.Default[propertyName].ToString();
                if (settingFileValue.Equals("-1"))
                    return i;
            }
            return -1;
        }

        private void okaybtn_Click(object sender, RoutedEventArgs e)
        {
            String CurrentWorkingFile = Properties.Settings.Default.activeFileName;
            if (!CurrentWorkingFile.Equals("-1"))
            {
                String ReplaceIt = replaceTextbox.Text;
                String ReplaceBy = DatabaseStackPanal.selected_database_connecting_string(ServerListbox, databaseListbox);
                replace_word_in_file(CurrentWorkingFile, ReplaceIt, ReplaceBy);

                MessageBox.Show("successfully Replace");
            }
            else
            {
                MessageBox.Show("Sorry, No Active File Found. Please Open File ");
            }
            return;
            
        }

        
        private void deletebtn_Click(object sender, RoutedEventArgs e)
        {
            ServerHandle.deleteServer(ServerListbox);
            databaseListbox.Items.Clear();
            return;
        }

        private void add_database_toSettingsbtn_Click(object sender, RoutedEventArgs e)
        {
            DatabaseStackPanal.Add_database_Name(databaseListbox, ServerListbox, server_file_textbox.Text, serverDispalyNameText.Text);
            return;    //serverDispalyNameText.Text, server_file_textbox.Text
        }

        private void database_delete_btn_Click(object sender, RoutedEventArgs e)
        {
            DatabaseStackPanal.delete_database_Name(databaseListbox,ServerListbox);
            return ;
        }


        
        private void serverListBox_Item_Click(object sender, RoutedEventArgs e)
        {
            databaseListbox.Items.Clear();
            String selectedServerdisplayName = ServerListbox.SelectedItems[0].ToString();
            int ServerId = ServerHandle.find_dispaly_name_index(selectedServerdisplayName);
            DatabaseStackPanal.initializeDatabaseString(ServerId, databaseListbox);

            String file_path = ServerHandle.selected_server_file_path(ServerListbox);
            severfilePathLabel.Content = file_path;
            connectingsStringLabel.Content = "";
            return;
        }

        public void replace_word_in_file(String file_path,String replaceIt,String replaceBy)
        {
            string text = File.ReadAllText(file_path);
            text = text.Replace(replaceIt, replaceBy);
            File.WriteAllText(file_path, text);

            return;
        }

        private void databaseListBox_Item_Click(object sender, RoutedEventArgs e)
        {
            String connecting_string = DatabaseStackPanal.selected_database_connecting_string(ServerListbox, databaseListbox);
            connectingsStringLabel.Content = connecting_string;

            return;
        }

        //ServerListBoxItemSelected
        private void ServerListBoxItemSelected(object sender, RoutedEventArgs e)
        {
            //ServerSelectedDisplayName
            if(ServerListbox.SelectedItems.Count > 0)
            {
                String newServerSelectedDisplayName = ServerListbox.SelectedItems[0].ToString();
                if (!ServerSelectedDisplayName.Equals(newServerSelectedDisplayName))
                {
                    databaseListbox.Items.Clear();
                    ServerSelectedDisplayName = newServerSelectedDisplayName;
                }

            }

        }

        private void dbListBoxItemSelected(object sender, RoutedEventArgs e)
        {
           // String connecting_string = DatabaseStackPanal.selected_database_connecting_string(ServerListbox, databaseListbox);
           // connectingsStringLabel.Content = connecting_string;
            return;
        }


    }
}
