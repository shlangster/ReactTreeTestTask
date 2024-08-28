using ReactTreeTestTask.Server.Data;
using ReactTreeTestTask.Server.Interfaces;

namespace ReactTreeTestTask.Server.Services
{
    public class TreeService : ITreeService
    {
        private AppDbContext _dbContext {  get; set; }
        public TreeService(AppDbContext dbContext) { 
            _dbContext = dbContext;
        }

        public async Task<NodeDto> GetTree(string treeName)
        {
            var root = _dbContext.Nodes.FirstOrDefault(_ => _.Name == treeName && _.ParentId == null);
            if (root == null)
            {
                root = new Node
                {
                    Name = treeName,
                };
                _dbContext.Nodes.Add(root);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var fullTree = await _dbContext.SelectFullTree(root.Id);
                root = fullTree.FirstOrDefault();
            }

            return new NodeDto(root);
        }

        public async Task CreateNode(string treeName, int parentNodeId, string nodeName)
        {
            var root = _dbContext.Nodes.FirstOrDefault(_ => _.Name == treeName && _.ParentId == null);
            if (root == null)
                throw new SecureException($"Tree with Name = {treeName} was not found");
            
            var parentNode = _dbContext.Nodes.FirstOrDefault(_ => _.ParentId != null && _.Id == parentNodeId);
            if (parentNode == null)
                throw new SecureException($"Node with ID = {parentNodeId} was not found");
            

            var fullTree = await _dbContext.SelectFullTree(root.Id);
            if (!fullTree.Any(_ => _.Id == parentNode.Id))
                throw new SecureException("Requested node was found, but it doesn't belong your tree");
            
            var newNode = new Node()
            {
                Name = nodeName,
                ParentId = parentNode.Id
            };

            _dbContext.Nodes.Add(newNode);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteNode(string treeName, int nodeId)
        {
            var root = _dbContext.Nodes.FirstOrDefault(_ => _.Name == treeName && _.ParentId == null);
            if (root == null)
                throw new SecureException($"Tree with Name = {treeName} was not found");
            

            var node = _dbContext.Nodes.FirstOrDefault(_ => _.Id == nodeId);
            if (node == null)
                throw new SecureException($"Node with ID = {nodeId} was not found");
            
            var fullTree = await _dbContext.SelectFullTree(root.Id);
            if (!fullTree.Any(_ => _.Id == node.Id))
                throw new SecureException("Requested node was found, but it doesn't belong your tree");
            
            if (fullTree.Any(_ => _.ParentId == node.Id))
                throw new SecureException("You have to delete all children nodes first");
            
            _dbContext.Nodes.Remove(node);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RenameNode(string treeName, int nodeId, string newNodeName)
        {
            var root = _dbContext.Nodes.FirstOrDefault(_ => _.Name == treeName && _.ParentId == null);
            if (root == null)
                throw new SecureException($"Tree with Name = {treeName} was not found");
            
            var node = _dbContext.Nodes.FirstOrDefault(_ => _.Id == nodeId);
            if (node == null)
                throw new SecureException($"Node with ID = {nodeId} was not found");
            
            var fullTree = await _dbContext.SelectFullTree(root.Id);
            if (!fullTree.Any(_ => _.Id == node.Id))
                throw new SecureException("Requested node was found, but it doesn't belong your tree");
            
            node.Name = newNodeName;
            _dbContext.Nodes.Update(node);
            await _dbContext.SaveChangesAsync();
        }


    }
}
