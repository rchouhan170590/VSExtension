using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;

namespace ServerAddWpf
{
    class ServerHandle
    {
        public static void initializeServerInWindow(ListBox ServreListbox)
        {
            ServreListbox.Items.Clear();

            for (int i = 1; i <= 3; i++)
            {
                String propertyName = "configServer" + i.ToString();
                String settingFileValue = Properties.Settings.Default[propertyName].ToString();
                if (!settingFileValue.Equals("-1"))
                {
                    string toBeSearched = "####";
                    string displayName = settingFileValue.Substring(settingFileValue.IndexOf(toBeSearched) + toBeSearched.Length);
                    String value = displayName;

                    ServreListbox.Items.Add(value);
                }
            }

        }

        public static int Add_New_Server(ListBox listbox, String displayName, String file_path)
        {
            int serverCountvalue = Properties.Settings.Default.ServerCount;
            if (serverCountvalue == 3)
            {
                System.Windows.Forms.MessageBox.Show("Sorry, Max Limit Cross Cant Add More");
                return 0;
            }
            displayName = displayName.Trim();
            if(displayName == null)
            {
                System.Windows.Forms.MessageBox.Show("Please Specify The Display Name");
                return 0;
            }

            if (find_dispaly_name_index(displayName) != -1)
            {
                System.Windows.Forms.MessageBox.Show("Sorry, This Display Name already Used");
                return 0;
            }

            if (File.Exists(file_path))
            {
                String setValue = file_path + "####" + displayName;
                listbox.Items.Add(displayName);

                int emptyIndex = first_empty_rowSetting();
                Properties.Settings.Default["configServer" + emptyIndex.ToString()] = setValue;
                Properties.Settings.Default.Save();
                System.Windows.Forms.MessageBox.Show("New Server File Info Added");
                return 1;
                
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please Select The File Path");
                return 0;
            }

        }

        public static int first_empty_rowSetting()
        {
            for (int i = 1; i <= 3; i++)
            {
                String propertyName = "configServer" + i.ToString();
                String settingFileValue = Properties.Settings.Default[propertyName].ToString();
                if (settingFileValue.Equals("-1"))
                    return i;
            }
            return -1;
        }

        public static int find_dispaly_name_index(String name)
        {

            for (int i = 1; i <= 3; i++)
            {
                String propertyName = "configServer" + i.ToString();
                String settingFileValue = Properties.Settings.Default[propertyName].ToString();
                if (!settingFileValue.Equals("-1"))
                {
                    string toBeSearched = "####";
                    string code = settingFileValue.Substring(settingFileValue.IndexOf(toBeSearched) + toBeSearched.Length);
                    if (code.Equals(name))
                        return i;
                }
            }
            return -1;
        }

        
        public static void deleteServer(ListBox listbox)
        {
            if(listbox.SelectedItems.Count > 0)
            {   
                int indexSettingFile = find_dispaly_name_index(listbox.SelectedItems[0].ToString());
                Properties.Settings.Default["configServer" + indexSettingFile.ToString()] = "-1";
                Properties.Settings.Default.Save();

                listbox.Items.Remove(listbox.SelectedItems[0]);

                String dbHalf_Property = "configServer" + indexSettingFile.ToString() + "DB";
                DatabaseStackPanal.reset_all_database_ofServer(dbHalf_Property);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Nothing is Selected");
            }
        }

        public static String selected_server_file_path(ListBox listbox)
        {
            String ServerInfoFile = "-1";
            if (listbox.SelectedItems.Count > 0)
            {
                int indexSettingFile = find_dispaly_name_index(listbox.SelectedItems[0].ToString());
                String valueFromSetting = Properties.Settings.Default["configServer" + indexSettingFile.ToString()].ToString();
                int charLocation = valueFromSetting.IndexOf("####", StringComparison.Ordinal);
                ServerInfoFile = valueFromSetting.Substring(0, charLocation);
            }
            
            return ServerInfoFile;
        }

        
    }
}
