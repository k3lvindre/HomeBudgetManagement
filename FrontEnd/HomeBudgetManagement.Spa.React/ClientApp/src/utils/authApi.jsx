const baseUrl = "http://localhost:5143/api/Identity/signin";

export const signIn = async (username, password) =>
{
    try {
        var creds = {
            "userName": username,
            "password": password
        }

        const response = await fetch(baseUrl, {
            method: 'POST',
            body: JSON.stringify(creds),
            headers: {
                'Content-Type': 'application/json'
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
            var result = await response.text();
            const expirationDate = new Date();
            expirationDate.setDate(expirationDate.getDate() + 1);
            const cookieValue = encodeURIComponent('token') + '=' + encodeURIComponent(result) + '; expires=' + expirationDate.toUTCString() + '; path=/';
            document.cookie = cookieValue;
        }
    }
    catch (error) {
        console.error('Signin Error:', error);
    }
}


