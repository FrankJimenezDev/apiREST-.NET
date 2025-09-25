namespace Common.Interfaces
{
    public interface IUser
    {
        int Id { get; set; }
        string Nombre { get; set; }
        string Dni { get; set; }
        ICollection<IAccount> Accounts { get; set; }
    }
}
