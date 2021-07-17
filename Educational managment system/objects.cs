using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational_managment_system
{
    /*
    class course
    {
        //____________main Attributes_________________
        private string Name;
        private string code;
        private string Password;
        private string pre;
        private string Doctor;//relation
        private int Limit;
        private int Occupied;
        //private string Uniqe = "code";
        datafile data = new datafile();

        //____________Relations_________________

        //____________Functions_________________
        public course(ref datafile data, string code, ref int back)
        {
            this.data = data;
            this.code = code;
            back = this.data.new_record(code);

            if (back == -1)
            {
                this.Name = this.data.read_by_column("name", this.code);
                this.Password = this.data.read_by_column("password", this.code);
                this.pre = this.data.read_by_column("pre", this.code);
                this.Doctor = this.data.read_by_column("doctor_id", this.code);
                // TODO: implement general method that try to convert to int
                this.Limit = int.Parse(this.data.read_by_column("limit", this.code));
                this.Occupied = int.Parse(this.data.read_by_column("occupied", this.code));

            }
            if (this.data.read_by_column("limit", this.code) == "")
            {
                this.data.update_by_column("limit", this.code, "0");
                this.Limit = 0;
                this.data.update_by_column("occupied", this.code, "0");
                this.Occupied = 0;
            }

        }
        public string name
        {
            get { return this.Name; }
            set
            {
                this.Name = value;
                this.data.update_by_column("name", this.code, this.Name);
            }
        }
        public string password
        {
            get { return this.Password; }
            set
            {
                this.Password = value;
                this.data.update_by_column("password", this.code, this.Password);
            }

        }
        public int set_pre(string value)
        {


            if (!general.is_availble(value, this.data.read_column("code")) & value != this.code)
            {
                this.pre = value;
                this.data.update_by_column("pre", code, pre);
                return 1;
            }

            else { }
            return -1;
        }
        public string get_pre() { return this.pre; }
        public int set_doctor(ref datafile doctors,string doctor_id)
        {
            if (!general.is_availble(doctor_id, doctors.read_column("id")))
            {
                this.Doctor = doctor_id;
                data.update_by_column("doctor_id", this.code, this.Doctor);
                return 1;
            }
            return -1;
        }
        public int limit
        {
            get { return this.Limit; }
            set
            {
                this.Limit = value;
                this.data.update_by_column("limit", this.code, this.Limit.ToString());
            }
        }
        public int occupied
        {
            get { return this.Occupied; }
            set
            {
                this.Occupied = value;
                this.data.update_by_column("occupied", this.code, this.Occupied.ToString());
            }
        }
    }
    class student
    {
        //____________main Attributes_________________
        private int id;
        private string User_name;
        private string Password;
        private string Full_name;
        private string Email;
        datafile data = new datafile();

        //____________Relations_________________

        //____________Functions_________________
        public student(ref datafile data, int id, ref int back)
        {
            this.data = data;
            this.id = id;
            back = this.data.new_record(id.ToString());
            if (back == -1)
            {
                this.User_name = this.data.read_by_column("username", this.id.ToString());
                this.Password = this.data.read_by_column("password", this.id.ToString());
                this.Full_name = this.data.read_by_column("fullname", this.id.ToString());
                this.Email = this.data.read_by_column("email", this.id.ToString());

            }

        }
        public string user_name
        {
            get { return this.User_name; }
            set
            {
                this.User_name = value;
                this.data.update_by_column("username", this.id.ToString(), this.User_name);
            }
        }
        public string password
        {
            get { return this.Password; }
            set
            {
                this.Password = value;
                this.data.update_by_column("password", this.id.ToString(), this.Password);
            }
        }
        public string full_name
        {
            get { return this.Full_name; }
            set
            {
                this.Full_name = value;
                this.data.update_by_column("fullname", this.id.ToString(), this.Full_name);
            }
        }
        public string email
        {
            get { return this.Email; }
            set
            {
                this.Email = value;
                this.data.update_by_column("email", this.id.ToString(), this.Email);
            }
        }
    }

    class general
    {
        public static bool is_availble(string keyword,string[] column)
        {
            for(int i =0;i< column.Length; i++)
            {
                if (column[i]==keyword)
                {
                    return false;
                }
            }
            return true;
        }
       
    }
    */
}
