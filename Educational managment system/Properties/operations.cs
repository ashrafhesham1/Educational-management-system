using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational_managment_system
{
    public  class student_operations
    {
        public student_operations() { }
        public static void register_in_course(student this_student)
        {
            datafile courses_resource = new datafile("data/courses.txt", "code");
            datafile std_crs_resource = new datafile("data/std_crs.txt", "student", "course");
            string[] all_courses = courses_resource.read_column("code");
            string[] student_courses = std_crs_resource.read_all_maches(this_student.id.ToString(), "student", "course");
            string[] not_student_courses = new string[all_courses.Length - student_courses.Length-1];
            int counter=0;
            for (int i = 1; i < all_courses.Length; i++)
            {
                bool temp = true;
                for(int l = 0; l < student_courses.Length; l++)
                {
                    if (student_courses[l] == all_courses[i])
                    {
                        temp = false;
                        break;
                    }
                }
                if (temp == true)
                {
                    not_student_courses[counter] = all_courses[i];
                    counter++;
                }
            }
            List<course> availble_courses = new List<course>();
            for(int i = 0; i < not_student_courses.Length; i++)
            {
                string[] temp = courses_resource.read_record(not_student_courses[i]).Split(',');
                if (int.Parse(temp[5]) != 0)
                {
                    if (int.Parse(temp[6]) >= int.Parse(temp[5]))
                        break;
                }
                availble_courses.Add(new course(temp[0]));
                availble_courses[availble_courses.Count - 1].name = temp[1];
                availble_courses[availble_courses.Count - 1].password = temp[2];
                availble_courses[availble_courses.Count - 1].pre = temp[3];
                availble_courses[availble_courses.Count - 1].doctor = int.Parse(temp[4]);
                availble_courses[availble_courses.Count - 1].limit= int.Parse(temp[5]);
                availble_courses[availble_courses.Count - 1].occupied = int.Parse(temp[6]);
            }
            if (availble_courses.Count == 0)
            {
                Console.WriteLine("No courses availble");
            }
            else
            {
                bool back = false;
                do
                {
                    Console.WriteLine(); Console.WriteLine("Please  make a choice:");
                    Console.WriteLine("1)Enter the course's code");
                    Console.WriteLine("2)List all availble courses");
                    Console.WriteLine("3)Back");
                    Console.Write("Enter choice:");
                    int choice = int.Parse(Console.ReadLine());
                    string course_code = "";
                    int course_index = 0;
                    switch (choice)
                    {
                        case 1:
                            bool code_check = false;
                            while (code_check == false)
                            {
                                Console.Write("Register in course:");
                                course_code = Console.ReadLine();
                                for (int i = 0; i < availble_courses.Count; i++)
                                {
                                    if (course_code == availble_courses[i].code)
                                        code_check = true;
                                }
                                if (code_check == false)
                                    Console.WriteLine("Course is not availble or not exist ,Please try again");
                            }
                            break;
                        case 2:
                            for (int i = 0; i < availble_courses.Count; i++)
                            {
                                Console.WriteLine("{0}){1}-{2}", i + 1, availble_courses[i].name, availble_courses[i].code);
                            }
                            Console.Write("Enter choice:");
                            course_code = availble_courses[int.Parse(Console.ReadLine()) - 1].code;
                            break;
                        case 3:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Wrong choice , Please try again");
                            break;
                    }
                    if (choice == 1 || choice == 2)
                    {
                        for (int i = 0; i < availble_courses.Count; i++)
                        {
                            if (availble_courses[i].code == course_code)
                            {
                                course_index = i;
                                break;
                            }
                        }
                        bool password_check = true;
                        bool pre_course_check = false;
                        if (availble_courses[course_index].password != "")
                        {
                            Console.Write("Enter the course password:");
                            string password = Console.ReadLine();
                            if (password != availble_courses[course_index].password)
                                password_check = false;
                        }
                        if (password_check)
                        {
                            if (availble_courses[course_index].pre != "null")
                            {
                                for (int i = 0; i < student_courses.Length; i++)
                                {
                                    if (student_courses[i] == availble_courses[course_index].pre)
                                        pre_course_check = true;
                                }
                            }
                            else
                                pre_course_check = true;

                        }
                        else
                            Console.WriteLine("Wrong password , Please try again");
                        if (pre_course_check == false)
                            Console.WriteLine("You need to start the pre-required courese:{0} first ", availble_courses[course_index].pre);

                        else if (password_check && pre_course_check)
                        {
                            std_crs_resource.new_record(this_student.id.ToString(), availble_courses[course_index].code);
                            std_crs_resource.update_by_column("graduate", this_student.id.ToString(), availble_courses[course_index].code, "false");
                            std_crs_resource.update_by_column("grade", this_student.id.ToString(), availble_courses[course_index].code, "-1");
                            availble_courses[course_index].occupied++;
                            courses_resource.update_by_column("occupied", availble_courses[course_index].code, availble_courses[course_index].occupied.ToString());
                            Console.WriteLine("Registeration completed successfully");

                        }
                    }
                }
                while (back == false);
            }
   

        }
        public static void view_course(student this_student)
        {
            datafile std_crs_source = new datafile("data/std_crs.txt", "student");
            datafile courses_source = new datafile("data/courses.txt", "code");
            datafile doctors_source = new datafile("data/doctors.txt", "id");
            datafile announcements_source = new datafile("data/announcments.txt", "announcment");
            String[] courses_codes = std_crs_source.read_all_maches(this_student.id.ToString(), "student", "course");
            if (courses_codes.Length == 0)
            {
                Console.WriteLine("No courses availble");
            }
            else
            {
                string[] courses_data = new string[courses_codes.Length];
                for (int i = 0; i < courses_codes.Length; i++)
                {
                    courses_data[i] = courses_source.read_record(courses_codes[i]);
                }
                course[] courses = new course[courses_data.Length];
                for (int i = 0; i < courses_data.Length; i++)
                {
                    string[] temp = courses_data[i].Split(',');
                    courses[i] = new course(temp[0]);
                    courses[i].name = temp[1];
                    courses[i].password = temp[2];
                    courses[i].pre = temp[3];
                    courses[i].doctor = int.Parse(temp[4]);
                    courses[i].limit = int.Parse(temp[5]);
                    courses[i].occupied = int.Parse(temp[6]);
                }
                int choice = 1;
                do
                {
                    Console.WriteLine("My courses list :");
                    for (int i = 0; i < courses.Length; i++)
                    {
                        Console.WriteLine("{0})Course: {1} - code: {2}", i + 1, courses[i].name, courses[i].code);
                    }
                    Console.WriteLine("Which ith [1:{0}] course to view ?", courses.Length);
                    int view_course = int.Parse(Console.ReadLine());
                    string[] course_announcments = announcements_source.read_all_maches(courses[view_course - 1].code, "course", "announcment");
                    Console.WriteLine("Course: {0} - Code: {1} ", courses[view_course - 1].name, courses[view_course - 1].code);
                    Console.WriteLine("taught by Dr. {0} ", doctors_source.read_by_column("fullname", courses[view_course - 1].doctor.ToString()));
                    if (course_announcments.Length > 0)
                    {
                        for (int i = 0; i < course_announcments.Length; i++)
                        {
                            Console.WriteLine(" #IMPORTANT : {0}", course_announcments[i]);
                        }
                    }
                    view_assignment(courses[view_course - 1], this_student);
                    Console.WriteLine("Enter 1 to choose onther course or 2 to back to main menue"); Console.WriteLine();
                    choice = int.Parse(Console.ReadLine());
                }
                while (choice == 1);
            }
           
        }
        private static void view_assignment(course this_course, student this_student)
        {
            bool view_assignments = true;
            do
            {
                datafile assignments_source = new datafile("data/assignments.txt", "code");
                datafile std_assign_source = new datafile("data/std_assign.txt", "student", "assignment");
                string[] course_assignments_codes = assignments_source.read_all_maches(this_course.code, "course", "code");
                string[] assignments_data = new string[course_assignments_codes.Length];
                for (int i = 0; i < assignments_data.Length; i++)
                {
                    assignments_data[i] = assignments_source.read_record(course_assignments_codes[i]);
                }
                assignment[] assignments = new assignment[assignments_data.Length];
                for (int i = 0; i < assignments.Length; i++)
                {
                    string[] temp = assignments_data[i].Split(',');
                    assignments[i] = new assignment(temp[0], int.Parse(temp[5]));
                    assignments[i].name = temp[1];
                    assignments[i].course = temp[2];
                    assignments[i].deadline = DateTime.Parse(temp[3]);
                    assignments[i].full_mark = int.Parse(temp[4]);
                    for (int l = 0; l < assignments[i].num_of_questions; l++)
                    {
                        string[] temp2 = temp[6].Split('*');
                        assignments[i].content[l] = temp2[l];
                    }
                }
                if (assignments.Length == 0)
                {
                    Console.WriteLine("No assignments availble");
                }
                else
                {
                    List<std_assign> std_assigns = new List<std_assign>();
                    for (int i = 0; i < assignments.Length; i++)
                    {
                        string temp = std_assign_source.read_record(this_student.id.ToString(), assignments[i].code);
                        if (temp != "")
                        {
                            string[] temp2 = temp.Split(',');
                            std_assigns.Add(new std_assign(int.Parse(temp2[0]), temp2[1]));
                            std_assigns[std_assigns.Count - 1] = new std_assign(int.Parse(temp2[0]), temp2[1]);
                            std_assigns[std_assigns.Count - 1].answer = temp2[2];
                            std_assigns[std_assigns.Count - 1].grade = int.Parse(temp2[3]) != -1 ? temp2[3] : "NA";
                        }

                    }
                    Console.WriteLine("Course has {0} assignments", assignments.Length);
                    for (int i = 0; i < assignments.Length; i++)
                    {
                        bool submit = false;
                        string grade = "NA";
                        Console.Write("Assignment{0}: {1}  ", i + 1, assignments[i].name);
                        for (int l = 0; l < std_assigns.Count; l++)
                        {
                            if (std_assigns[l].assignment == assignments[i].code)
                            {
                                submit = true;
                                grade = std_assigns[l].grade.ToString();
                            }
                        }
                        if (submit)
                            Console.WriteLine("Submitted - grade:{0}/{1}", grade, assignments[i].full_mark);
                        else if (assignments[i].deadline < DateTime.Now)
                            Console.WriteLine("Not submitted - deadline:Closed");
                        else
                            Console.WriteLine("Not submitted - deadline:{0}", assignments[i].deadline.ToString());
                    }
                    int choice = -1;
                    do
                    {
                        Console.WriteLine(); Console.WriteLine("Please make a choice: ");
                        Console.WriteLine("1-Unregister from the course");
                        Console.WriteLine("2-Submit solution");
                        Console.WriteLine("3-Back");
                        Console.WriteLine("Enter choice: ");
                        choice = int.Parse(Console.ReadLine());
                        switch (choice)
                        {
                            case 1:
                                unregister_course(this_student, this_course);
                                view_assignments = false;
                                break;
                            case 2:
                                if (assignments.Length == 0)
                                {
                                    Console.WriteLine("No assignment availble in this course");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Which of [1-{0}] to submit?", assignments.Length);
                                    int choice2 = int.Parse(Console.ReadLine());
                                    solve_assignment(this_student, assignments[choice2 - 1]);
                                    Console.WriteLine();
                                    Console.WriteLine("Your answer has been submitted successfuly ..... Please make a choice");
                                    Console.WriteLine("1)Submit onther assignment");
                                    Console.WriteLine("2)Back ");
                                    choice2 = int.Parse(Console.ReadLine());
                                    switch (choice2)
                                    {
                                        case 1:
                                            view_assignments = true; break;
                                        case 2:
                                            view_assignments = false; break;
                                    }

                                    break;
                                }

                            case 3:
                                view_assignments = false;
                                break;
                            default:
                                choice = -1; break;
                        }
                    }
                    while (choice == -1);
                }
            
            }
            while (view_assignments == true);
            
        }
        private static void unregister_course(student this_student , course this_course)
        {
            datafile std_crs_source = new datafile("data/std_crs.txt", "student", "course");
            datafile std_assign_source = new datafile("data/std_assign.txt", "student", "assignment");
            datafile assignments_source = new datafile("data/assignments.txt", "code");
            datafile courses_source = new datafile("data/courses.txt", "code");
            std_crs_source.delete_record(this_student.id.ToString(), this_course.code);
            string[] courses_assignments = assignments_source.read_all_maches(this_course.code, "course", "code");
            string[] students_assignments = std_assign_source.read_all_maches(this_student.id.ToString(), "student", "assignment");
            for(int i = 0; i < students_assignments.Length; i++)
            {
                for(int l = 0; l < courses_assignments.Length; l++)
                {
                    if (students_assignments[i] == courses_assignments[l])
                    {
                        std_assign_source.delete_record(this_student.id.ToString(), students_assignments[i]);
                        break;
                    }
                }
            }
            this_course.occupied-=1;
            courses_source.update_by_column("occupied", this_course.code, this_course.occupied.ToString());
            Console.WriteLine("Unregesteration completed successfully");
        }
        private static void solve_assignment(student this_student , assignment this_assignment)
        {
            if (this_assignment.deadline < DateTime.Now)
                Console.WriteLine("Sorry,You can't edit this assignment anymore");
            else
            {
                datafile std_assign_source = new datafile("data/std_assign.txt", "student", "assignment");
                datafile courses_source = new datafile("data/courses.txt", "code");
                string std_assign = std_assign_source.read_record(this_student.id.ToString(), this_assignment.code);
                if (std_assign != "")
                    Console.WriteLine("This assignment already submited");
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("submitting:{0}  with code:{1}    course:{2}", this_assignment.name, this_assignment.code,
                        courses_source.read_by_column("name", this_assignment.course));
                    Console.WriteLine("Deadline:{0}      marks:{1}", this_assignment.deadline, this_assignment.full_mark);
                    Console.WriteLine("Assignment:");
                    for(int i = 0; i < this_assignment.content.Length; i++)
                    {
                        Console.WriteLine("{0}){1}", i + 1, this_assignment.content[i]);
                    }
                    Console.WriteLine("Write your answer below in one line:");
                    string answer = Console.ReadLine();
                    std_assign_source.new_record(this_student.id.ToString(),this_assignment.code);
                    std_assign_source.update_by_column("answer", this_student.id.ToString(),this_assignment.code,answer);
                    std_assign_source.update_by_column("grade", this_student.id.ToString(), this_assignment.code, "-1");

                }
            }
        }
        public static void show_grades(student this_student)
        {
            datafile std_crs_source = new datafile("data/std_crs.txt", "student", "course");
            datafile std_assign_source = new datafile("data/std_assign.txt", "student", "assignment");
            datafile assignments_source = new datafile("data/assignments.txt", "code");
            datafile courses_source = new datafile("data/courses.txt","code");
            string[] student_courses = std_crs_source.read_all_maches(this_student.id.ToString(), "student", "course");
            int[] courses_assignments_numpers = new int[student_courses.Length];
            string[] all_student_assignments = std_assign_source.read_all_maches(this_student.id.ToString(),"student","assignment");
            string[] all_student_grades = std_assign_source.read_all_maches(this_student.id.ToString(), "student", "grade");
            string[] all_assignments_full = new string[all_student_assignments.Length];
            string[] assignments_courses = new string[all_student_assignments.Length];
            for (int i = 0; i < student_courses.Length; i++)
            {
                string[] temp = assignments_source.read_all_maches(student_courses[i], "course", "code");
                courses_assignments_numpers[i] = temp.Length;
            }
            for (int i = 0; i < assignments_courses.Length; i++)
            {
                assignments_courses[i] = assignments_source.read_all_maches(all_student_assignments[i], "code", "course")[0];
                all_assignments_full[i]= assignments_source.read_all_maches(all_student_assignments[i],"code","fullmark")[0];
            }
            for(int i = 0; i < student_courses.Length; i++)
            {
                int num_of_students_assigns = 0;
                int grade = 0;
                int full = 0;

                for (int l = 0; l < assignments_courses.Length; l++)
                {
                    if (assignments_courses[l] == student_courses[i])
                    {
                        num_of_students_assigns++;
                        if (all_student_grades[l] != "-1")
                        {

                            full += int.Parse(all_assignments_full[l]);
                            grade += int.Parse(all_student_grades[l]);

                        }
                        else
                        {
                            if (DateTime.Parse(assignments_source.read_by_column("deadline", all_student_assignments[l])) > DateTime.Now)
                            {

                            }
                            else
                            {
                                full += int.Parse(all_assignments_full[l]);
                            }
                        }
                    }

                }
                Console.Write("Course code:{0}... Total submitted assignment:{1} ", student_courses[i],num_of_students_assigns);
                Console.WriteLine("From:{0}        Grade:{1}/{2}", courses_assignments_numpers[i],grade,full);
            }
            Console.WriteLine("Press any key to back");
            Console.ReadKey();
        }
    }
    public class doctor_operations : general_operations
    {
        public static void new_course(doctor this_doctor)
        {
            datafile courses_resource = new datafile("data/courses.txt", "code");
            string[] keys = courses_resource.read_column("code");
            Console.WriteLine("Enter the Course's code ");
            string code = Console.ReadLine();
            bool is_key_availble = true;
            for(int i = 0; i < keys.Length; i++)
            {
                if (keys[i] == code)
                    is_key_availble = false;break;
            }
            if (is_key_availble)
            {
                course thi_course = new course(code);
                Console.Write("Enter the course's name: ");
                thi_course.name = Console.ReadLine();
                Console.Write("Enter the course's password: ");
                thi_course.password = Console.ReadLine();
                Console.Write("Is course has prerequisite(Y/N)? ");
                char pre = char.Parse(Console.ReadLine());
                switch (pre)
                {
                    case 'y':
                        bool pre_exist = false;
                        while (pre_exist==false)
                        {
                            Console.Write("Enter the prerequisite code: ");
                            thi_course.pre = Console.ReadLine();
                            for (int i = 0; i < keys.Length; i++)
                            {
                                if (thi_course.pre == keys[i])
                                {
                                    pre_exist = true;
                                    break;
                                }

                            }
                            if (pre_exist == false)
                                Console.WriteLine("Course doesn't exist , Please try again");
                        }
                        break;
                    case 'n':
                        thi_course.pre = "null";
                        break;
                    
                }
                thi_course.doctor = this_doctor.id;
                Console.Write("Is course has limit(Y/N)? ");
                char limit = char.Parse(Console.ReadLine());
                switch (limit)
                {
                    case 'y':
                        Console.Write("Enter the limit: ");
                        thi_course.limit = int.Parse(Console.ReadLine());
                        break;
                    case 'n':
                        thi_course.limit = 0;
                        break;
                }
                thi_course.occupied = 0;
                courses_resource.new_record(thi_course.code);
                courses_resource.update_by_column("name", thi_course.code, thi_course.name);
                courses_resource.update_by_column("password", thi_course.code, thi_course.password);
                courses_resource.update_by_column("pre", thi_course.code, thi_course.pre);
                courses_resource.update_by_column("doctor", thi_course.code, thi_course.doctor.ToString());
                courses_resource.update_by_column("limit", thi_course.code, thi_course.limit.ToString());
                courses_resource.update_by_column("occupied", thi_course.code, thi_course.occupied.ToString());
                Console.WriteLine("Cours had been created successfully");Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Code is not availble");Console.WriteLine();
            }
        }
        public static void view_course(doctor this_doctor)
        {
            datafile courses_source = new datafile("data/courses.txt", "code");
            datafile std_crs_source = new datafile("data/std_crs.txt", "student", "course");
            datafile crs_ta_source = new datafile("data/crs_ta.txt", "course", "ta");
            datafile assignments_source = new datafile("data/assignments.txt", "code");
            datafile std_assign_source = new datafile("data/std_assign.txt", "student", "assignment");
            datafile technical_assistant_source = new datafile("data/tech_assistants.txt", "id");
            string[] doctor_course_codes = courses_source.read_all_maches(this_doctor.id.ToString(), "doctor", "code");
            if (doctor_course_codes.Length == 0)
            {
                Console.WriteLine("No courses availble");
            }
            else
            {
                course[] doctor_courses = new course[doctor_course_codes.Length];
                for (int i = 0; i < doctor_course_codes.Length; i++)
                {
                    string[] temp = courses_source.read_record(doctor_course_codes[i]).Split(',');
                    doctor_courses[i] = new course(temp[0]);
                    doctor_courses[i].name = temp[1];
                    doctor_courses[i].password = temp[2];
                    doctor_courses[i].pre = temp[3];
                    doctor_courses[i].doctor = int.Parse(temp[4]);
                    doctor_courses[i].limit = int.Parse(temp[5]);
                    doctor_courses[i].occupied = int.Parse(temp[6]);
                }
                int choice = -1;
                while (choice == -1)
                {
                    Console.WriteLine("My courses list:");
                    for (int i = 0; i < doctor_courses.Length; i++)
                    {
                        Console.WriteLine("{0})Course: {1}  -  Code:{2}", i + 1, doctor_courses[i].name, doctor_courses[i].code);
                    }
                    Console.WriteLine("which ith [1-{0}] course to view?", doctor_courses.Length);
                    choice = int.Parse(Console.ReadLine());
                    string[] course_assignments_codes = assignments_source.read_all_maches(doctor_courses[choice - 1].code, "course", "code");
                    assignment[] course_assignments = new assignment[course_assignments_codes.Length];
                    for (int i = 0; i < course_assignments.Length; i++)
                    {
                        string[] temp = assignments_source.read_record(course_assignments_codes[i]).Split(',');
                        course_assignments[i] = new assignment(temp[0], int.Parse(temp[5]));
                        course_assignments[i].name = temp[1];
                        course_assignments[i].course = temp[2];
                        course_assignments[i].deadline = DateTime.Parse(temp[3]);
                        course_assignments[i].full_mark = int.Parse(temp[4]);
                        for (int l = 0; l < course_assignments[i].num_of_questions; l++)
                        {
                            string[] temp2 = temp[6].Split('*');
                            course_assignments[i].content[l] = temp2[l];
                        }
                    }
                    int choice2 = -1;
                    Console.Write("Course: {0}  -  Code:{1}    ", doctor_courses[choice - 1].name, doctor_courses[choice - 1].code);
                    Console.WriteLine("number of students = {0}/{1} ", doctor_courses[choice - 1].occupied, doctor_courses[choice - 1].limit);
                    Console.WriteLine("Course's password : {0}", doctor_courses[choice - 1].password);
                    string[] Course_technical_assistants = crs_ta_source.read_all_maches(doctor_courses[choice - 1].code, "course", "ta");
                    Console.Write("Technical assistants : ");
                    foreach (string assistant in Course_technical_assistants)
                        Console.Write(" - {0}", technical_assistant_source.read_by_column("fullname", assistant));
                    if (Course_technical_assistants.Length == 0)
                        Console.Write("Course has no technical assistants");
                    Console.WriteLine();
                    Console.WriteLine("Course has {0} assignments:", course_assignments.Length);
                    for (int i = 0; i < course_assignments.Length; i++)
                    {
                        Console.Write("{0})Assignment: {1} - code: {2} ", i + 1, course_assignments[i].name, course_assignments[i].code);
                        Console.WriteLine("number of submissions: {0}", std_assign_source.read_all_maches(course_assignments[i].code, "assignment", "student").Length);
                    }
                    Console.WriteLine(); Console.WriteLine("Please make a choice:");
                    Console.WriteLine("1)Create a new assignment");
                    Console.WriteLine("2)View existed assignment");
                    Console.WriteLine("3)Show a grade report");
                    Console.WriteLine("4)Invite technical assistant");
                    Console.WriteLine("5)Make an announcement");
                    Console.WriteLine("6)Back");
                    choice2 = int.Parse(Console.ReadLine());
                    switch (choice2)
                    {
                        case 1:
                            create_assignment(doctor_courses[choice - 1]);
                            Console.WriteLine(); choice = -1;
                            break;
                        case 2:
                            Console.Write("Which ith [1-{0}] assignment to view? ", course_assignments.Length);
                            int assignment_number = int.Parse(Console.ReadLine());
                            view_assignment(course_assignments[assignment_number - 1]);
                            Console.WriteLine(); choice = -1;
                            break;
                        case 3:
                            show_grade_report(doctor_courses[choice - 1], course_assignments);
                            Console.WriteLine(); choice = -1;
                            break;
                        case 4:
                            invite_tech_asssistant(doctor_courses[choice - 1]);
                            Console.WriteLine(); choice = -1;
                            break;
                        case 5:
                            make_announcement(doctor_courses[choice - 1]);
                            Console.WriteLine(); choice = -1;
                            break;
                        case 6:
                            break;
                        default:
                            Console.WriteLine("Wrong choice , Please try again");
                            choice = -1;
                            break;
                    }
                }
            } 
        }
       
        private static void invite_tech_asssistant(course this_course)
        {
            datafile tech_assistants_source = new datafile("data/tech_assistants.txt", "id");
            datafile crs_ta_source = new datafile("data/crs_ta.txt", "course", "ta");
            string[] tech_assistant_codes = tech_assistants_source.read_column("id");
            string[] crs_tech_assitant_codes = crs_ta_source.read_all_maches(this_course.code, "course", "ta");
            List<string> availble_tech_assistants_codes = new List<string>();
            List<string> availble_tech_assistants_names = new List<string>();
            for (int i = 1; i < tech_assistant_codes.Length; i++)
            {
                bool check = true;
                for (int l = 0; l < crs_tech_assitant_codes.Length; l++)
                {
                    if (tech_assistant_codes[i] == crs_tech_assitant_codes[l])
                    {
                        check = false;
                        break;
                    }
                }
                if (check == true)
                    availble_tech_assistants_codes.Add(tech_assistant_codes[i]);
            }
            if (availble_tech_assistants_codes.Count == 0)
            {
                Console.WriteLine("No technical assistants availble");
            }
            else
            {
                for (int i = 0; i < availble_tech_assistants_codes.Count; i++)
                {
                    availble_tech_assistants_names.Add(tech_assistants_source.read_by_column("fullname", availble_tech_assistants_codes[i]));
                }

                if (availble_tech_assistants_codes.Count == 0)
                {
                    Console.WriteLine("No Technical assistant availble");
                }
                else
                {
                    Console.WriteLine("Please make a choice:");
                    Console.WriteLine("1)Enter technical assistant id");
                    Console.WriteLine("2)List all technical assistant");
                    Console.Write("Enter a choice:");
                    int choice = int.Parse(Console.ReadLine());
                    int ta_index = -1;
                    int choice2 = -1;
                    switch (choice)
                    {
                        case 1:
                            while (choice2 == -1)
                            {
                                Console.Write("Enter th id");
                                choice2 = int.Parse(Console.ReadLine());
                                for (int i = 0; i < availble_tech_assistants_codes.Count; i++)
                                {
                                    if (choice2.ToString() == availble_tech_assistants_codes[i])
                                    {
                                        ta_index = i;
                                        break;
                                    }
                                }
                                if (ta_index == -1)
                                {
                                    Console.WriteLine("Wrong choice , Please try again");
                                    choice2 = -1;
                                }
                            }
                            break;
                        case 2:
                            while (choice2 == -1)
                            {
                                for (int i = 0; i < availble_tech_assistants_codes.Count; i++)
                                {
                                    Console.WriteLine("{0}){1} - code {2}", i + 1, availble_tech_assistants_names[i], availble_tech_assistants_codes[i]);
                                }
                                Console.Write("Which ith [1:{0}] technica assistant to invite: ", availble_tech_assistants_codes.Count);
                                ta_index = int.Parse(Console.ReadLine());
                                if (ta_index > 0 && ta_index <= availble_tech_assistants_codes.Count)
                                {
                                    ta_index--;
                                    choice2 = 0;
                                }
                                else
                                {
                                    Console.WriteLine("Wrong choice , Please try again");
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    if (choice == 1 || choice == 2)
                    {
                        Console.WriteLine("Assignments permission (T/F): ");
                        char assignment_permission = char.Parse(Console.ReadLine());
                        switch (assignment_permission)
                        {
                            case 't':
                                crs_ta_source.new_record(this_course.code, availble_tech_assistants_codes[ta_index]);
                                crs_ta_source.update_by_column("approve", this_course.code, availble_tech_assistants_codes[ta_index], "0");
                                crs_ta_source.update_by_column("assignment", this_course.code, availble_tech_assistants_codes[ta_index], "1");
                                Console.WriteLine("invite had been sent successfully"); Console.WriteLine();
                                break;
                            case 'f':
                                crs_ta_source.new_record(this_course.code, availble_tech_assistants_codes[ta_index]);
                                crs_ta_source.update_by_column("approve", this_course.code, availble_tech_assistants_codes[ta_index], "0");
                                crs_ta_source.update_by_column("assignment", this_course.code, availble_tech_assistants_codes[ta_index], "0");
                                Console.WriteLine("invite had been sent successfully"); Console.WriteLine();
                                break;
                            default:
                                Console.WriteLine("Wrong choice , Please try again");
                                break;
                        }
                    }
                }
            }   
        }
        private static void make_announcement(course thid_course)
        {
            datafile announcments_source = new datafile("data/announcments.txt", "course", "announcment");
            Console.WriteLine("Please Enter the announcment in one line:");
            string announcment = Console.ReadLine();
            announcments_source.new_record(thid_course.code, announcment);
            Console.WriteLine("Announcment had been sent successfully");Console.WriteLine();
        }
    }
    public class Technical_assistant_operations : general_operations
    {
        public static void view_course(tech_assistant this_tech_assistant)
        {
            datafile crs_ta_source = new datafile("data/crs_ta.txt", "course", "ta");
            datafile courses_source = new datafile("data/courses.txt", "code");
            datafile std_assign_source = new datafile("data/std_assign.txt", "student", "assignment");
            datafile assignments_source = new datafile("data/assignments.txt", "code");
            string[] ta_courses_codes = crs_ta_source.read_all_maches(this_tech_assistant.id.ToString(), "ta", "course");
            course[] ta_courses = new course[ta_courses_codes.Length];
            if (ta_courses.Length == 0)
            {
                Console.WriteLine("No courses availble");
            }
            else
            {
                for (int i = 0; i < ta_courses_codes.Length; i++)
                {
                    string[] temp = courses_source.read_record(ta_courses_codes[i]).Split(',');
                    ta_courses[i] = new course(temp[0]);
                    ta_courses[i].name = temp[1];
                    ta_courses[i].password = temp[2];
                    ta_courses[i].pre = temp[3];
                    ta_courses[i].doctor = int.Parse(temp[4]);
                    ta_courses[i].limit = int.Parse(temp[5]);
                    ta_courses[i].occupied = int.Parse(temp[6]);
                }
                int choice = -1;
                while (choice == -1)
                {
                    Console.WriteLine("My courses list:");
                    for (int i = 0; i < ta_courses.Length; i++)
                    {
                        Console.WriteLine("{0})Course: {1}  -  Code:{2}", i + 1, ta_courses[i].name, ta_courses[i].code);
                    }
                    Console.WriteLine("which ith [1-{0}] course to view?", ta_courses.Length);
                    choice = int.Parse(Console.ReadLine());
                    string[] course_assignments_codes = assignments_source.read_all_maches(ta_courses[choice - 1].code, "course", "code");
                    assignment[] course_assignments = new assignment[course_assignments_codes.Length];
                    for (int i = 0; i < course_assignments.Length; i++)
                    {
                        string[] temp = assignments_source.read_record(course_assignments_codes[i]).Split(',');
                        course_assignments[i] = new assignment(temp[0], int.Parse(temp[5]));
                        course_assignments[i].name = temp[1];
                        course_assignments[i].course = temp[2];
                        course_assignments[i].deadline = DateTime.Parse(temp[3]);
                        course_assignments[i].full_mark = int.Parse(temp[4]);
                        for (int l = 0; l < course_assignments[i].num_of_questions; l++)
                        {
                            string[] temp2 = temp[6].Split('*');
                            course_assignments[i].content[l] = temp2[l];
                        }
                    }
                    int choice2 = -1;
                    Console.Write("Course: {0}  -  Code:{1}    ", ta_courses[choice - 1].name, ta_courses[choice - 1].code);
                    Console.WriteLine("number of students = {0}/{1} ", ta_courses[choice - 1].occupied, ta_courses[choice - 1].limit);
                    Console.WriteLine("Course's password : {0}", ta_courses[choice - 1].password);
                    Console.WriteLine("Course has {0} assignments:", course_assignments.Length);
                    for (int i = 0; i < course_assignments.Length; i++)
                    {
                        Console.Write("{0})Assignment: {1} - code: {2} ", i + 1, course_assignments[i].name, course_assignments[i].code);
                        Console.WriteLine("number of submissions: {0}", std_assign_source.read_all_maches(course_assignments[i].code, "assignment", "student").Length);
                    }
                    Console.WriteLine(); Console.WriteLine("Please make a choice:");
                    if (crs_ta_source.read_record(ta_courses[choice - 1].code, this_tech_assistant.id.ToString()).Split(',')[3] == "1")
                    {
                        Console.WriteLine("1)Create a new assignment");
                        Console.WriteLine("2)View existed assignment");
                        Console.WriteLine("3)Show a grade report");
                        Console.WriteLine("4)Back");
                        choice2 = int.Parse(Console.ReadLine());
                    }
                    else
                    {
                        Console.WriteLine("1)View existed assignment");
                        Console.WriteLine("2)Show a grade report");
                        Console.WriteLine("3)Back");
                        choice2 = int.Parse(Console.ReadLine()) + 1;
                    }
                    switch (choice2)
                    {
                        case 1:
                            create_assignment(ta_courses[choice - 1]);
                            Console.WriteLine(); choice = -1;
                            break;
                        case 2:
                            Console.Write("Which ith [1-{0}] assignment to view? ", course_assignments.Length);
                            int assignment_number = int.Parse(Console.ReadLine());
                            view_assignment(course_assignments[assignment_number - 1]);
                            Console.WriteLine(); choice = -1;
                            break;
                        case 3:
                            show_grade_report(ta_courses[choice - 1], course_assignments);
                            Console.WriteLine(); choice = -1;
                            break;
                        case 4:
                            break;
                        default:
                            Console.WriteLine("Wrong choice , Please try again");
                            choice = -1;
                            break;
                    }
                }
            }
            
        }
        public static void view_invitations(tech_assistant this_tech_assistant)
        {
            datafile crs_ta_source = new datafile("data/crs_ta.txt", "course", "ta");
            datafile courses_source = new datafile("data/courses.txt", "code");
            string[] crs_ta_codes = crs_ta_source.read_all_maches(this_tech_assistant.id.ToString(), "ta", "course");
            List<crs_ta> all_crs_ta = new List<crs_ta>();
            for(int i=0; i < crs_ta_codes.Length; i++)
            {
                string[] temp = crs_ta_source.read_record(crs_ta_codes[i], this_tech_assistant.id.ToString()).Split(',');
                if (int.Parse(temp[2]) == 0)
                {
                    all_crs_ta.Add(new crs_ta(crs_ta_codes[i], this_tech_assistant.id));
                    all_crs_ta[all_crs_ta.Count - 1].approve = 0;
                    all_crs_ta[all_crs_ta.Count - 1].assignment = int.Parse(temp[3]);
                }
            }
            if (all_crs_ta.Count == 0)
            {
                Console.WriteLine("No invitations availble");
            }
            else
            {
                for (int i = 0; i < all_crs_ta.Count; i++)
                {
                    Console.Write("{0})Course:{1} - code {2} ", i + 1, courses_source.read_by_column("name", all_crs_ta[i].course), all_crs_ta[i].course);
                    Console.Write("Assignment permission: ");
                    if (all_crs_ta[i].assignment == 1)
                        Console.WriteLine("Yes");
                    else
                        Console.WriteLine("No");
                }
                Console.Write("Which ith [1-{0}] to make action? ", all_crs_ta.Count);
                int choice = int.Parse(Console.ReadLine()) - 1;
                Console.WriteLine("Accept the invitation (T/F)? ");
                char choice2 = char.Parse(Console.ReadLine());
                switch (choice2)
                {
                    case 'y':
                        all_crs_ta[choice].approve = 1;
                        crs_ta_source.update_by_column("approve", all_crs_ta[choice].course, all_crs_ta[choice].ta.ToString(), "1");
                        Console.WriteLine("The invetation had been accepted successfully"); Console.WriteLine();
                        break;
                    case 'f':
                        all_crs_ta[choice].approve = 0;
                        crs_ta_source.delete_record(all_crs_ta[choice].course, all_crs_ta[choice].ta.ToString());
                        Console.WriteLine("The invetation had been denied successfully"); Console.WriteLine();
                        break;
                }
            }          
        }
    }
    public class general_operations
    {
        protected static void create_assignment(course this_course)
        {
            datafile assignments_source = new datafile("data/assignments.txt", "code");
            int course_assignment_num = assignments_source.read_all_maches(this_course.code, "course", "code").Length + 1;
            string new_code = this_course.code + "a" + course_assignment_num.ToString();
            Console.Write("Enter the number of Questions: ");
            assignment this_assignment = new assignment(new_code, int.Parse(Console.ReadLine()));
            for (int i = 0; i < this_assignment.num_of_questions; i++)
            {
                Console.Write("question{0}: ", i + 1);
                this_assignment.content[i] = Console.ReadLine();
            }
            Console.Write("Enter the full mark: ");
            this_assignment.full_mark = int.Parse(Console.ReadLine());
            Console.Write("Enter deadline(mm-dd-yyyy): ");
            this_assignment.deadline = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter The assignment name: ");
            this_assignment.name = Console.ReadLine();
            assignments_source.new_record(this_assignment.code);
            assignments_source.update_by_column("name", this_assignment.code, this_assignment.name);
            assignments_source.update_by_column("course", this_assignment.code, this_course.code);
            assignments_source.update_by_column("deadline", this_assignment.code, this_assignment.deadline.ToString());
            assignments_source.update_by_column("fullmark", this_assignment.code, this_assignment.full_mark.ToString());
            assignments_source.update_by_column("num_of_questions", this_assignment.code, this_assignment.num_of_questions.ToString());
            assignments_source.update_by_column("content", this_assignment.code, string.Join("*", this_assignment.content));
            Console.WriteLine("Assignment created successfully");
        }
        protected static void view_assignment(assignment this_assignment)
        {
            datafile std_assign_source = new datafile("data/std_assign.txt", "student", "assignment");
            datafile students_source = new datafile("data/students.txt", "id");
            string[] std_assign_codes = std_assign_source.read_all_maches(this_assignment.code, "assignment", "student");
            std_assign[] std_assigns = new std_assign[std_assign_codes.Length];
            for (int i = 0; i < std_assigns.Length; i++)
            {
                string[] temp = std_assign_source.read_record(std_assign_codes[i], this_assignment.code).Split(',');
                std_assigns[i] = new std_assign(int.Parse(temp[0]), temp[1]);
                std_assigns[i].answer = temp[2];
                std_assigns[i].grade = temp[3];
            }
            Console.WriteLine("Assignment: {0} - code: {1} ", this_assignment.name, this_assignment.code);
            Console.WriteLine("questions:");
            foreach (string question in this_assignment.content)
                Console.WriteLine(question);
            Console.WriteLine("Assignment has {0} submissions:", std_assigns.Length);
            for (int i = 0; i < std_assigns.Length; i++)
            {
                Console.Write("{0})Student: {1}    ", i + 1, students_source.read_by_column("fullname", std_assigns[i].student.ToString()));
                if (int.Parse(std_assigns[i].grade) == -1)
                {
                    Console.WriteLine("-Not Marked-");
                }
                else
                {
                    Console.WriteLine("Grade: {0}/{1}", std_assigns[i].grade, this_assignment.full_mark);
                }
                Console.WriteLine("    Answer: {0}", std_assigns[i].answer);
            }
            Console.WriteLine(); Console.WriteLine("Please make a choice:");
            Console.WriteLine("1) Mark an assignment");
            Console.WriteLine("2) Back");
            Console.WriteLine("Enter a choice: ");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Console.Write("Which ith [1-{0}] to mark? ", std_assigns.Length);
                    choice = int.Parse(Console.ReadLine());
                    if (choice <= std_assigns.Length && choice > 0)
                    {
                        Console.Write("Enter the grade (from {0}):", this_assignment.full_mark);
                        int mark = int.Parse(Console.ReadLine());
                        if (mark >= 0 && mark <= this_assignment.full_mark)
                        {
                            std_assign_source.update_by_column("grade", std_assigns[choice - 1].student.ToString(), std_assigns[choice - 1].assignment, mark.ToString());
                            Console.WriteLine("grade submitted successfully");
                        }
                        else
                        {
                            Console.WriteLine("Unvalid grade");
                        }
                    }
                    else
                        Console.WriteLine("Wrong choice , Please try again");
                    break;
                case 2:
                    break;
                default:
                    Console.WriteLine("Wrong choice , Please try again");
                    break;
            }
        }
        protected static void show_grade_report(course this_course, assignment[] this_courses_assignments)
        {
            datafile std_assign_source = new datafile("data/std_assign.txt", "student", "assignment");
            datafile std_crs_source = new datafile("data/std_crs.txt", "student", "course");
            datafile students_source = new datafile("data/students.txt", "id");
            string[] course_students_ids = std_crs_source.read_all_maches(this_course.code, "course", "student");
            for (int i = 0; i < course_students_ids.Length; i++)
            {
                int grade = 0, full = 0, submitted = 0;
                for (int l = 0; l < this_courses_assignments.Length; l++)
                {
                    string temp = std_assign_source.read_record(course_students_ids[i], this_courses_assignments[l].code);
                    if (temp == "")
                    {
                        if (this_courses_assignments[l].deadline < DateTime.Now)
                        {
                            full += this_courses_assignments[l].full_mark;
                        }
                    }
                    else
                    {
                        string[] temp2 = temp.Split(',');
                        if (int.Parse(temp2[3]) != -1)
                        {
                            grade += int.Parse(temp2[3]);
                            full += this_courses_assignments[l].full_mark;

                        }
                        submitted++;
                    }
                }
                Console.Write("Student:{0}   ", students_source.read_all_maches(course_students_ids[i], "id", "fullname"));
                Console.Write("submitted assignments: {0}/{1}   ", submitted, this_courses_assignments.Length);
                Console.WriteLine("Grade: {0}/{1}   ", grade, full);
            }
            Console.WriteLine("Press any key to get back"); Console.ReadKey(); Console.WriteLine();
        }
    }
}
