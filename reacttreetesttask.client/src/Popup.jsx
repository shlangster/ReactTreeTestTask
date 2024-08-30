import PropTypes from 'prop-types';
import './Popup.css';

function Popup({ title, children, handleSubmit, handleClose }) {

    const handleFormEvent = (e) => {
        e.preventDefault();
        handleSubmit();
    }

    return (
        <div className="backdrop">
            <form onSubmit={handleFormEvent}>
                <div className="modal">
                    <div className="modal-body">
                        <div className='modal-name'>{title}</div>
                        <div className="modal-content">
                            {children}
                        </div>
                    </div>
                    <button className="submit-btn" type="submit">Save</button>&nbsp;
                    <button className="close-btn" type="button" onClick={() => handleClose()}>Close</button>
                </div>
            </form>
        </div>
    );
}

Popup.propTypes = {
    children: PropTypes.any,
    handleClose: PropTypes.func,
    handleSubmit: PropTypes.func,
    title: PropTypes.string
};

export default Popup;