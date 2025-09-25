namespace Common.Interfaces
{
    public interface IAccount
    {
        int Id { get; set; }
        string Number { get; set; }
        decimal Balance { get; set; }
        int UserId { get; set; }
        IUser User { get; set; }
    }
}
