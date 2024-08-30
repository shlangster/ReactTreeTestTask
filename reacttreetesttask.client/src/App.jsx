import { useEffect, useState } from 'react';
import Tree from './Tree'
import Popup from './Popup';
import ErrorMessage from './ErrorMessage';
import './App.css';

function App() {
    const [trees, setTrees] = useState([]);
    const [error, setError] = useState("");
    const [popupVisible, setPopupVisible] = useState(false);
    const [treeName, setTreeName] = useState("");

    useEffect(() => {
        populateTrees();
    }, []);

    const populateTrees = async () => {
        const response = await fetch('api.user.tree.list', { method: "POST" });
        const data = await response.json();
        if (response.ok) {
            setTrees([...data.map(_ => _.name)]);
        }
        else {
            setError(data.data.message);
        }
    }

    const addTree = async () => {
        const response = await fetch(`api.user.tree.get?treeName=${treeName}`, { method: "POST" });
        if (response.ok) {
            setTreeName("");
            setPopupVisible(false);
            await populateTrees();
        }
        else {
            const data = await response.json();
            setError(data.data.message);
        }
    }



    return (
        <div>
            {error && <ErrorMessage message={error} onClose={() => setError()} />}
        
            <div>
                <h1 id="tableLabel">Tree</h1><button onClick={() => setPopupVisible(true)}>Add New Tree</button><br /><br />
                {popupVisible &&
                    <Popup title="Add Tree" handleClose={() => setPopupVisible(false)} handleSubmit={() => addTree()} >
                        <label htmlFor="treename">Tree name:</label>&nbsp;
                        <input type="text" name="treename" id="treename" required value={treeName} onChange={e => setTreeName(e.target.value)} />
                    </Popup>
                }
                {trees.map(tree => { return (<Tree treeName={tree} reload={populateTrees} key={tree} onError={e => setError(e)} />) })}
            </div>
        </div>
    );
}

export default App;