using Microsoft.EntityFrameworkCore;

namespace ReactTreeTestTask.Server.Data
{
    [PrimaryKey("Id")]
    public class Node
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public List<Node> Children { get; set; }
        public Node Parent { get; set; }
    }
}
