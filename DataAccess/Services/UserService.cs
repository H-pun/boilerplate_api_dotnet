using System;
using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Linq;
using DataAccess.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataAccess.Services
{
    public class UserService : BaseManager<User>
    {

        public dynamic Login(string email)
        {
            try
            {
                var user = con.Query("usp_login", new { email }, commandType: CommandType.StoredProcedure).SingleOrDefault();
                if (((IDictionary<string, object>)user).ContainsKey("ongoingExam"))
                {
                    if (user.ongoingExam != null)
                    {
                        user.ongoingExam = JsonConvert.DeserializeObject<dynamic>(user.ongoingExam)[0];
                    }
                }
                return user;
            }
            catch { throw; }

        }
        public dynamic GetUserByToken(string token)
        {
            try
            {
                var user = con.Query("usp_getUserByToken", new { token }, commandType: CommandType.StoredProcedure).SingleOrDefault();
                if (((IDictionary<string, object>)user).ContainsKey("ongoingExam"))
                {
                    if (user.ongoingExam != null)
                    {
                        user.ongoingExam = JsonConvert.DeserializeObject<dynamic>(user.ongoingExam)[0];
                    }
                }
                return user;
            }
            catch { throw; }

        }
        public User GetByEmail(string email, bool nullable = false)
        {
            try
            {
                dynamic result = con.Query<User>("select * from users where email = @email and deletedAt is null", new { email }).SingleOrDefault();
                if (result == null && !nullable)
                {
                    throw new ArgumentException("Email not registered");
                }
                return result;
            }
            catch { throw; }
        }
        public void RegisterAdmin(User user, Admin admin)
        {
            try
            {
                con.Open();
                using (var ts = con.BeginTransaction())
                {
                    con.Insert(user, transaction: ts);
                    con.Insert(admin, transaction: ts);
                    ts.Commit();
                }
            }
            catch { con.Close(); throw; }
            finally { con.Close(); }

        }
    }
}