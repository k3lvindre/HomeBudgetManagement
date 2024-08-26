// src/components/OrderForm.js
import React, { useState, useEffect } from "react";
import { getAll } from '../../utils/budgetApi'; 

const OrderForm = () => {
    const [orderItems, setOrderItems] = useState([{ product: "", quantity: 1 }]);
    const [products, setProducts] = useState([]);

    const fetchProducts = async () => {
        await getAll().then((data) => {
            if (data) {
                setProducts(data);
            } else {
                setProducts([]);
            }
        });
    }

    useEffect(() => {
        fetchProducts();
    }, []); 
    
    const handleInputChange = (index, event) => {
        const values = [...orderItems];
        if (event.target.name === "product") {
            values[index].product = event.target.value;
        } else if (event.target.name === "quantity") {
            values[index].quantity = event.target.value;
        }
        setOrderItems(values);
    };

    const handleAddItem = () => {
        setOrderItems([...orderItems, { product: "", quantity: 1 }]);
    };

    const handleRemoveItem = (index) => {
        const values = [...orderItems];
        values.splice(index, 1);
        setOrderItems(values);
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log("Order Submitted: ", orderItems);
        // Submit order logic here
    };

    return (
        <form onSubmit={handleSubmit}>
            <h1>Order Form</h1>
            {orderItems.map((item, index) => (
                <div key={index} style={{ marginBottom: "10px" }}>
                    <label>Product:</label>
                    <select
                        name="product"
                        value={item.product}
                        onChange={(event) => handleInputChange(index, event)}
                        required
                    >
                        <option value="" disabled>Select a product</option>
                        {products.map((product) => (
                            <option key={product.id} value={product.id}>
                                {product.name}
                            </option>
                        ))}
                    </select>
                    <label>Quantity:</label>
                    <input
                        type="number"
                        name="quantity"
                        value={item.quantity}
                        onChange={(event) => handleInputChange(index, event)}
                        min="1"
                        required
                    />
                    <button
                        type="button"
                        onClick={() => handleRemoveItem(index)}
                        disabled={orderItems.length === 1}
                    >
                        Remove
                    </button>
                </div>
            ))}
            <button type="button" onClick={handleAddItem}>
                Add Another Product
            </button>
            <button type="submit">Submit Order</button>
        </form>
    );
};

export default OrderForm;
