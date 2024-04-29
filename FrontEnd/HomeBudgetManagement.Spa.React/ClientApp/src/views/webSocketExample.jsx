import React, { useEffect, useState } from 'react';

function WebSocketPage() {
    const [messages, setMessages] = useState([]);
    const [websocket, setWebsocket] = useState(null);
    const [message, setMessage] = useState('');
    const [receivedData, setReceivedData] = useState([]);
    const [fileName, setFileName] = useState(''); // Default file name

    useEffect(() => {
        // Create WebSocket connection.
        const ws = new WebSocket('ws://localhost:5000/ws');

        // Connection opened
        ws.onopen = () => {
            console.log('WebSocket Connected');
        };

        // Listen for messages
        ws.onmessage = (event) => {
            console.log('Message from server ', event.data);
            setMessages(prev => [...prev, event.data]);

            if (typeof event.data === "string") {
                //setFileName(event.data);
                setFileName("x.jpg");
            } else {
                const reader = new FileReader();
                reader.onload = () => {
                    setReceivedData(reader.result);
                };
                reader.readAsArrayBuffer(event.data);
            }

            handleDownload();
        };

        // Listen for possible errors
        ws.onerror = (event) => {
            console.error('WebSocket error observed:', event);
        };

        // Connection closed
        ws.onclose = (event) => {
            console.log('WebSocket is closed now.', event);
        };

        // Assign websocket connection to state
        setWebsocket(ws);

        // Cleanup function
        return () => {
            ws.close();
        };
    }, []);

    const handleMessageChange = (event) => {
        const { name, value } = event.target;
        setMessage(value);
    }
    
    const sendMessage = () => {
        websocket.send(message);
    }

    // Function to handle download
    const handleDownload = () => {
        const element = document.createElement("a");
        const file = new Blob([receivedData], { type: 'application/octet-stream' });
        element.href = URL.createObjectURL(file);
        element.download = fileName;
        document.body.appendChild(element); // Required for this to work in FireFox
        element.click();
        document.body.removeChild(element);
    };

    return (
        <div>
            <h1>WebSocket Messages</h1>
            <ul>
                {messages.map((msg, index) => (
                    <li key={index}>{msg}</li>
                ))}
            </ul>
            <div className="row">
                <label>Message:</label>
                <div className="input-group">
                    <input
                        type="text"
                        className="form-control"
                        onChange={handleMessageChange}
                        name="message"
                    />
                    <div className="input-group-append">
                        <button className="btn btn-outline-secondary" type="button" onClick={sendMessage}>
                            Search
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default WebSocketPage;
