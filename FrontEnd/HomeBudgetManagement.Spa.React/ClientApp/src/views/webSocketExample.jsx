import React, { useEffect, useState } from 'react';

function WebSocketPage() {
    const [messages, setMessages] = useState([]);
    const [websocket, setWebsocket] = useState(null);
    const [message, setMessage] = useState('');
    const [receivedBytes, setReceivedBytes] = useState([]);
    const [fileName, setFileName] = useState('image.jpg'); // Default file name

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

            if (typeof event.data === "string") {
                setMessages(prev => [...prev, event.data]);
            } else if (event.data instanceof Blob) {
                // Assuming the whole file is sent in one chunk
                setFileName("something.jpg");
                setReceivedBytes(event.data);
            } 
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

    useEffect(() => {
        if (receivedBytes.size > 0) handleDownload(receivedBytes);
    }, [receivedBytes])

    const handleMessageChange = (event) => {
        const { name, value } = event.target;
        setMessage(value);
    }
    
    const sendMessage = () => {
        websocket.send(message);
    }

    // Function to handle download
    const handleDownload = (bytes) => {
        const blob = new Blob([bytes], { type: 'application/octet-stream' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        const list = document.createElement('li');
        a.href = url;
        a.download = fileName;
        a.innerText = "Download File";
        list.appendChild(a);
        var element = document.getElementById('file');
        element.appendChild(list);
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
                <ul id="file">
                </ul>
            </div>
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
