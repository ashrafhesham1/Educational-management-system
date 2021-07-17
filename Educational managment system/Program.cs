using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
 

namespace Educational_managment_system
{
    class Program
    {
        static void Main(string[] args)
        {
            int main_win_choice;
            do
            {
                Console.WriteLine("Please make a choice:");
                Console.WriteLine("1-Login");
                Console.WriteLine("2-Sign up");
                Console.WriteLine("3-Shut down the system");
                Console.Write("Your choice is:");
                main_win_choice = int.Parse(Console.ReadLine());
                switch (main_win_choice)
                {
                    case 1:
                        string login_check;
                        do
                        {
                            Console.WriteLine("Please enter your user name and password:");
                            Console.Write("User name:");
                            string user_name = Console.ReadLine();
                            Console.Write("Password:");
                            string password = Console.ReadLine();
                            Console.WriteLine();
                            login_check = login(user_name, password);
                            switch (login_check)
                            {
                                case "wrong password":
                                    Console.WriteLine("Wrong password , Please try again"); Console.WriteLine();
                                    break;
                                case "user doesn't exist":
                                    Console.WriteLine("user doesn't exist , Please try again"); Console.WriteLine();
                                    break;
                            }
                        }
                        while (login_check == "wrong password" || login_check == "user doesn't exist");
                        main_win_choice = -1;
                        break;
                    case 2:
                        Sign_up();
                        main_win_choice = -1;
                        break;
                    case 3:
                        break;
                    default:
                        main_win_choice = -1;
                        Console.WriteLine("Wrong choice , Please try again");
                        Console.WriteLine("");
                        break;
                }
            }
            while (main_win_choice == -1);
        }
        static string login(string username, string password)
        {
            datafile login_op = new datafile("data/login_infs.txt", "username");
            string login_inf = login_op.read_record(username);
            if (login_inf != null)
            {
                string[] temp = login_inf.Split(',');
                if (temp[1] == password)
                {
                    switch (temp[2])
                    {
                        case "s":
                            std_interface(username);
                            break;
                        case "d":
                            dr_interface(username);
                            break;
                        case "t":
                            ta_interface(username);
                            break;
                    }
                    return temp[2];
                }
                return "wrong password";
            }
            return "user doesn't exist";
        }
        static void Sign_up()
        {
            datafile login_infs_source = new datafile("data/login_infs.txt", "username");
            string[] all_user_names = login_infs_source.read_column("username");
            string user_name="";
            bool valid_username = false;
            while (valid_username == false)
            {
                Console.Write("Enter user name: ");
                user_name = Console.ReadLine();
                bool is_username_availble = true;
                for (int i = 0; i < all_user_names.Length; i++)
                {
                    if (all_user_names[i] == user_name)
                    {
                        is_username_availble = false;
                        break;
                    }
                }
                if (is_username_availble == true)
                    valid_username = true;
                else
                    Console.WriteLine("User name is not availble , Please try diffrent one");
            }
            Console.Write("Please Enter Your Password: ");
            String password = Console.ReadLine();
            Console.Write("Please Enter Your full name: ");
            String fullname = Console.ReadLine();
            int id = 0;
            Console.WriteLine("Please Enter a choice: ");
            Console.WriteLine("1) Student");
            Console.WriteLine("2) Doctor");
            Console.WriteLine("3) Technical assistant");
            Console.Write("Enter a choice : ");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    datafile students_source = new datafile("data/students.txt", "id");
                    bool email_check = false;
                    string email = "";
                    while (email_check == false)
                    {
                        Console.Write("Please Enter Your Email: ");
                        email = Console.ReadLine();
                        email_check = check_email(email);
                        if (email_check == false)
                            Console.WriteLine("Please Enter a valid email");

                    }
                    id = students_source.read_column("id").Length - 1;
                    login_infs_source.new_record(user_name);
                    login_infs_source.update_by_column("password", user_name, password);
                    login_infs_source.update_by_column("type", user_name, "s");
                    students_source.new_record(id.ToString());
                    students_source.update_by_column("username",id.ToString() ,user_name);
                    students_source.update_by_column("fullname", id.ToString(), fullname);
                    students_source.update_by_column("email", id.ToString(), email);
                    Console.WriteLine("Your account had been created successfully");Console.WriteLine();
                    break;
                case 2:
                    datafile doctors_source = new datafile("data/doctors.txt", "id");
                    id = doctors_source.read_column("id").Length - 1;
                    login_infs_source.new_record(user_name);
                    login_infs_source.update_by_column("password", user_name, password);
                    login_infs_source.update_by_column("type", user_name, "d");
                    doctors_source.new_record(id.ToString());
                    doctors_source.update_by_column("username", id.ToString(), user_name);
                    doctors_source.update_by_column("fullname", id.ToString(), fullname);
                    Console.WriteLine("Your account had been created successfully"); Console.WriteLine();
                    break;
                case 3:
                    datafile ta_source = new datafile("data/doctors.txt", "id");
                    id = ta_source.read_column("id").Length - 1;
                    login_infs_source.new_record(user_name);
                    login_infs_source.update_by_column("password", user_name, password);
                    login_infs_source.update_by_column("type", user_name, "t");
                    ta_source.new_record(id.ToString());
                    ta_source.update_by_column("username", id.ToString(), user_name);
                    ta_source.update_by_column("fullname", id.ToString(), fullname);
                    Console.WriteLine("Your account had been created successfully"); Console.WriteLine();
                    break;
                default:
                    Console.WriteLine("Wrong choice , Please try again");
                    break;
            }
        }
        static void std_interface(string user_name)
        {
            datafile students = new datafile("data/students.txt", "id");
            student this_student = new student(int.Parse(students.read_by_column("id", user_name)));
            this_student.user_name = user_name;
            this_student.full_name = students.read_by_column("fullname", user_name);
            this_student.email = students.read_by_column("email", user_name);
            Console.WriteLine("Welcome {0} you are logged in", this_student.full_name);
            int choice;
            do
            {
                Console.WriteLine("Please make choice:");
                Console.WriteLine("1-Register in course");
                Console.WriteLine("2-View course");
                Console.WriteLine("3-Greads report");
                Console.WriteLine("4-Log out");
                Console.Write("Enter choice :");
                choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        student_operations.register_in_course(this_student);
                        choice = -1;
                        break;
                    case 2:
                        student_operations.view_course(this_student);
                        choice = -1;
                        break;
                    case 3:
                        student_operations.show_grades(this_student);
                        Console.WriteLine();
                        choice = -1;
                        break;
                    case 4:
                        break;
                    default:
                        choice = -1;
                        break;
                }
            }
            while (choice == -1);
        }
        static void dr_interface(string user_name)
        {
            datafile doctors_source = new datafile("data/doctors.txt", "id");
            doctor this_doctor = new doctor(int.Parse(doctors_source.read_by_column("id", user_name)));
            this_doctor.user_name = doctors_source.read_by_column("username", this_doctor.id.ToString());
            this_doctor.full_name = doctors_source.read_by_column("fullname", this_doctor.id.ToString());
            Console.WriteLine(); Console.WriteLine("Welcome Dr.{0} you are logged in ", this_doctor.full_name);
            int choice = -1;
            while (choice == -1)
            {
                Console.WriteLine("Please make a choice :");
                Console.WriteLine("1)Create a new course");
                Console.WriteLine("2)View my courses");
                Console.WriteLine("3)Log out");
                Console.Write("Enter a choice:");
                choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        doctor_operations.new_course(this_doctor);
                        choice = -1;
                        break;
                    case 2:
                        doctor_operations.view_course(this_doctor);
                        choice = -1;
                        break;
                    case 3:
                        break;
                    default:
                        Console.WriteLine("Wrong choice , Please try again");
                        choice = -1;
                        break;
                }
            }
        }
        static void ta_interface(string user_name)
        {
            datafile ta_source = new datafile("data/Tech_assistants.txt", "id");
            tech_assistant this_ta = new tech_assistant(int.Parse(ta_source.read_by_column("id", user_name)));
            this_ta.full_name = ta_source.read_by_column("fullname", this_ta.id.ToString());
            Console.WriteLine(); Console.WriteLine("Welcome {0} you are logged in ", this_ta.full_name);
            int choice = -1;
            while (choice == -1)
            {
                Console.WriteLine("Please make a choice :");
                Console.WriteLine("1) View my courses");
                Console.WriteLine("2) View invitations");
                Console.WriteLine("3)Log out");
                Console.Write("Enter a choice:");
                choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Technical_assistant_operations.view_course(this_ta);
                        choice = -1;
                        break;
                    case 2:
                        Technical_assistant_operations.view_invitations(this_ta);
                        choice = -1;
                        break;
                    case 3:
                        break;
                    default:
                        Console.WriteLine("Wrong choice , Please try again");
                        choice = -1;
                        break;
                }
            }

        }
        private static bool check_email(string email)
        {
            bool check = false;

            string[] email_form = email.Split('@');
            if (email_form.Length == 2)
            {
                string[] email_form2 = email_form[1].Split('.');
                if (email_form2.Length == 2)
                    check = true;
            }
            return check;
        }
    }
}
