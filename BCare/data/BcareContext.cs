﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using BCare.Models;
using System.Security.Cryptography;
using System.Text;

namespace BCare.data
{
    public class BcareContext
    {
        public string ConnectionString { get; set; }
        public BcareContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM pharmaceutical_company", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User()
                        {
                            UserID = reader.GetInt32("Pharm_ID"),
                            FirstName = reader.GetString("Pharm_Name")
                        });
                    }
                }
            }

            return users;
        }
        public bool Login(string username , string password)
        {
            User user = new User();
            using (MySqlConnection conn = GetConnection())
            {
  
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM premission_for_users", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if(username.Equals(reader.GetString("User_Name")))
                        {
                            var sha512 = SHA512.Create();
                            byte[] bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
                            string hashedPassword = BitConverter.ToString(bytes).Replace("-", "");
                            if (hashedPassword.Equals(reader.GetString("PW_Hash"))) {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public void SignUp(int User_ID, string First_Name, string Last_Name, string Gender, string Birth_Date, int HMO_ID, string Blood_Type, string Address)
        {
            //int userId = GenerateAutoID();
            // User newUser = new User()
            //{
            //    UserID = userId,
            //    HMOID = HMOID,
            //    FirstName = firstname,
            //    LastName = lastname,
            //    Gender = new Gender (),
            //    BirthDate = Convert.ToDateTime(birhdate),
            //    BloodType = new BloodType(),
            //    Address = address
            //};
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO users VALUES (User_ID, First_Name, Last_Name, Gender, Birth_Date, HMO_ID, Blood_Type, Address) ", conn);
                cmd.ExecuteNonQuery();
                // 'FOREIGN KEY (HMO_ID) REFERENCES health_maintenance_organizations (HMO_ID)'
            }
           // return newUser;
        }

        private int GenerateAutoID()
        {
            int i;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select Count (User_ID) from users", conn);
                i = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
                i++;
            }
            return i;
        }

        public List<blood_test> GetUserTests(int userId)
        {
            List<blood_test> testsForUser = new List<blood_test>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from blood_test WHERE BUser_ID=UserID", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        testsForUser.Add(new blood_test()
                        {
                            BTestID = reader.GetInt32("BTest_ID"),
                            BUserID = reader.GetInt32("BUser_ID"),
                            BTestDate = reader.GetDateTime("BTest_Date"),
                            //BDocName = reader.GetString("BDoc_Name")
                        });
                    }
                }
            }
            return testsForUser;
        }

        public List<blood_test_data> GetTestResulltByID (int testId)
        {
            List<blood_test_data> bloodTestResult = new List<blood_test_data>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from blood_test_data WHERE BTest_ID=testID", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bloodTestResult.Add(new blood_test_data()
                        {
                            BTestID = reader.GetInt32("BTest_ID"),
                            BCompID = reader.GetInt32("BComp_ID"),
                            Value = reader.GetDouble("Value")
                        });
                    }
                }
            }

            return bloodTestResult;
        }
    }
}
