// src/App.js
import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import React from 'react';
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import Home from './views/index';
import BudgetForm from './views/budget/budget';
import OrderForm from './views/order/order';
import WebSocketPage from './views/webSocketExample';
import Nav from './Nav';

function App() {
    return (
        <div>
            <Nav logo={reactLogo} />
        
           <Routes>
               <Route exact path="/"  element={<Home />} />
               <Route path="/budget/budget" element={<BudgetForm />} />
               <Route path="/order" element={<OrderForm />} />
               <Route path="/websocketpage" element={<WebSocketPage />} />
           </Routes>
        </div>
    );
}

export default App;
