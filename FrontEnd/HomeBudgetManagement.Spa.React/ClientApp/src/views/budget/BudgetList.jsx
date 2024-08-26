import React, { useState, useEffect, useRef } from 'react';
import { deleteBudget, search} from '../../utils/budgetApi'; 

const BudgetList = () => {
    const [items, setItems] = useState([]);
    const [sum, setSum] = useState(0);
    const [dateRange, setDateRange] = useState({dateFrom: '', dateTo: ''});
    const [selectedItemType, setSelectedItemType] = useState('0');

    //useEffect takes a function as its first argument, and the second argument is an array of dependencies. 
    //If the dependency array is empty([]), the effect will run only once after the initial render, 
    //simulating the behavior of componentDidMount.If you provide dependencies, the effect will re - run whenever
    //any of those dependencies change.
    //In summary, componentDidMount is used in class components, and useEffect is used in functional components to achieve similar effects.
    useEffect(() =>
    {
        loadData();
    },[]); //in 2nd argument you can pass any value here(e.g selectedItemType) that if it changed it will rerun all the logic inside this so like tracking value changes then updates.

    const loadData = async () => {
        // Fetch data from your API endpoint
        var request = {
            listOfId: null,
            type: selectedItemType == '0' ? null : selectedItemType,
            dateFrom: dateRange.dateFrom,
            dateTo: dateRange.dateTo,
        };

        await search(request).then((data) => {
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

    const handleDelete = async (itemId) => {
        await deleteBudget(itemId).then(async () => {
            await loadData('0');
        });
    };

    const handleOptionChange = (event) => {
        var itemType = event.target.value;
        setSelectedItemType(itemType);
    };

    const handleDateChange = (event) => {
        const { name, value } = event.target;
        dateRange[name] = value;
        setDateRange(dateRange);
    };

    const handleSearch = async (event) => {
        await loadData();
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
            <div className="row">
                <p>Filter by:</p>

                <div className="btn-group btn-group-toggle" data-toggle="buttons">
                    <label className={`btn btn-outline-primary ${selectedItemType === '0' ? 'active' : ''}`}>
                        <input
                            type="radio"
                            name="itemTypeOptions"
                            id="itemType"
                            value="0"
                            checked={selectedItemType === '0'}
                            onChange={handleOptionChange}
                        />
                        All
                    </label>

                    <label className={`btn btn-outline-primary ${selectedItemType === '1' ? 'active' : ''}`}>
                        <input
                            type="radio"
                            name="itemTypeOptions"
                            id="itemType"
                            value="1"
                            checked={selectedItemType === '1'}
                            onChange={handleOptionChange}
                        />
                        Expense
                    </label>

                    <label className={`btn btn-outline-primary ${selectedItemType === '2' ? 'active' : ''}`}>
                        <input
                            type="radio"
                            name="itemTypeOptions"
                            id="itemType"
                            value="2"
                            checked={selectedItemType === '2'}
                            onChange={handleOptionChange}
                        />
                        Income
                    </label>

                    <label className={`btn btn-outline-primary ${selectedItemType === '3' ? 'active' : ''}`}>
                        <input
                            type="radio"
                            name="itemTypeOptions"
                            id="itemType"
                            value="3"
                            checked={selectedItemType === '3'}
                            onChange={handleOptionChange}
                        />
                        Savings
                    </label>
                </div>
            </div>

            <div className="row">
                <label>Date Range:</label>
                <div className="input-group">
                    <input
                        type="date"
                        className="form-control"
                        onChange={handleDateChange}
                        placeholder="From"
                        name="dateFrom"
                    />
                    <input
                        type="date"
                        className="form-control"
                        onChange={handleDateChange}
                        placeholder="To"
                        name="dateTo"
                    />
                    <div className="input-group-append">
                        <button className="btn btn-outline-secondary" type="button" onClick={handleSearch}>
                            Search
                        </button>
                    </div>
                </div>
            </div>

            <div className="row">
                <div className="col-md-4">
                    <p style={inlineStyle}>Items</p>
                </div>
                <div className="col-md-4">
                    <p>Total:{items?.length??0}</p>
                </div>
                <div className="col-md-4">
                    <p>Sum:{sum}</p>
                </div>
            </div>
            
            <table className="table">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Date</th>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Price</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {items?.map((item) => (
                        <tr key={item.id}>
                            <td>{item.id}</td>
                            <td>{new Date(item.createddate).toLocaleDateString()}</td>
                            <td>{item.name}</td>
                            <td>{item.description}</td>
                            <td>{item.price}</td>
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
