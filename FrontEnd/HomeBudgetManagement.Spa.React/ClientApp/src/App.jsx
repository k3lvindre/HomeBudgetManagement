// src/App.js
import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import React from 'react';
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import Home from './views/index';
import CreateExpenseForm from './views/expense/Create';
import Nav from './Nav';

function App() {
    return (
        <div>
            <Nav logo={reactLogo} />
        
           <Routes>
               <Route exact path="/"  element={<Home />} />
                <Route path="/expense/create" element={<CreateExpenseForm />} />
           </Routes>
        </div>
    );
}

export default App;
