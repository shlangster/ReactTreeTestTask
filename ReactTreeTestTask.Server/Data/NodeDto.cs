using Newtonsoft.Json;

namespace ReactTreeTestTask.Server.Data
{
    public class NodeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<NodeDto> Children { get; set; }

        public NodeDto(Node rootNode)
        {
            Id = rootNode.Id;
            Name = rootNode.Name;
            if(rootNode.Children != null)
                Children = rootNode.Children.Select(_ => new NodeDto(_)).ToList();
        }
      
    }
}
