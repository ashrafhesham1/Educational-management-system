using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Educational_managment_system
{
    class datafile
    {
        private string path;
        private Dictionary<string, int> columns = new Dictionary<string, int>();
        private string key;
        private string second_part_key;
        public datafile(string path,string key)
        {
            if (!File.Exists(path))
            {
                throw new Exception("File doesn't exist,please enter columns on declaration");
            }
            this.path = path;
            this.columns = get_columns(this.path);
            this.key = key;
            this.second_part_key = null;
        }
        public datafile(string path, string key , string second_part_key)
        {
            if (!File.Exists(path))
            {
                throw new Exception("File doesn't exist,please enter columns on declaration");
            }
            this.path = path;
            this.columns = get_columns(this.path);
            this.key = key;
            this.second_part_key = second_part_key;
        }
        public datafile() { }

        public int update_by_column(string column, string key,string newdata)
        {
            bool check_key=false;
            if (column == this.key)
            {
                check_key = is_key_availble(key);
            }
            if (column!=this.key||check_key==true)
            {
                    string oldline = this.read_record(key);
                    string[] Sline = oldline.Split(',');
                    Sline[this.columns[column]] = newdata;
                  //  string newline = stostring(Sline);
                    string newline = string.Join(",", Sline);

                update_line(this.path, oldline, newline);
                return 1;
            }
            else if(check_key==false) { }
            return -1;
        }
        public int update_by_column(string column, string f_key, string s_key, string newdata)
        {
            bool check_key = false;
            if (column == this.key||column==this.second_part_key)
            {
                check_key = is_key_availble(f_key,s_key);
            }
            if ((column != this.key && column != this.second_part_key) || check_key == true)
            {
                string oldline = this.read_record(f_key,s_key);
                string[] Sline = oldline.Split(',');
                Sline[this.columns[column]] = newdata;

                string newline = string.Join(",", Sline);
                  //  stostring(Sline);
                update_line(this.path, oldline, newline);
                return 1;
            }
            else if (check_key == false) { }
            return -1;
        }
        public string read_by_column(string column, string key)
        {
            string[] document = read(this.path);
            string[] line = searchFile(this.path, key).Split(',');
            string data = line[this.columns[column]];
            return data;
        }
        public string[] read_column(string column)
        {
            string[] document = read(this.path);
            string[] full_column = new string[document.Length];
            for(int i = 0; i < document.Length; i++)
            {
                string[] line = document[i].Split(',');
                full_column[i] = line[this.columns[column]];
            }
            return full_column;
        }
        public int new_record(string key)
        {
            if (is_key_availble(key) == true)
            {
                string spilit = "";
                int columns = this.columns.Count() - 1;
                int order = this.columns[this.key];
                for (int i = 0; i < order; i++)
                    spilit += ',';
                spilit += key;
                for (int i = 0; i < columns - order; i++)
                    spilit += ',';
                append(this.path, spilit);
                return 1;
            }
            return -1;
        }
        public int new_record(string f_key,string s_key)
        {
            if (is_key_availble(f_key,s_key) == true)
            {
                string spilit = "";
                int columns = this.columns.Count() - 1;
                int order = this.columns[this.key];
                for (int i = 0; i < order; i++)
                    spilit += ',';
                spilit = spilit + f_key +','+s_key;
                for (int i = 0; i < columns - order-1; i++)
                    spilit += ',';
                append(this.path, spilit);
                return 1;
            }
            return -1;
        }
        public void delete_record(string key)
        {
            string[] keys = this.read_column(this.key);
            string[] document = read(this.path);
            string[] newdocument = new string[document.Length - 1];
            document[Array.IndexOf(keys,key)]= "";
            newdocument = arr_shift(document);
            writeall(this.path, newdocument);
        }
        public void delete_record(string f_key,string s_key)
        {
            int record_index = -1;
            string[] f_keys = this.read_column(this.key);
            string[] s_keys = this.read_column(this.second_part_key);
            for(int i = 0; i < f_keys.Length; i++)
            {
                if (f_keys[i] == f_key && s_keys[i] == s_key)
                    record_index = i;
            }
            if (record_index != -1)
            {
                string[] document = read(this.path);
                string[] newdocument = new string[document.Length - 1];
                document[record_index] = "";
                newdocument = arr_shift(document);
                writeall(this.path, newdocument);
            }

        }
        public string read_record(string key)
        {
            string[] keys = read_column(this.key);
            for (int i = 0; i<keys.Length;i++)
            {
                if (keys[i] == key)
                    return read(this.path)[i];
            }
            return null;
        }
        public string read_record(string first_key,string second_part_key)
        {
            string[] f_keys = read_column(this.key);
            string[] s_keys = read_column(this.second_part_key);
            for (int i = 0; i < f_keys.Length; i++)
            {
                if (f_keys[i] == first_key && s_keys[i] == second_part_key)
                    return read(this.path)[i];
            }
            return "";
        }

        public string[] read_all_maches(string key,string key_column_name, string return_column_name) //for relations
        {
            string[] key_column = this.read_column(key_column_name);
            string[] return_column = this.read_column(return_column_name);
            List<string> result_column = new List<string>();
            for(int i = 0; i < key_column.Length; i++)
            {
                if (key_column[i] == key)
                {
                    result_column.Add(return_column[i]);
                }
            }
            return result_column.ToArray();
        }
        static void append(string path,string data)
        {
            File.AppendAllText(path, data + Environment.NewLine);
        }
        static void writeall(string path, string[] data)
        {
            File.WriteAllLines(path, data);
        }
        static string[] read(string path)
        {
            string[] data = File.ReadAllLines(path);
            return data;
        }
        static string searchFile(string path, string searchKey)
        {
            StreamReader readdata = new StreamReader(path);
            string line = "";
            do
            {
                line = readdata.ReadLine();
                if (line != null)
                {
                    if (line.Contains(searchKey))
                    {
                        readdata.Close();
                        return line;
                    }
                }
            }
            while (line != null);
            readdata.Close();
            return null;
        }
        static int update_line(string path, string oldline, string newline)
        {
            string[] arr_data = File.ReadAllLines(path);
            for (int i = 0; i < arr_data.Length; i++)
            {
                if (arr_data[i] == oldline)
                {
                    arr_data[i] = newline;
                    File.WriteAllLines(path, arr_data);
                    return 1;
                }
            }
            return -1;
        }
        private Dictionary<string,int> get_columns(string path)
        {
            StreamReader readdata = new StreamReader(path);
            string[] columns_names = readdata.ReadLine().Split(',');
            readdata.Close();
            Dictionary<string, int> columns = new Dictionary<string, int>();
            for (int i =0; i < columns_names.Length; i++)
            {
                columns[columns_names[i]] = i;
            }
            return columns;
        }
        private string[] arr_shift(string[] arr)
        {
            String[] newarr = new string[arr.Length - 1];
            int count = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != "")
                {
                    newarr[count] = arr[i];
                    count++;
                }
            }
            return newarr;
        }
        private bool is_key_availble(string newkey)
        {
            string[] keys = this.read_column(this.key);
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] == newkey)
                    return false;
            }
            return true;
        }
        private bool is_key_availble(string f_newkey,string s_newkey)
        {
            string[] f_keys = this.read_column(this.key);
            string[] s_keys = this.read_column(this.second_part_key);

            for (int i = 0; i < f_keys.Length; i++)
            {
                if (f_keys[i] == f_newkey && s_keys[i]==s_newkey)
                    return false;
            }
            return true;
        } //composite key
    }
}
