import React, { useState, useEffect } from 'react';

const BudgetList = () => {
    var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3VzZXJkYXRhIjoia2VsdmluMiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiTmlja05hbWUiOiJrZWx2IiwiZXhwIjoxNzA4Mzk4NzczLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAifQ.Wq5Lyn7xY4CU4sq1Wv_uisQ0w1J349Q2v1RLeDDzF0tRWviW3R-mf8sQoh6dfXrjifde0o5S2Dha1rzAJy3Jzg";

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
        getData(selectedSearchOption);
    }, [selectedSearchOption]); //in 2nd argument you can pass any value here(e.g selectedSearchOption) that if it changed it will rerun all the logic inside this so like tracking value changes then updates.

    var getData = (searchValue) => {
        // Fetch data from your API endpoint
        var request = {
            listOfId: null,
            type: searchValue == '0' ? null : searchValue
        };

        console.log(request);

        // Fetch data from your API endpoint
        fetch('http://localhost:5143/api/sampleendpointmodificationthatmapstodownstream/budget/search',
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(request),
            })
            .then(response => {
                if (!response.ok) {
                    console.log(`HTTP error! Status: ${response.status}`);
                    setItems([]);
                    setSum(0);
                    return;
                }
                return response.json(); // Assuming the response is in JSON format
            })
            .then((data) => {
                setItems(data);
                setSum(computeSum(data));
            });
    }

    const handleView = (itemId) => {
        // Implement the view logic here
        console.log(`View item with ID: ${itemId}`);
    };

    const handleUpdate = (itemId) => {
        // Implement the update logic here
        console.log(`Update item with ID: ${itemId}`);
    };

    const handleDelete = (itemId) => {
        // Implement the delete logic here
        console.log(`Delete item with ID: ${itemId}`);
    };

    const handleOptionChange = (event) => {
        var searchValue = event.target.value;
        setSelectedSearchOption(searchValue);
        console.log(selectedSearchOption);
    };


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
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {items?.map((item) => (
                        <tr key={item.id}>
                            <td>{item.id}</td>
                            <td>{item.description}</td>
                            <td>{item.amount}</td>
                            <td>
                                <button className="btn btn-info mr-2" onClick={() => handleView(item.id)}>View</button>
                                <button className="btn btn-warning mr-2" onClick={() => handleUpdate(item.id)}>Edit</button>
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
