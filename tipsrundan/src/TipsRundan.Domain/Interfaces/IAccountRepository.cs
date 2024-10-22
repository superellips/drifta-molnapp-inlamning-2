using System;
using System.Collections.Generic;
using TipsRundan.Domain.Entities;

namespace TipsRundan.Domain.Interfaces;

    public interface IAccountRepository
    {
        User ReadById(Guid id);
        User ReadByName(string userName);
        List<User> ReadAll();
        User Create(User user);
        User Update(User user);
        User Delete(User user);
    }
