using AssignmentUWP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AssignmentUWP.Services
{
    class CheckValid
    {
        private bool isValidFirstName;
        private bool isValidLastName;
        private bool isValidEmail;
        private bool isValidPassword;
        private bool isValidPhone;
        private bool isValidAddress;

        private bool isValidAll;

        public bool IsValidFirstName { get => isValidFirstName; set => isValidFirstName = value; }
        public bool IsValidLastName { get => isValidLastName; set => isValidLastName = value; }
        public bool IsValidEmail { get => isValidEmail; set => isValidEmail = value; }
        public bool IsValidPassword { get => isValidPassword; set => isValidPassword = value; }
        public bool IsValidPhone { get => isValidPhone; set => isValidPhone = value; }
        public bool IsValidAddress { get => isValidAddress; set => isValidAddress = value; }

        public bool IsValidAll { get => isValidAll; set => isValidAll = value; }

        public string FirstName_Message = "First Name không được để trống";
        public string LastName_Message = "Last Name không được để trống";
        public string Email_Message = "Email không được để trống và phải chứa kí tự @";
        public string Password_Message = "Password phải có ít nhất 5 kí tự";
        public string Phone_Message = "Phone phải chứa các kí tự là số";
        public string Address_Message = "Address không được để trống";


        public Dictionary<String, String> listError;

        public bool CheckValidFirstName(string firstName)
        {
            if(firstName == "")
            {
                IsValidFirstName = false;
            }
            else
            {
                IsValidFirstName = true;
            }
            return IsValidFirstName;
        }

        public bool CheckValidLastName(string lastName)
        {
            if (lastName == "")
            {
                IsValidLastName = false;
            }
            else
            {
                IsValidLastName = true;
            }
            return IsValidLastName;
        }

        public bool CheckValidEmail(string email)
        {
            if (email == null)
            {
                IsValidEmail= false;
            }
            else if (!email.Contains('@'))
            {
                IsValidEmail = false;
            }
            else
            {
                IsValidEmail = true;
            }
            return IsValidEmail;
        }

        public bool CheckValidPassword(string password)
        {
            if (password == null)
            {
                IsValidPassword = false;
            }
            else if(password.Length < 5)
            {
                IsValidPassword = false;
            }
            else
            {
                IsValidPassword = true;
            }
            return IsValidPassword;
        }

        public bool CheckValidPhone(string phone)
        {
            if (phone == null)
            {
                IsValidPhone = false;
            }
            else 
            {
                IsValidPhone = true;
            }            
            return IsValidPhone;
        }

        public bool CheckValidAddress(string address)
        {
            if (address == "")
            {
                IsValidAddress = false;
            }
            else
            {
                IsValidAddress = true;
            }
            return IsValidAddress;
        }

        //public bool CheckValidAll(Member member)
        //{
        //    if(CheckValidFirstName(member.firstName) == false ||
        //        CheckValidLastName(member.lastName) ==false ||
        //        CheckValidEmail(member.email) == false ||
        //        CheckValidPassword(member.password) ==false ||
        //        CheckValidPhone(member.phone) == false ||
        //        CheckValidAddress(member.address) == false)
        //    {
        //        IsValidAll = false;
        //        return false;
        //    }
        //    else
        //    {
        //        IsValidAll = true;
        //    }
        //    return IsValidAll;
        //}

        public bool CheckValidAll(Member member)
        {
            listError = new Dictionary<string, string>();
            if(CheckValidFirstName(member.firstName) == false)
            {
                listError.Add("FirstName_Message", FirstName_Message);
            }
            if (CheckValidLastName(member.lastName) == false)
            {
                listError.Add("LastName_Message", LastName_Message);
            }
            if (CheckValidEmail(member.email) == false)
            {
                listError.Add("Email_Message", Email_Message);
            }
            if (CheckValidPassword(member.password) == false)
            {
                listError.Add("Password_Message", Password_Message);
            }
            if (CheckValidPhone(member.phone) == false)
            {
                listError.Add("Phone_Message", Phone_Message);
            }
            if (CheckValidAddress(member.address) == false)
            {
                listError.Add("Address_Message", Address_Message);
            }
            if(listError.Count != 0)
            {
                IsValidAll = false;
            }
            else
            {
                IsValidAll = true;
            }
            return IsValidAll;
        }        
    }
}
