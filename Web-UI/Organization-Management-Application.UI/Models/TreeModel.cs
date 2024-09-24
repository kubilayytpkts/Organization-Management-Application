namespace Organization_Management_Application.UI.Models
{
    public class TreeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TreeModel> Children { get; set; } = new List<TreeModel>();
    }
}
