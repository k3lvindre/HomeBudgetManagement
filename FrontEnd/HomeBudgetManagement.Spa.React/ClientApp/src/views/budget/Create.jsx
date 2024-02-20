// src/views/expense/CreateExpenseForm.jsx
import React, { useState } from 'react';
import DropdownComponent from './ItemTypeDropdown';

const CreateBudgetForm = () => {
    var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3VzZXJkYXRhIjoia2VsdmluMiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiTmlja05hbWUiOiJrZWx2IiwiZXhwIjoxNzA4Mzk4NzczLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAifQ.Wq5Lyn7xY4CU4sq1Wv_uisQ0w1J349Q2v1RLeDDzF0tRWviW3R-mf8sQoh6dfXrjifde0o5S2Dha1rzAJy3Jzg";

    const [description, setDescription] = useState('');
    const [amount, setAmount] = useState();
    const [selectedType, setSelectedType] = useState(1);

    const handleDescriptionChange = (e) => setDescription(e.target.value);
    const handleAmountChange = (e) => setAmount(e.target.value);
    const handleSelect = (itemType) => setSelectedType(itemType);

    const handleSubmit = async (e) => {
        e.preventDefault();

        try
        {
            var request = {
                description: description,
                amount: amount,
                type: selectedType
            };

            const response = await fetch('http://localhost:5143/api/sampleendpointmodificationthatmapstodownstream/budget', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(request),
            });

            if (response.ok) {
                console.log('Item reated.');
            }
            // Optionally, you can redirect or update the list
        } catch (error) {
            console.error('Error creating item:', error);
        }
    };

    return (
        <div className="container mt-5">
              <h2 className="mb-4">Create an Item</h2>
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label htmlFor="Type">Type</label>
                    <DropdownComponent selectedType={selectedType} onSelect={handleSelect} />
                </div>
                <div className="form-group">
                      <label htmlFor="description">Description</label>
                      <input
                        type="text"
                        className="form-control"
                        id="description"
                        value={description}
                        onChange={handleDescriptionChange}
                        required
                      />
                    </div>
                    <div className="form-group">
                      <label htmlFor="amount">Amount</label>
                      <input
                        type="number"
                        className="form-control"
                        id="amount"
                        value={amount}
                        onChange={handleAmountChange}
                        required
                      />
                    </div>
                    <button type="submit" className="btn btn-primary">
                      Save
                    </button>
              </form>
        </div>
    );
};

export default CreateBudgetForm;
