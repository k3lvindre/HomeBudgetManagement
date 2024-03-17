import React, { useState, useEffect, useRef } from 'react';
import { deleteBudget, search} from '../../utils/budgetApi'; 

const BudgetList = () => {
    const [items, setItems] = useState([]);
    const [sum, setSum] = useState(0);
    const [selectedSearchOption, setSelectedSearchOption] = useState('0');

    //useEffect takes a function as its first argument, and the second argument is an array of dependencies. 
    //If the dependency array is empty([]), the effect will run only once after the initial render, 
    //simulating the behavior of componentDidMount.If you provide dependencies, the effect will re - run whenever
    //any of those dependencies change.
    //In summary, componentDidMount is used in class components, and useEffect is used in functional components to achieve similar effects.
    useEffect(() =>
    {
        loadData(selectedSearchOption);
    }, [selectedSearchOption]); //in 2nd argument you can pass any value here(e.g selectedSearchOption) that if it changed it will rerun all the logic inside this so like tracking value changes then updates.

    const loadData = (searchValue) => {
        // Fetch data from your API endpoint
        var request = {
            listOfId: null,
            type: searchValue == '0' ? null : searchValue
        };

        search(request).then((data) => {
            if (data) {
                setItems(data);
                setSum(computeSum(data));
            } else {
                setItems([]);
                setSum(0);
            }
        });
    }

    const handleView = (itemId) => {
        // Implement the view logic here
        console.log(`View item with ID: ${itemId}`);

        // Get the base URL
        var baseUrl = window.location.origin;

        // Specify the path for the new page
        var newPath = `/budget/budget?id=` + itemId;

        // Construct the new URL by combining the base URL and the new path
        var newUrl = baseUrl + newPath;

        // Redirect to the new URL
        window.location.href = newUrl;
    };

    const handleDelete = (itemId) => {
        deleteBudget(itemId).then(() => {
            loadData('0');
        });
    };

    const handleOptionChange = (event) => {
        var searchValue = event.target.value;
        setSelectedSearchOption(searchValue);
    };

    //.map() - Maps over each item in the list and extracts the amount property from each item.
    //It transforms the original array of objects into an array of amounts.
    //
    var computeSum = (list) => list.map(item => item.amount).reduce((acc, currentValue) => acc + currentValue, 0);

    const inlineStyle = {
        fontSize: '18px',
        fontWeight: 'bold',
    };

    return (
        <div className="container mt-5">
            <div class="row">
                <p>Filter by:</p>

                <div className="btn-group btn-group-toggle" data-toggle="buttons">
                    <label className={`btn btn-outline-primary ${selectedSearchOption === '0' ? 'active' : ''}`}>
                        <input
                            type="radio"
                            name="radioOptions"
                            id="searchOption"
                            value="0"
                            checked={selectedSearchOption === '0'}
                            onChange={handleOptionChange}
                        />
                        All
                    </label>

                    <label className={`btn btn-outline-primary ${selectedSearchOption === '1' ? 'active' : ''}`}>
                        <input
                            type="radio"
                            name="radioOptions"
                            id="searchOption"
                            value="1"
                            checked={selectedSearchOption === '1'}
                            onChange={handleOptionChange}
                        />
                        Expense
                    </label>

                    <label className={`btn btn-outline-primary ${selectedSearchOption === '2' ? 'active' : ''}`}>
                        <input
                            type="radio"
                            name="radioOptions"
                            id="searchOption"
                            value="2"
                            checked={selectedSearchOption === '2'}
                            onChange={handleOptionChange}
                        />
                        Income
                    </label>

                    <label className={`btn btn-outline-primary ${selectedSearchOption === '3' ? 'active' : ''}`}>
                        <input
                            type="radio"
                            name="radioOptions"
                            id="searchOption"
                            value="3"
                            checked={selectedSearchOption === '3'}
                            onChange={handleOptionChange}
                        />
                        Savings
                    </label>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <p style={inlineStyle}>Items</p>
                </div>
                <div class="col-md-4">
                    <p>Total:{items?.length??0}</p>
                </div>
                <div class="col-md-4">
                    <p>Sum:{sum}</p>
                </div>
            </div>
            
            <table className="table">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Description</th>
                        <th>Amount</th>
                        <th>Type</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {items?.map((item) => (
                        <tr key={item.id}>
                            <td>{item.id}</td>
                            <td>{item.description}</td>
                            <td>{item.amount}</td>
                            <td>{item.type}</td>
                            <td>
                                <button className="btn btn-info mr-2" onClick={() => handleView(item.id)}>View</button>
                                <button className="btn btn-danger" onClick={() => handleDelete(item.id)}>Delete</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default BudgetList;
