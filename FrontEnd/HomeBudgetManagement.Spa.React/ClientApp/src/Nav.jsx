import React from 'react';
import { Link } from 'react-router-dom'; // If you're using React Router

const Nav = ({ logo }) => {
    return (
        <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
            <Link className="navbar-brand" to="/">
                {/* Fallback if the browser doesn't support SVG */}
                <img src={logo} alt="Logo" className="navbar-logo" />
            </Link>

            {/* Hamburger menu for mobile */}
            <button
                className="navbar-toggler"
                type="button"
                data-toggle="collapse"
                data-target="#navbarNav"
                aria-controls="navbarNav"
                aria-expanded="false"
                aria-label="Toggle navigation"
            >
                <span className="navbar-toggler-icon"></span>
            </button>

            <div className="collapse navbar-collapse" id="navbarNav">
                <ul className="navbar-nav ml-auto">
                    <li className="nav-item">
                        <Link className="nav-link" to="/">Home</Link>
                    </li>
                    <li className="nav-item">
                        <Link className="nav-link" to="/expense/create">Create Expense</Link>
                    </li>
                    <li className="nav-item">
                        <Link className="nav-link" to="/about">About</Link>
                    </li>
                    {/* Add more navigation items as needed */}
                </ul>
            </div>
        </nav>
    );
};

export default Nav;
