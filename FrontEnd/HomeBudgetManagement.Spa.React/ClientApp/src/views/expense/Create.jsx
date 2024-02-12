// src/views/expense/CreateExpenseForm.jsx
import React, { useState } from 'react';

const CreateExpenseForm = () => {
    const [description, setProductName] = useState('');
    const [amount, setProductPrice] = useState('');

    const handleNameChange = (e) => setProductName(e.target.value);
    const handlePriceChange = (e) => setProductPrice(e.target.value);

    const handleSubmit = async (e) => {
        e.preventDefault();

        try
        {
            var expense = {
                description: description,
                amount: amount
            };

            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3VzZXJkYXRhIjoia2VsdmluIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJOaWNrTmFtZSI6ImtlbHYiLCJleHAiOjE3MDY4MjkxMzgsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9._pjmF6G-5zegVL2Pmguu5wULGf_07_EYVXrT9QeQzwSnQZ92F5afsfKhS_vICz2UehQ7Fe6Usg3WgSD95_2MTA";
            const response = await fetch('http://localhost:5143/api/sampleendpointmodificationthatmapstodownstream/expenses', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(expense),
            });

            if (response.ok) {
                console.log('Expense created');
            }
            // Optionally, you can redirect or update the product list
        } catch (error) {
            console.error('Error creating expense:', error);
        }
    };

    return (
        <div className="container mt-5">
              <h2 className="mb-4">Create a New Expense</h2>
              <form onSubmit={handleSubmit}>
                    <div className="form-group">
                      <label htmlFor="description">Description</label>
                      <input
                        type="text"
                        className="form-control"
                        id="description"
                        value={description}
                        onChange={handleNameChange}
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
                        onChange={handlePriceChange}
                        required
                      />
                    </div>
                    <button type="submit" className="btn btn-primary">
                      Create Expense
                    </button>
              </form>
        </div>
    );
};

export default CreateExpenseForm;
