import React, { useState, useEffect } from 'react';

const ItemList = () => {
    const [items, setItems] = useState([]);
    const [sum, setSum] = useState(0);
    const [selectedSearchOption, setSelectedSearchOption] = useState('0');

    var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3VzZXJkYXRhIjoia2VsdmluIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJOaWNrTmFtZSI6ImtlbHYiLCJleHAiOjE3MDgzNDAxNzQsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.WTPouTAoMouhagJx4JifkW5aP1Lm_IvMTVezJX-L4StCWYPhGgoD_u2zghCu-adp3jEl8n0lJ9cmAWjPyenKPg";

    useEffect(() =>
    {
        getData();
    }, []);

    var getData = () => {
        // Fetch data from your API endpoint
        fetch('http://localhost:5143/api/sampleendpointmodificationthatmapstodownstream/budget',
            {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            })
            .then((response) => response.json())
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

        if (searchValue == '0') {
            getData();
            return;
        }

        var request = {
            listOfId: null,
            type: searchValue
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


        setSelectedSearchOption(event.target.value);
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

export default ItemList;
