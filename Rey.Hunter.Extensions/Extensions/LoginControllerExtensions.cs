using Rey.Hunter.Models.Identity;
using Rey.Identity.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Rey.Hunter.Models;

namespace Microsoft.AspNetCore.Mvc {
    public static class LoginControllerExtensions {
        public static ILoginManager<User, Role, Account> LoginManager(this Controller controller) {
            return controller.HttpContext.RequestServices?.GetService<ILoginManager<User, Role, Account>>();
        }

        public static ILoginContext<User> LoginContext(this Controller controller) {
            return controller.HttpContext.RequestServices?.GetService<ILoginContext<User>>();
        }

        public static User CurrentUser(this Controller controller) {
            return controller.LoginContext().User;
        }

        public static Account CurrentAccount(this Controller controller) {
            return controller.CurrentUser().Account.Concrete(controller.GetMonDatabase());
        }

        public static TModel AttachCurrentAccount<TModel>(this Controller controller, TModel model)
            where TModel : AccountModel {
            model.Account = controller.CurrentAccount();
            return model;
        }
    }
}
