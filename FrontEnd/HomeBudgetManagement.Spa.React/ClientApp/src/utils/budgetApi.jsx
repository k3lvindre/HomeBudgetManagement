import { getCookie } from './cookieManager'; 
import { signIn } from './authApi'; 

const baseUrl = "http://localhost:5143/api/sampleendpointmodificationthatmapstodownstream/budget/";
const token = await auth();

const auth = async () => {
    await signIn("kelvin2", "P@ssword12345");
    return getCookie('token');
}

export const getById = async (id) => {
    try
    {
        const response = await fetch(baseUrl + id, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });

        //we can use .then in fetch like
        //.then(response => {
        //    if (!response.ok) {
        //        console.log(`HTTP error! Status: ${response.status}`);
        //        setItems([]);
        //        setSum(0);
        //        return;
        //    }
        //    return response.json(); // Assuming the response is in JSON format
        //})
        //.then((data) => {
        //    setItems(data);
        //    setSum(computeSum(data));
        //});
        //BUT IN CASE YOU WANT TO DO IT MANUALLY 
        if (response.ok) {
            return await response.json();
        }
    }
    catch (error)
    {
        console.error('getById:', error);
        alert("getById Error Getting Item.");
    }
}

export const postBudget = async (request) => {
    try {
        const response = await fetch(baseUrl, {
            method: request.id == null ? 'POST' : 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(request),
        });

        if (response.ok) {
            alert("Operation Successful");
        }
        // Optionally, you can redirect or update the list
    } catch (error) {
        console.error('postBudget:', error);
        alert("postBudget Error Encountered");
    }
}

export const search = (request) => {
    console.log(request);

    // Fetch data from your API endpoint
    //Here we dont use await because we want to other task unlike in other method which the response needs to await so it can be read
    //but here we use .then so no need for await
    return fetch(baseUrl + 'search',
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
                return;
            }
            return response.json(); // Assuming the response is in JSON format
        })
        .then((data) => {
            return data;
        });
}

export const deleteBudget = (itemId) => {

    // Implement the delete logic here
    console.log(`Delete item with ID: ${itemId}`);

    // Fetch data from your API endpoint
    return fetch(baseUrl + itemId,
    {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    })
    .then(response => {
        if (!response.ok) {
            console.log(`HTTP error! Status: ${response.status}`);
            alert("Unable to delete.");
            return;
        }
        return response.json(); // Assuming the response is in JSON format
    })
    .then((data) => {
        return data;
    });
};