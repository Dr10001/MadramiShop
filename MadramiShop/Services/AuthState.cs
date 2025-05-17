using MadramiShop.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadramiShop.Services;

public class AuthState
{
    private const string UserDataKey = "user";

    public AuthState()
    {
        var loggedInUser = MauiInterop.GetFromStorage<LoggedInUser?>(UserDataKey, null);
        if (loggedInUser != null)
        {
            User = loggedInUser;
            IsLoggedIn = true;
        }
    }
    public LoggedInUser? User { get; set; }
    public bool IsLoggedIn { get; private set; }
    public void Login(LoggedInUser loggedInUser) 
    {
        MauiInterop.SaveToStorage(UserDataKey, loggedInUser);
        User = loggedInUser;
        IsLoggedIn = true;
    }

    public void Logout()
    {
        User = null;
        IsLoggedIn = false;
        MauiInterop.RemoveFromStorage(UserDataKey);
    }
    public string RedirectUrlFromLogin { get; set; } = "/";
}
