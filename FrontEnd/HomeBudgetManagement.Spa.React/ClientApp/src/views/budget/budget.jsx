// src/views/expense/CreateExpenseForm.jsx
import React, { useState, useEffect, memo, useRef } from 'react';
import DropdownComponent from '../budget/ItemTypeDropdown';
import { getById, postBudget } from '../../utils/budgetApi'; 
import ItemTypes from '../../common/itemTypes'; // Import your enum

//added props for the sake of example, in case we want to pass props to this component
const BudgetForm = (props) => {
    //var vs const
    //Avoid using var in modern JavaScript unless you have specific reasons for doing so.
    //var is function-scoped, not block - scoped.This can lead to unexpected behavior, especially with hoisting.
    //Variables declared with var are hoisted to the top of the function or global scope,
    //which can sometimes result in unintuitive behavior.
    //Best Practices:
    //Use const by default for variables that do not need reassignment.
    //Use let for variables that may need to be reassigned because it is mutable.
    //Avoid using var due to its function-scoping and potential hoisting issues 
    //but const and let are better because block scoped
    const [description, setDescription] = useState('');
    const [amount, setAmount] = useState();
    const [selectedType, setSelectedType] = useState(1);

    const handleDescriptionChange = (e) => setDescription(e.target.value);
    const handleAmountChange = (e) => setAmount(e.target.value);
    const handleSelect = (itemType) => setSelectedType(itemType);

    // Parse query parameters from URL
    const searchParams = new URLSearchParams(window.location.search);
    const id = searchParams.get('id');
    
    useEffect(() => {
        if (id > 0) loadData();
    },[])

    const loadData = async () => {
        var budget = await getById(id);
        setDescription(budget.description);
        setAmount(budget.amount);
        setSelectedType(ItemTypes[budget.type]);
    }

    const handleSubmit = async (e) => {
        e.preventDefault();

        var request = {
            id: id,
            description: description,
            price: amount,
            type: selectedType
        };

        await postBudget(request);
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

export default BudgetForm;
