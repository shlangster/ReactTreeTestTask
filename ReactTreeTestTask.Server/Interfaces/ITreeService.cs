using ReactTreeTestTask.Server.Data;

namespace ReactTreeTestTask.Server.Interfaces
{
    public interface ITreeService
    {
        public Task<NodeDto> GetTree(string treeName);
        public List<NodeDto> GetTreeList();
        public Task CreateNode(string treeName, int parentNodeId, string nodeName);
        public Task DeleteNode(string treeName, int nodeId);
        public Task RenameNode(string treeName, int nodeId, string newNodeName);

    }
}
