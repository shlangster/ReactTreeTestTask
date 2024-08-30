import { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import './App.css';
import './assets/fontawesome/css/fontawesome.css';
import './assets/fontawesome/css/solid.css';
import Popup from './Popup';

function TreeNode({ treeName, name, id, children, onChange, onError, level }) {

    const [isExpanded, setIsExpanded] = useState();
    const [addPopupVisible, setAddPopupVisible] = useState();
    const [renamePopupVisible, setRenamePopupVisible] = useState();
    const [nodeName, setNodeName] = useState("");
    const [newNodeName, setNewNodeName] = useState();
    

    useEffect(() => { setNewNodeName(name) }, [name]);


    const toggle = () => setIsExpanded(!isExpanded);

    const saveNewNode = async () => {
        const response = await fetch(`api.user.tree.node.create?treeName=${treeName}&parentNodeId=${id}&nodeName=${nodeName}`, { method: "POST" });
        if (response.ok) {
            setNodeName("");
            onChange(level+1);
        } else {
            const response_json = await response.json();
            onError(response_json.data.message);
        }
        setAddPopupVisible(false);
    }

    const renameNode = async () => {
        const response = await fetch(`api.user.tree.node.rename?treeName=${treeName}&nodeId=${id}&newNodeName=${newNodeName}`, { method: "POST" });
        if (response.ok) {
            name = newNodeName;
            onChange(level);
        } else {
            const response_json = await response.json();
            onError(response_json.data.message);
        }
        setRenamePopupVisible(false);
    }

    const deleteNode = async () => {
        const response = await fetch(`api.user.tree.node.delete?treeName=${treeName}&nodeId=${id}`, { method: "POST" });
        if (response.ok) {
            onChange(level);
        } else {
            const response_json = await response.json();
            onError(response_json.data.message);
        }
    }
    
    return (
        <div className="node" style={{ marginLeft: 20 }}>
            <div className="node_head">
                <div className="icon">{children?.length > 0 && <i onClick={() => toggle()} className={isExpanded ? "fa-solid fa-minus" : "fa-solid fa-plus"} ></i>}</div>
                <div className="node_name">{name}</div>
                <div className="buttons">
                    <button onClick={() => setAddPopupVisible(true)}><i className="fa-solid fa-plus" ></i></button>&nbsp;
                    <button onClick={() => setRenamePopupVisible(true)}><i className="fa-solid fa-pen-to-square"></i></button>&nbsp;
                    <button onClick={() => { if (confirm('Are you sure?')) deleteNode(true) }}><i className="fa-solid fa-trash-can"></i></button>
                </div>

                {addPopupVisible &&
                    <Popup title="Add Node" handleClose={() => setAddPopupVisible(false)} handleSubmit={saveNewNode} >
                        <label htmlFor="nodename">Node name:</label>&nbsp;
                        <input type="text" name="nodename" id="nodename" required value={nodeName} onChange={e => setNodeName(e.target.value)} />
                    </Popup>
                }

                {renamePopupVisible &&
                    <Popup title="Rename Node" handleClose={() => setRenamePopupVisible(false) } handleSubmit={renameNode} >
                        <label htmlFor="newnodename">Node name:</label>&nbsp;
                        <input type="text" name="newnodename" id="newnodename" required value={newNodeName} onChange={e => setNewNodeName(e.target.value)} />
                    </Popup>
                }
            </div>

            {isExpanded &&
                children.map(node => {
                    return (
                        <TreeNode treeName={treeName} key={node.id} onChange={onChange} onError={onError} level={level + 1} {...node} />
                    )
                })    
            }
        </div>
    );
    
}

TreeNode.propTypes = {
    children: PropTypes.any,
    onChange: PropTypes.func,
    onError: PropTypes.func,
    treeName: PropTypes.string,
    name: PropTypes.string,
    id: PropTypes.number,
    level: PropTypes.number,
};

export default TreeNode;
