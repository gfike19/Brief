using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Brief.Models
{
    public class LoggedUserModel
    {
        public string CreatorID { get; set; }
        public string CreatorFirstName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        //public int SaveDetails()
        //{
        //    SqlConnection con = new SqlConnection(GetConString.ConString());
        //    string query = "SELECT FirstName, Id FROM AspNetUsers WHERE UserName = " + CreatorUserName;
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    con.Open();

        //    int i = cmd.ExecuteNonQuery();
        //    con.Close();
        //    return i;

        //    using (conn)
        //    {
        //        conn.Open();
        //        SqlDataReader rdr = cmd.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            var blog = new Blog();
        //            blog.CreatorName = rdr["CreatorName"].ToString();
        //            blog.Title = rdr["Title"].ToString();
        //            blog.Content = rdr["Content"].ToString();
        //            model.Add(blog);
        //        }
        //    }
        //}
    }
}
