namespace GymManagment.DAL.Models
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; } = default!;
        public ICollection<Session> Session { get; set; } = new List<Session>();
    }
}