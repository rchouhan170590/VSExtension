using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ServerAddWpf
{
    class DatabaseStackPanal
    {
     
        public static void initializeDatabaseString(int ServerName,ListBox listbox)
        {
            //listbox.Items.Clear();
            String databaseName = "configServer" + ServerName.ToString() + "DB";
            
            for(int i=1;i<=3;i++)
            {
                String propertyName = databaseName + i.ToString();
                String settingFileValue = Properties.Settings.Default[propertyName].ToString();
                if (!settingFileValue.Equals("-1"))
                {
                    string toBeSearched = "####";
                    string displayName = settingFileValue.Substring(settingFileValue.IndexOf(toBeSearched) + toBeSearched.Length);
                    String value = displayName;

                    listbox.Items.Add(value);
                }
            }
        }

        public static void Add_database_Name(ListBox datalistbox,ListBox serverListbox,String connectingString,String displayName)
        {
            if(serverListbox.SelectedItems.Count > 0)
            {
                displayName = displayName.Trim();
                connectingString = connectingString.Trim();
                if ((displayName == null) || (connectingString == null))
                {
                    System.Windows.Forms.MessageBox.Show("Please Specify The Display Name And Connecting String");
                    return;
                }

                String selectedServerdisplayName = serverListbox.SelectedItems[0].ToString();
                int ServerName = ServerHandle.find_dispaly_name_index(selectedServerdisplayName);

                String dataHalfProperty = "configServer"+ ServerName .ToString()+ "DB";

                int emptyIndex = first_empty_rowDataSetting(dataHalfProperty);
                if(emptyIndex == -1)
                {
                    System.Windows.Forms.MessageBox.Show("Sorry Max Limited cross");
                    return;
                }

                if (find_dispaly_name_indexData(displayName, dataHalfProperty) != -1)
                {
                    System.Windows.Forms.MessageBox.Show("Sorry, This Display Name already Used");
                    return;
                }

                String setValue = connectingString + "####" + displayName;
                datalistbox.Items.Add(displayName);

                Properties.Settings.Default[dataHalfProperty + emptyIndex.ToString()] = setValue;
                Properties.Settings.Default.Save();

                System.Windows.Forms.MessageBox.Show("New Data Entry Added to Server File Info Added");
                return;

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please First Select The Server");
            }
            
        }

        public static void delete_database_Name(ListBox datalistbox,ListBox serverListbox)
        {
            if (datalistbox.SelectedItems.Count > 0)
            {
                String selectedServerdisplayName = serverListbox.SelectedItems[0].ToString();
                int ServerName = ServerHandle.find_dispaly_name_index(selectedServerdisplayName);

                String dataHalfProperty = "configServer" + ServerName.ToString() + "DB";
                
                int indexSettingFile = find_dispaly_name_indexData(datalistbox.SelectedItems[0].ToString(), dataHalfProperty);
                Properties.Settings.Default[dataHalfProperty + indexSettingFile.ToString()] = "-1";
                Properties.Settings.Default.Save();

                datalistbox.Items.Remove(datalistbox.SelectedItems[0]);
                System.Windows.Forms.MessageBox.Show("DataBase Deleted");

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Nothing is Selected");
            }

            return;
        }


        public static int first_empty_rowDataSetting(String property)
        {
            for (int i = 1; i <= 3; i++)
            {
                String propertyName = property + i.ToString();
                String settingFileValue = Properties.Settings.Default[propertyName].ToString();
                if (settingFileValue.Equals("-1"))
                    return i;
            }
            return -1;
        }

        public static int find_dispaly_name_indexData(String name,String property)
        {
            for (int i = 1; i <= 3; i++)
            {
                String propertyName = property + i.ToString();
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

        public static void reset_all_database_ofServer(String property)
        {
            // property = "config"+ServerName + "DB";
            for(int i=1;i<=3;i++)
            {
                Properties.Settings.Default[property + i.ToString()] = "-1";
                Properties.Settings.Default.Save();
            }
        }

        public static String selected_database_connecting_string(ListBox ServerListbox, ListBox dbListbox)
        {
            String connectingString = "No any Connecting String";
            if(dbListbox.SelectedItems.Count > 0)
            {
                int serverSelectedIndex = ServerHandle.find_dispaly_name_index(ServerListbox.SelectedItems[0].ToString());
                String halfProperty = "configServer" + serverSelectedIndex.ToString() + "DB";
                String dbDisplayName = dbListbox.SelectedItems[0].ToString();
                int dbDispalyIndex = find_dispaly_name_indexData(dbDisplayName, halfProperty);

                String fullProperty = halfProperty + dbDispalyIndex.ToString();

                String valueFromSetting = Properties.Settings.Default[fullProperty].ToString();
                int charLocation = valueFromSetting.IndexOf("####", StringComparison.Ordinal);
                connectingString = valueFromSetting.Substring(0, charLocation);
            }

            return connectingString;
        }

    }
}
