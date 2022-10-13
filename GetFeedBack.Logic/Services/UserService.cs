using GetFeedBack.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetFeedBack.Logic.Services
{
    public class UserService : IUserService
    {
        public string GetRandomString()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
