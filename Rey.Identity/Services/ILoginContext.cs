using Rey.Identity.Models;

namespace Rey.Identity.Services {
    public interface ILoginContext<TUser>
        where TUser : class, IUser {
        TUser User { get; }
    }
}
