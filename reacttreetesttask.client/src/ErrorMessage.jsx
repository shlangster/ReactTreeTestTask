import PropTypes from 'prop-types';
import './ErrorMessage.css';

function ErrorMessage({ onClose, message }) {

    return (
        <div>
            <div className="alert">
                <span className="closebtn" onClick={onClose}>&times;</span>
                {message}
            </div> 
        </div>
    );
}

ErrorMessage.propTypes = {
    onClose: PropTypes.func,
    message: PropTypes.string,
};

export default ErrorMessage;