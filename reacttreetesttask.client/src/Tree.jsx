import { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import TreeNode from './TreeNode';

function Tree({ treeName, reload, onError }) {
    const [tree, setTree] = useState();

    

    useEffect(() => {
        populateTree();
    }, []);

    const onChange = (level) => {
        if (level == 0) 
            reload();
        else 
            populateTree();
    }

   
    return (
        <div>
            <TreeNode {...tree} treeName={treeName} onChange={onChange} level={0} onError={onError} />
        </div>
    );
    
    async function populateTree() {
        
        const response = await fetch(`api.user.tree.get?treeName=${treeName}`, {method: "POST"});
        const data = await response.json();
        if (response.ok) {
            setTree(data);
        }
        else
        {
            onError(data.data.message);
        }
    }
}

Tree.propTypes = {
    treeName: PropTypes.string,
    onError: PropTypes.func,
    reload: PropTypes.func,
};

export default Tree;