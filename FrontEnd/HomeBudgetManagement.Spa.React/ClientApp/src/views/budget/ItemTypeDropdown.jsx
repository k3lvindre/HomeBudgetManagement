// DropdownComponent.jsx
import React from 'react';
import Dropdown from 'react-bootstrap/Dropdown';
import ItemTypes from '../../common/ItemTypes'; // Import your enum

const DropdownComponent = ({ selectedType, onSelect }) => {
    var getKeyByValue = (object, value) => Object.keys(object).find(key => object[key] == value);

    return (
        <Dropdown onSelect={onSelect} required>
            <Dropdown.Toggle variant="success" id="dropdown-basic">
                {getKeyByValue(ItemTypes, selectedType) || 'Select Type'}
            </Dropdown.Toggle>

            <Dropdown.Menu>
                {Object.entries(ItemTypes).map(([key, value]) => (
                    <Dropdown.Item key={value} eventKey={value}>
                        {key}
                    </Dropdown.Item>
                ))}
            </Dropdown.Menu>
        </Dropdown>
    );
};

export default DropdownComponent;
