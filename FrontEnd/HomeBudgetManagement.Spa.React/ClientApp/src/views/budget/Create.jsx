// src/views/expense/CreateExpenseForm.jsx
import React, { useState } from 'react';

const CreateBudgetForm= () => {
    const [description, setDescription] = useState('');
    const [amount, setAmount] = useState('');

    const handleDescriptionChange = (e) => setDescription(e.target.value);
    const handleAmountChange = (e) => setAmount(e.target.value);

    const handleSubmit = async (e) => {
        e.preventDefault();

        try
        {
            var request = {
                description: description,
                amount: amount,
                type: 2
            };

            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3VzZXJkYXRhIjoia2VsdmluIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJOaWNrTmFtZSI6ImtlbHYiLCJleHAiOjE3MDgzNDAxNzQsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.WTPouTAoMouhagJx4JifkW5aP1Lm_IvMTVezJX-L4StCWYPhGgoD_u2zghCu-adp3jEl8n0lJ9cmAWjPyenKPg";
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
