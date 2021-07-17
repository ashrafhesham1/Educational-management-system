using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational_managment_system
{
    public class student
    {
        public int id { get; }
        public string user_name;
        public string password;
        public string full_name;
        public string email;
        
        public student(int id)
        {
            this.id = id;
        }
    }
    public class course
    {
        public string code { get; }
        public string name;
        public string password;
        public string pre;
        public int doctor;
        public int limit;
        public int occupied;

        public course(string code)
        {
            this.code = code;
        }
    }
    public class doctor
    {
        public int id { get; }
        public string full_name;
        public string user_name;
        public string password;

        public doctor(int id)
        {
            this.id = id;
        }
    }
    public class tech_assistant
    {
        public int id { get; }
        public string full_name;
        public string username;
        public string password;

        public tech_assistant(int id)
        {
            this.id = id;
        }
    }
    public class assignment
    {
        public string code { get; }
        public string name;
        public string course;
        public DateTime deadline;
        public int full_mark;
        public int num_of_questions { get; }
        public string[] content;

        public assignment(string code , int num_of_questions)
        {
            this.code = code;
            this.num_of_questions = num_of_questions;
            this.content = new string[num_of_questions];
        }

    }
    public class std_assign
    {
        public int student { get; }
        public string assignment { get; }
        public string answer;
        public string grade;
        public std_assign(int student,string assignment)
        {
            this.student = student;
            this.assignment = assignment;
        }
    }
    public class crs_ta
    {
        public string course { get; }
        public int ta { get; }
        public int approve;
        public int assignment;
        public crs_ta(string course , int ta)
        {
            this.course = course;
            this.ta = ta;
        }
    }

}
