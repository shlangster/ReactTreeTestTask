using Newtonsoft.Json;
using System.Collections.Generic;

namespace ReactTreeTestTask.Server.Data
{
    public class NodeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public IList<NodeDto> Children { get; set; }

        public NodeDto(Node rootNode)
        {
            Id = rootNode.Id;
            Name = rootNode.Name;
            Children = new List<NodeDto>();
            if(rootNode.Children != null)
                Children = rootNode.Children.Select(_ => new NodeDto(_)).ToList();
        }
      
    }
}
